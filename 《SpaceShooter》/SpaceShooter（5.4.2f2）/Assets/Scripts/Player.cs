


//说明：
//1、通过 Input类 实现了飞船的移动；
//2、通过 自定义类Boundary 实现了飞船的边界控制；
//3、通过 tilt 实现了飞船的倾斜角度；
//4、通过 Inout类+Instantiate 实现了发射子弹；
//5、通过逻辑关系控制发射时间间隔；



using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 将飞船活动的边界用一个类Boundary封装起来，其实是没有必要，但封装后更好看
/// 用于管理飞船活动的边界
/// </summary>
[System.Serializable]
public class Area
{
    public float xMin = -6.0f, xMax = 6.0f, zMin = -4.0f, zMax = 14.0f;
}

public class Player : MonoBehaviour
{

    /// <summary>
    /// 定义一个类型为float的变量title，值为...
    /// 用作飞机的飞行时的倾斜角度
    /// </summary>
    public float tilt = 6.0f;

    /// <summary>
    /// 定义一个类型为float的变量speed，值为...
    /// 用作控制飞机飞行速度
    /// </summary>
    public float speed = 5.0f;

    /// <summary>
    /// 声明一个类型为Boundary的变量boundary
    /// 用于控制飞机的飞行边界
    /// </summary>
    public Area boundary;

    /// <summary>
    /// 定义一个类型为float的变量fireRate，值为...
    /// 用于发射时间间隔
    /// </summary>
    public float fireRate = 0.1f;

    /// <summary>
    /// 声明一个类型为GameObject的变量shot
    /// 用于装载Bolt预设体
    /// </summary>
    public GameObject bolt;

    /// <summary>
    /// 声明一个类型为Transform的变量shotSpawn
    /// 用于设置子弹的位置
    /// </summary>
    public Transform shotSpawn;

    /// <summary>
    /// 定义一个类型为float的变量nextFire，值为...
    /// 用于记录下一次发射的时间
    /// </summary>
    private float nextFire = 0.0f;


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {

            nextFire = Time.time + fireRate;


            //Instantiate（克隆、复制）：复制对象、位置、角度
            Instantiate(bolt, shotSpawn.position, shotSpawn.rotation);


            //GetComponent是Component类中的一个方法，它是一个方法，不是类
            //至于为什么可以直接使用，不用管
            //得到AudioSource这个类，并使用Play()这个方法
            //Play()这个方法的作用是播放声音
            GetComponent<AudioSource>().Play();
        }


        else if (Input.GetButton("Fire1") && Time.time > nextFire)
        {

            nextFire = Time.time + fireRate;


            //Instantiate（克隆、复制）：复制对象、位置、角度
            Instantiate(bolt, shotSpawn.position, shotSpawn.rotation);


            //GetComponent是Component类中的一个方法，它是一个方法，不是类
            //至于为什么可以直接使用，不用管
            //得到AudioSource这个类，并使用Play()这个方法
            //Play()这个方法的作用是播放声音
            GetComponent<AudioSource>().Play();
        }
    }

    void FixedUpdate()
    {
        //得到水平方向输入
        //定义一个类型为float的变量moveHorizontal，并使用 类型+方法 返回的一个值来赋值
        float moveHorizontal = Input.GetAxis("Horizontal");

        //得到垂直方向输入
        //定义一个类型为float的变量moveVertical，并使用 类型+方法 返回的一个值来赋值
        float moveVertical = Input.GetAxis("Vertical");

        //定义一个Vector3的变量movement，值为...；
        //作为刚体速度
        //变量movement的类型为Vector3，则被赋予的值的类型也应该为Vector3，故此处重载了一个类型为Vector3的构造器
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //定义一个Rigidbody的变量rb，并赋值为...
        //定义的变量rb的类型为Rigidbody，故被赋予的值得类型也应该为Rigidbody，所以这里用使用了GetComponent这个方法来获得类型为Rigidbody的值
        //作用是用来承载飞船这个游戏对象
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            //此处rb相当于Rigidbody(物理)类型的一个实例化，故使用的velocity则是Rigidbody类模板中的一个通/共用的方法
            //velocity是一个类型为Vector3的字段，封装了物体的一些速度特性
            //作用是设置rb这个游戏对象的物理速度
            rb.velocity = movement * speed;

            //类型为Rigidbody的游戏对象rb的位置为...
            //位置为Vector3类型，所以要赋予的值也应当为Vector3类型
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 0.0f, Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));
        }

        //类型为Rigidbody的游戏对象rb的角度为...
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);

    }




}
