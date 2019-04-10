using UnityEngine;
using System.Collections;

/// <summary>
/// 声明一个委托MyJumpdelegate（）
/// </summary>
public delegate void MyJumpdelegate();

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 声明一个类型为Rigidbody的变量target
    /// 用来存放当前游戏对象上的Rigidbody组件，通过调用Rigidbody中的某些API来实现一些效果
    /// 可以赋值搭载组件的当前游戏对象；这里也可不赋值，下面将会在Start函数中进行初始化赋值
    /// </summary>
    public Rigidbody target;

    /// <summary>
    /// 定义一个类型为float的变量speed，值为...
    /// </summary>
    [HideInInspector]
    public float speed = 1.0f;

    /// <summary>
    /// 定义一个类型为float的变量walkSpeedDownscale，值为...
    /// </summary>
    [HideInInspector]
    public float walkSpeedDownscale = 2.0f;

    /// <summary>
    /// 定义一个类型为float的变量turnSpeed，值为...
    /// 表示用键盘操作target对象旋转时的旋转速度
    /// </summary>
    [HideInInspector]
    public float turnSpeed = 2.0f;

    /// <summary>
    /// 定义一个类型为float的变量mouseTurnSpeed，值为...
    /// 表示用鼠标操作target对象旋转时的旋转速度
    /// </summary>
    [HideInInspector]
    public float mouseTurnSpeed = 0.3f;

    /// <summary>
    /// 定义一个类型为float的变量jumpSpeed，值为...
    /// 表示target对象跳跃时的速度
    /// </summary>
    [HideInInspector]
    public float jumpSpeed = 1.0f;


    /// <summary>
    /// 定义一个类型为LayerMask的变量groundLayers，值为...
    /// 表示参与射线探测的各个层的掩码
    /// </summary>
    [HideInInspector]
    public LayerMask groundLayers = -1;

    /// <summary>
    /// 定义一个类型为float的变量groundedCheckOffset，值为...
    /// </summary>
    [HideInInspector]
    public float groundedCheckOffset = 0.7f;

    /// <summary>
    /// 定义一个类型为bool的变量showGizmos，初始值为true
    /// 用来判断Scene窗口中是否有小部件
    /// </summary>
    [HideInInspector]
    public bool showGizmos = true;

    /// <summary>
    /// 定义一个类型为bool的变量requireLock，初始值为true
    /// </summary>
    [HideInInspector]
    public bool requireLock = true;

    /// <summary>
    /// 定义一个类型为bool的变量controlLock，初始值false
    /// </summary>
    [HideInInspector]
    public bool controlLock = false;

    /// <summary>
    /// 定义一个类型为MyJumpdelegate的变量 onJump，值为null
    /// 这是一个委托变量onJump
    /// </summary>
    public MyJumpdelegate onJump = null;

    /// <summary>
    /// 定义一个类型为const float的变量inputThreshold，值为...
    /// </summary>
    private const float inputThreshold = 0.01f;

    /// <summary>
    /// 定义一个类型为const float的变量groundDrag，初始值为5.0
    /// 用来设置Rigidbody组件下面的drag属性值
    /// </summary>
    private const float groundDrag = 5.0f;

    /// <summary>
    /// 定义一个类型为const float的变量directionalJumpFactor，值为...
    /// </summary>
    private const float directionalJumpFactor = 0.7f;

    /// <summary>
    /// 定义一个类型为const float的变量groundedDistance，值为...
    /// </summary>
    private const float groundedDistance = 0.5f;

    /// <summary>
    /// 声明一个类型为bool的变量grounded，初始值为false
    /// 用来判断target对象是否着地
    /// </summary>
    private bool grounded;

    /// <summary>
    /// 定义一个类型为bool的变量walking，值为...
    /// 用来判断角色对象是否在移动，初始值为false
    /// </summary>
    private bool walking;

    /// <summary>
    /// 这是一个属性，属性和变量的区别在于有一个大括号{}
    /// 用来计算侧向位移值
    /// </summary>
    float SidestepAxisInput
    {
        get
        {
            //当鼠标右键被按下时
            if (Input.GetMouseButton(1))
            {
                float sidestep = Input.GetAxis("Sidestep");     //sidestep表示Sidestep输入轴的值，用于控制Player对象的侧向移动
                float horizontal = Input.GetAxis("Horizontal");     //horizontal表示水平输入大小
                return Mathf.Abs(sidestep) > Mathf.Abs(horizontal) ? sidestep : horizontal;     //表示SidestepAxisInput的值取sidestep和horizontal中的较大值
            }

            //当鼠标右键没有被按下时
            else
            {
                return Input.GetAxis("Sidestep");       //SidestepAxisInput的值取Sidestep轴的输入值
            }
        }
    }

    /// <summary>
    /// 这是一个属性，相当于一个变量，只不过有一些控制条件
    /// 该属性将用于下一节的动画状态控制代码中
    /// </summary>
    public bool Grounded
    {
        get
        {
            return grounded;
        }
    }


    /// <summary>
    /// 用来初始化各变量
    /// </summary>
    void Start()
    {
        //如果target为空
        if (target == null)
        {
            target = GetComponent<Rigidbody>();     //将当前游戏对象上的Rigidbody组件赋值给target
        }

        //顺序结构，判断，如果target为空
        if (target == null)
        {
            Debug.LogError("变量target未赋值，Player对象上未添加Rigidbody组件");      //输出错误信息
            enabled = false;      //使当前脚本禁用
            return;     //表示退出本函数
        }

        target.freezeRotation = true;       //表示游戏对象在受到外力时不会旋转

        walking = false;        //表示游戏对象不在移动
    }


    void Update()
    {
        float rotationAmount;       //表示绕目标对象上方（Y轴）的旋转角度

        //用来判断/计算分别用在鼠标和键盘操作下rotationAmount的值
        if (Input.GetMouseButton(1) && (!requireLock || controlLock || Cursor.lockState == CursorLockMode.Locked))
        {
            if (controlLock)
            {
                Cursor.lockState = CursorLockMode.Locked;       //Cursor.lockState表示在Game窗口中表示的锁定状态
            }

            rotationAmount = Input.GetAxis("Mouse X") * mouseTurnSpeed * Time.deltaTime;       //获得鼠标在水平方向上的位移
        }

        else
        {
            if (controlLock)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            rotationAmount = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;      //将水平轴的输入值赋值给rotationAmount
        }

        target.transform.RotateAround(target.transform.up, rotationAmount);     //设置目标对象的偏转/旋转

        //表示当输入轴ToggleWalk对象的按键被按下时，切换walking的值
        if (Input.GetButtonDown("ToggleWalk"))
        {
            walking = !walking;
        }

    }

    /// <summary>
    /// 对一些物理特效进行处理
    /// </summary>
    void FixedUpdate()
    {
        grounded = Physics.Raycast(target.transform.position + target.transform.up * -groundedCheckOffset, target.transform.up * -1,
            groundedDistance, groundLayers);    //判断target对象是否着地，将值赋值给grounded


        //如果grounded为true，则执行下面程序
        if (grounded)
        {
            target.drag = groundDrag;       //将groundDrag设置为刚体运动时的阻力系数

            //如果按下了Jump键
            if (Input.GetButton("Jump"))
            {
                target.AddForce(jumpSpeed * target.transform.up + target.velocity.normalized * directionalJumpFactor,
                    ForceMode.VelocityChange/*表示为刚体添加一个瞬时速度，并且忽略刚体的质量*/);        //给target对象事假一个瞬时速度

                //如果onJump已赋值，则调用上面绑定的回调函数delegate
                if (onJump != null)
                {
                    onJump();
                }
            }

            //当target对象着地时，且Jump按键未被按下时，执行下面程序语句
            else
            {
                Vector3 movement = Input.GetAxis("Vertical") * target.transform.forward + SidestepAxisInput
                    * target.transform.right;   //定义一个Vector3类型的变量movement，并将Vertical的输入值赋值

                float appliedSpeed = walking ? speed / walkSpeedDownscale : speed;      //取其中的一个值赋值给appliedSpeed

                //获得Vertical轴上的输入值
                if (Input.GetAxis("Vertical") < 0.00f)
                {
                    appliedSpeed /= walkSpeedDownscale;
                }

                //如果movement的长度大于inputThreshold
                if (movement.magnitude > inputThreshold)
                {
                    target.AddForce(movement.normalized * appliedSpeed, ForceMode.VelocityChange);      //添加一个即时速度
                }

                //如果movement的长度小于inputThreshold
                else
                {
                    target.velocity = new Vector3(0.0f, target.velocity.y, 0.0f);       //设置target对象的速度为...，表示仅在Y轴方向运动
                    return;
                }
            }
        }

        //表示target对象未着地，将Rigidbody的阻力系数设置为0
        else
        {
            target.drag = 0.0f;
        }
    }

    /// <summary>
    /// 用于在Unity编辑器的Scene窗口中绘制小部件（Gizmos）
    /// 注意：在OnDrawGizmos方法中绘制的图形只出现在Scene窗口中，不会出现在Game窗口中
    /// </summary>
    void OnDrawGizmos()
    {
        //如果showGizmos为false或target未赋值，则退出函数
        if (!showGizmos || target == null)
        {
            return;     //表示退出本函数
        }

        Gizmos.color = grounded ? Color.blue : Color.red;       //当target对象着地时Gizmos的颜色为蓝色，否则为红色

        Vector3 p = target.transform.position;
        Vector3 u = target.transform.up;
        Vector3 a = p + u * -groundedCheckOffset;

        Gizmos.DrawLine(a, a + u * -groundedDistance);      //绘制一条线
    }
}
