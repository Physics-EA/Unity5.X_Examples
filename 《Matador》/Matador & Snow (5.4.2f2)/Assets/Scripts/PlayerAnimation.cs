using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    /// <summary>
    /// 声明一个枚举类型的变量CharacterState
    /// 用来表示角色的状态
    /// </summary>
    enum CharacterState
    {
        Normal,     //表示角色正常行走的状态
        Jumping,      //表示角色跳跃的状态
        Falling,        //表示角色从空中掉落下来的状态
        Landing         //表示着地时的状态
    }

    /// <summary>
    /// 声明一个类型为Animation的变量target
    /// 用来存放目标对象上的Animation组件
    /// </summary>
    public Animation target;

    /// <summary>
    /// 声明一个类型为Rigidbody的变量rigidbody
    /// 用来存放目标对象上的Rigidbody组件
    /// </summary>
    public Rigidbody rigidbody;


    public Transform root, spine, hub;

    /// <summary>
    /// 定义一个类型为float的变量walkSpeed，初始值为0.2
    /// 表示目标对象在走路时的速度
    /// </summary>
    [HideInInspector]
    public float walkSpeed = 0.2f;

    /// <summary>
    /// 定义一个类型为float的变量runSpeed，初始值为1
    /// 表示目标对象在跑步时的速度
    /// </summary>
    [HideInInspector]
    public float runSpeed = 1.0f;

    /// <summary>
    /// 定义一个类型为float的变量rotationSpeed，初始值为6
    /// 表示目标对象在旋转时的速度
    /// </summary>
    [HideInInspector]
    public float rotationSpeed = 6.0f;

    /// <summary>
    /// 定义一个类型为float的变量shuffleSpeed，初始值为7
    /// 表示目标对象在拖着脚时的速度
    /// </summary>
    [HideInInspector]
    public float shuffleSpeed = 7.0f;


    public float runningLandingFactor = 0.2f;

    /// <summary>
    /// 声明一个类型为PlayerController的变量controller
    /// 用来存放PlayerController脚本组件
    /// </summary>
    private PlayerController controller;

    /// <summary>
    /// 定义一个枚举类型CharacterState变量State，初始值为CharacterState.Falling
    /// 表示Player对象在降落中
    /// </summary>
    private CharacterState state = CharacterState.Falling;

    /// <summary>
    /// 定义一个类型为bool的变量canLand，初始值为true
    /// 用来表示是否可以着陆
    /// </summary>
    private bool canLand = true;


    private float currentRotation;

    private Vector3 lastRootForward;

    /// <summary>
    /// 表示Player对象在XZ平面的运动速度，这是一个属性
    /// </summary>
    private Vector3 HorizontalMovement
    {
        get
        {
            return new Vector3(rigidbody.velocity.x, 0.0f, rigidbody.velocity.z);       //要得到Player对象在XZ平面的速度，只需要将Player对象在Y轴上的速度设置为0
        }
    }



    /// <summary>
    /// 初始化个变量
    /// </summary>
    void Start()
    {
        //为什么这里controller不赋值就会报错，而例子不会  //因为在VerifySetup中返回了false，所以没有赋值   
        controller = GetComponent<PlayerController>();

        //如果target为空
        if (target == null)
        {
            target = GetComponent<Animation>();     //获得目标对象上的Animation组件
        }

        //如果rigidbody为空
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();      //获得目标对象上的Rigidbody组件
        }


        //如果各变量均已赋值
        if (VerifySetups())
        {
            controller.onJump += OnJump;    //将函数OnJump绑定到PlayerController脚本的委托onJump上
            currentRotation = 0.0f;
            lastRootForward = root.forward;
        }

    }



    void FixedUpdate()
    {
        //当Player对象着地后
        if (controller.Grounded)
        {
            //满足以下条件，则调用OnLand（）函数
            if (state == CharacterState.Falling || (state == CharacterState.Jumping && canLand))
            {
                OnLand();
            }

        }

        //否则，如果枚举变量state状态为Jumping，则将canLand设置为true
        else if (state == CharacterState.Jumping)
        {
            canLand = true;
        }



    }

    private void Update()
    {
        //根据Player对象不同的状态实现不同的动作
        switch (state)
        {
            //正常行走状态
            case CharacterState.Normal:

                Vector3 movement = HorizontalMovement;      //定义一个类型为Vector3的变量movement，值为HorizontalMovement

                //当Player对象的运动速度小于walkSpeed时
                if (movement.magnitude < walkSpeed)
                {
                    //如果lastRootForward和root.forward的夹角大于1度，则动画片段渐变为Shuffle
                    if (Vector3.Angle(lastRootForward, root.forward) > 1.0f)
                    {
                        target.CrossFade("Shuffle");    //动画片段渐变为Shuffle

                        lastRootForward = Vector3.Slerp(lastRootForward, root.forward, Time.deltaTime * shuffleSpeed);
                    }

                    //否则，动画片段渐变到Idle
                    else
                    {
                        target.CrossFade("Idle");       //动画片段渐变为Idle
                    }

                }

                //当Player对象的运动速度大于walkSpeed时
                else
                {
                    //如果lastRootForward和root.forward的夹角大于91度，则表示Player要朝反方向运动，将动画片段Walk和Run的速度乘以-1
                    target["Walk"].speed = target["Run"].speed = Vector3.Angle(root.forward, movement) > 91.0f ? -1.0f : 1.0f;

                    //如果Player对象的运动速度小于runSpeed，则播放动画片段Walk
                    if (movement.magnitude < runSpeed)
                    {
                        target.CrossFade("Walk");       //动画片段渐变为Walk
                    }
                    //如果Player对象的运动速度大于runSpeed，则播放动画片段Run
                    else
                    {
                        target.CrossFade("Run");        //动画片段渐变为Run
                    }

                    lastRootForward = root.forward;
                }
                break;

            //跳跃状态
            case CharacterState.Jumping:
                target.CrossFade("Jump");
                break;

            //下降状态
            case CharacterState.Falling:
                target.CrossFade("Fall");
                break;

            //正在着地状态
            case CharacterState.Landing:
                target.CrossFade("Land");
                break;
        }
    }



    void LateUpdate()
    {
        float targetAngle = 0.0f;       //定义一个类型为float的变量targetAngle，初始值为0度，表示目标旋转角度

        Vector3 movement = HorizontalMovement;      //定义一个类型为Vector3的变量movement，赋值为HorizontalMovement

        //如果目标对象在XZ平面上的速度值大于walkSpeed时
        if (movement.magnitude >= walkSpeed)
        {
            targetAngle = Vector3.Angle(movement, new Vector3(root.forward.x, 0.0f, root.forward.z));   //计算movement与root.forward在XZ平面上的夹角，并赋值给targetAngle

            //如果夹角targetAngle大于movement与root.right * -1的夹角
            if (Vector3.Angle(movement, root.right) > Vector3.Angle(movement, root.right * -1))
            {
                targetAngle *= -1.0f;    //targetAngle乘以-1
            }
            //当targetAngle的绝对值大于91时
            if (Mathf.Abs(targetAngle) > 91.0f)
            {
                targetAngle = targetAngle + (targetAngle > 0 ? -180.0f : 180.0f);   //将比较的值赋给targetAngle
            }
        }

        currentRotation = Mathf.Lerp(currentRotation, targetAngle, Time.deltaTime * rotationSpeed);
        hub.RotateAround(hub.position, root.up, currentRotation);
        spine.RotateAround(spine.position, root.up, currentRotation * -1.0f);
    }




    /// <summary>
    /// 用来检测变量是否可以赋值
    /// 在VerifySetups（）函数中使用
    /// </summary>
    /// <param name="component">表示脚本中的变量</param>
    /// <param name="name">表示该变量的名称</param>
    /// <returns>bool</returns>
    bool VerifySetup(Component component, string name)
    {
        //如果变量未赋值
        if (component == null)
        {
            Debug.LogError("参数" + name + "未赋值.");   //显示错误
            enabled = false;       //禁用该脚本
            return false;       //返回false
        }
        return true;
    }



    /// <summary>
    /// 实现对变量是否可以赋值的检测
    /// </summary>
    /// <returns>bool</returns>
    bool VerifySetups()
    {
        return VerifySetup(target, "target")
            && VerifySetup(rigidbody, "rigidbody")
            && VerifySetup(root, "root")
            && VerifySetup(spine, "spine")
            && VerifySetup(hub, "hub");
    }



    /// <summary>
    /// 用来响应Player跳跃事件
    /// </summary>
    void OnJump()
    {
        canLand = false;      //Player对象跳跃起来后，在上升的过程中不可着地，此时设置为false
        state = CharacterState.Jumping;     //表示Player对象在跳跃中
        Invoke("fall", target["Jump"].length);      //在Jump动画片段播放完毕后调用函数Fall
    }




    /// <summary>
    /// 用来表示Player对象下落的函数Fall
    /// </summary>
    void Fall()
    {
        //如果目标对象已经着地，则返回
        if (controller.Grounded)
        {
            return;
        }

        state = CharacterState.Falling;     //否则将枚举类型的变量state设置为Falling，表示Player对象还在下落的过程中，尚未着地
    }




    /// <summary>
    /// 表示Player对象在着地的过程中
    /// </summary>
    void OnLand()
    {
        canLand = false;
        state = CharacterState.Landing;       //将枚举变量state设置为Landing
        Invoke("Land", target["Land"].length * (HorizontalMovement.magnitude/*表示Player对象水平速度矢量的长度*/ < walkSpeed ? 1.0f : runningLandingFactor));       //表示在一段时间后调用函数Land完成着地动作
    }




    /// <summary>
    /// 表示Player对象已经着地
    /// </summary>
    void Land()
    {
        //表示只有在枚举变量为Landing的时候才进行下面的程序语句，否则什么也不做返回
        if (state != CharacterState.Landing)
        {
            return;
        }

        state = CharacterState.Normal;         //将枚举变量state设置为Normal
    }

}
