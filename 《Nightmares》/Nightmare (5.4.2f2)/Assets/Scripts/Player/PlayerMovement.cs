
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{

    /// <summary>
    /// 定义一个类型为float的变量speed，值为...
    /// 用来表示角色的运动速度
    /// </summary>
    [HideInInspector]
    public float speed = 6f;

    /// <summary>
    /// 声明一个类型为Vector3的变量movement
    /// 用来表示两帧间角色移动的距离
    /// </summary>
    Vector3 movement;

    /// <summary>
    /// 声明一个类型为Animator的变量anim
    /// 用来存放当前角色对象上的Animator组件实例（对象）
    /// </summary>
    Animator anim;

    /// <summary>
    /// 声明一个类型为Rigidbody的变量rb
    /// 用来存放角色对象上的Rigidbody组件实例（对象）
    /// </summary>
    Rigidbody rb;

    /// <summary>
    /// 声明一个类型为int的变量floorMask
    /// 表示Floor层对应的掩码，用于碰撞检测时指定检测层
    /// </summary>
    int floorMask;

    /// <summary>
    /// 定义一个类型为float的变量camRayLength，值为...
    /// 用于指定碰撞检测时射线的长度
    /// </summary>
    float camRayLength = 100f;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");     //得到层名称为“Floor“的掩码值
        anim = GetComponent<Animator>();    //得到当前游戏对象上的Animator组件实例
        rb = GetComponent<Rigidbody>();     //得到当前游戏对象上的Rigidbody组件实例
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");       //获得水平方向的输入
        float v = Input.GetAxisRaw("Vertical");         //获得垂直方向的输入

        Move(h, v);
        //Turning();
        Animating(h, v);
    }

    /// <summary>
    /// 用来控制角色的移动
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);     // 更新Vector3实例的x，y和z三个分量数值
        movement = movement.normalized * speed * Time.deltaTime;    //归一化向量后，设置移动速度
        rb.MovePosition(transform.position + movement);     //将角色移动到目标位置
    }

    /// <summary>
    /// 用来实现鼠标在窗口中移动时角色始终朝向鼠标位置效果
    /// </summary>
    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            rb.MoveRotation(newRotation);
        }
    }


    /// <summary>
    /// 用于更新动画参数
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);  //设置Idle和Move状态之间的条件布尔值IsWalking
    }



    public void RunAnimatorOpen()
    {
        anim.SetBool("move", true);
    }


    public void RunAnimatorStop()
    {
        anim.SetBool("move", false);
    }


    public void MoveInRightJoyStick(Vector2 weizhi)
    {
        if (weizhi.y != 0 || weizhi.x != 0)
        {
            //设置角色的朝向(朝向当前坐标+摇杆偏移量)
            transform.LookAt(new Vector3(transform.position.x + weizhi.x, transform.position.y, transform.position.z + weizhi.y));
        }
    }

}

