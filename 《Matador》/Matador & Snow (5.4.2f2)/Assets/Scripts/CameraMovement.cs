using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

    /// <summary>
    /// 声明一个类型为Collider的变量target
    /// 用来存放目标对象上的Collider组件
    /// </summary>
    public Collider target;

    /// <summary>
    /// 声明一个类型为Camera的变量camera
    /// 用来存放Player对象的子对象Main Camera
    /// </summary>
    public Camera camera;

    /// <summary>
    /// 定义一个类型为LayerMask的变量obstacleLayers，其值为-1
    /// 用于指定FixedUpdate函数中球形探测的碰撞层
    /// </summary>
    public LayerMask obstacleLayers = -1;

    /// <summary>
    /// 定义一个类型为LayerMask的变量groundLayers，其值为-1
    /// 用于指定FixedUpdate函数中确定摄像机是否着地的线性探测的碰撞层
    /// </summary>
    public LayerMask groundLayers = -1;


    public float groundedCheckOffset = 0.7f;
    public float rotationUpdateSpeed = 60.0f;
    public float lookUpSpeed = 20.0f;
    public float distanceUpdateSpeed = 10.0f;
    public float followUpdateSpeed = 10.0f;
    public float maxForwardAngle = 80.0f;
    public float minDistance = 0.1f;
    public float maxDistance = 10.0f;
    public float zoomSpeed = 1.0f;
    public bool requireLock = true;
    public bool controlLock = true;
    private const float movementThreshold = 0.1f;
    private const float rotationThreshold = 0.1f;
    private const float groundedDistance = 0.5f;

    /// <summary>
    /// 声明一个类型为Vector3的变量lastStationaryPosition
    /// 表示target对象上次驻停的位置
    /// </summary>
    private Vector3 lastStationaryPosition;


    private float optimalDistance;
    private float targetDistance;
    private bool grounded = false;


    /// <summary>
    /// 声明一个类型为float的属性ViewRadius
    /// 这是一个属性！
    /// 用来表示视野半径
    /// </summary>
    float ViewRadius
    {
        get
        {
            float fieldOfViewRadius = (optimalDistance * Mathf.Tan(camera.fieldOfView / 2.0f) * Mathf.Deg2Rad);
            float doubleCharacterRadius = Mathf.Max(target.bounds.extents.x, target.bounds.extents.z) * 2.0f;
            return Mathf.Min(doubleCharacterRadius, fieldOfViewRadius);         //取二者中的较小者
        }
    }


    /// <summary>
    /// 声明一个类型为Vector3的属性SnappedCameraForward
    /// 这是一个属性！
    /// 根据Player对象的方向修正后的摄像机位置
    /// </summary>
    Vector3 SnappedCameraForward
    {
        get
        {
            Vector3 f = camera.transform.forward;
            Vector2 planeForward = new Vector2(f.x, f.z);
            planeForward = new Vector2(target.transform.forward.x, target.transform.forward.z).normalized * planeForward.magnitude;
            return new Vector3(planeForward.x, f.y, planeForward.y);
        }
    }

    /// <summary>
    /// Use this for initialization（初始化各个变量）
    /// </summary>
    void Start()
    {
        //如果target变量为空，则为target赋值
        if (target == null)
        {
            target = GameObject.Find("Player").GetComponent<Collider>();      //将Player对象上面的Collider组件赋值给target
        }

        //如果camera为空，且Camera.main不为空，则将Camera.main赋值给camera
        if (camera == null && Camera.main != null)
        {
            camera = Camera.main;       //将Camera.main赋值给camera
        }

        //如果判断target还是为空，则...
        if (target == null)
        {
            Debug.LogError("Player对象上未添加Collider组件");       //打印Debug
            enabled = false;          //禁用该脚本
            return;         //退出本函数
        }

        //如果判断camera还是为空，则...
        if (camera == null)
        {
            Debug.LogError("camera未赋值，未添加Camera.main对象");       //打印Debug
            enabled = false;        //禁用该脚本
            return;     //退出本函数
        }

        lastStationaryPosition = target.transform.position;     //将lastStationaryPosition的位置设置为Player对象的位置

        targetDistance = optimalDistance = (camera.transform.position - target.transform.position).magnitude;       //targetDistance和optimalDistance均设置为Player对象到camera的距离
    }



    private void FixedUpdate()
    {
        grounded = Physics.Raycast(camera.transform.position + target.transform.up * -groundedCheckOffset, target.transform.up * -1, groundedDistance, groundLayers);

        Vector3 inverseLineOfSight = camera.transform.position - target.transform.position;         //表示Player对象到摄像机的矢量

        RaycastHit hit;

        if (Physics.SphereCast(target.transform.position, ViewRadius, inverseLineOfSight, out hit, optimalDistance, obstacleLayers))
        {
            targetDistance = Mathf.Min((hit.point - target.transform.position).magnitude, optimalDistance);
        }

        else
        {
            targetDistance = optimalDistance;
        }
    }



    // Update is called once per frame
    void Update()
    {
        optimalDistance = Mathf.Clamp(optimalDistance + Input.GetAxis("Mouse ScrollWheel") /*获得鼠标滚轮的输入量*/* -zoomSpeed * Time.deltaTime, minDistance, maxDistance);      //将optimalDistance设置为...，其值限定在minDistance和maxDistance之间
    }




    private void LateUpdate()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) && (!requireLock || controlLock || Cursor.lockState == CursorLockMode.Locked))
        {
            if (controlLock)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            FreeUpdate();
            lastStationaryPosition = target.transform.position;
        }

        else
        {
            if (controlLock)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            Vector3 movement = target.transform.position - lastStationaryPosition;
            if (new Vector2(movement.x, movement.z).magnitude > movementThreshold)
            {
                FollowUpdate();
            }
        }

        DistanceUpdate();
    }



    void FollowUpdate()
    {
        Vector3 cameraForward = target.transform.position - camera.transform.position;

        cameraForward = new Vector3(cameraForward.x, 0.0f, cameraForward.z);        //表示摄像机到目标对象的矢量

        float rotationAmount = Vector3.Angle(cameraForward, target.transform.forward);      //表示cameraForward和目标对象之间的夹角

        //如果夹角小于rotationThreshold，则更新lastStationaryPosition
        if (rotationAmount < rotationThreshold)
        {
            lastStationaryPosition = target.transform.position;
        }

        rotationAmount *= followUpdateSpeed * Time.deltaTime;

        //如果cameraForward月player对象右侧的夹角小于左侧的夹角，则rotationAmount乘以-1
        if (Vector3.Angle(cameraForward, target.transform.right) < Vector3.Angle(cameraForward, target.transform.right * -1.0f))
        {
            rotationAmount *= -1.0f;
        }

        camera.transform.RotateAround(target.transform.position, Vector3.up, rotationAmount);       //将摄像机绕Y轴旋转rotationAmount
    }



    void FreeUpdate()
    {
        float rotationAmount;

        //当单击鼠标右键时
        if (Input.GetMouseButton(1))
        {
            FollowUpdate();
        }

        //否则根据MouseX轴的输入值来确定rotationAmount
        else
        {
            rotationAmount = Input.GetAxis("Mouse X") * rotationUpdateSpeed * Time.deltaTime;
            camera.transform.RotateAround(target.transform.position, Vector3.up, rotationAmount);       //将摄像机绕Y轴旋转rotationAmount
        }

        rotationAmount = Input.GetAxis("Mouse Y") * -1.0f * lookUpSpeed * Time.deltaTime;

        bool lookFromBelow = Vector3.Angle(camera.transform.forward, target.transform.up * -1) > Vector3.Angle(camera.transform.forward, target.transform.up);      //当摄像机前向与target.transform.up夹角大于与-target.transform.up的夹角时，表明是相机从底部向上观察target对象，此时lookFromBelow为true

        if (grounded && lookFromBelow)
        {
            camera.transform.RotateAround(camera.transform.position, camera.transform.right, rotationAmount);
        }

        else
        {
            camera.transform.RotateAround(target.transform.position, camera.transform.right, rotationAmount);

            camera.transform.LookAt(target.transform.position);

            float forwardAngle = Vector3.Angle(target.transform.forward, SnappedCameraForward);

            if (forwardAngle > maxForwardAngle)
            {
                camera.transform.RotateAround(target.transform.position, camera.transform.right, lookFromBelow ? forwardAngle - maxForwardAngle : maxForwardAngle - forwardAngle);
            }
        }
    }


    /// <summary>
    /// 用来更新摄像机的位置
    /// </summary>
    void DistanceUpdate()
    {
        Vector3 dir = (camera.transform.position - target.transform.position).normalized;       //表示target对象到摄像机的方向

        Vector3 targetPosition = target.transform.position + dir * targetDistance;      //表示target对象的目标位置，其值为当前位置沿着dir方向向前移动targetDistance

        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, Time.deltaTime * distanceUpdateSpeed);
    }


}
