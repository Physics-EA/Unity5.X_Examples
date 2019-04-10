

using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    /// <summary>
    /// 定义一个类型为float的变量tumble，值为...
    /// 用来控制小行星的旋转速度
    /// </summary>
    public float tumble = 10.0f;

    /// <summary>
    /// 定义一个类型为int的变量 scoreValue，值为...
    /// 用来计分
    /// </summary>
    private int scoreValue = 10;

    /// <summary>
    /// 声明一个类型为GameManager的变量 gameController
    /// 用来进行游戏管理
    /// </summary>
    private UIManager gameController;

    /// <summary>
    /// 声明一个类型为GameObject的变量 explosion
    /// 用来存放子弹击中小行星后的的粒子特效
    /// </summary>
    public GameObject explosion;

    /// <summary>
    /// 声明一个类型为GameObject的变量playerExplosion
    /// 飞船与小行星碰撞后的粒子对象
    /// </summary>
    public GameObject playerExplosion;

    /// <summary>
    /// 声明一个类型为float的变量speed
    /// 用作控制小行星的速度
    /// </summary>
    public float speed;



    void Start()
    {
        //得到游戏对象上的Rigidbody组件，并随机给它添加一个旋转角度
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;

        //子弹上有刚体组件Rigidbody，通过GetComponent方法获得组件Rigidbody(类)，设置刚体Bolt的速度vleocity
        GetComponent<Rigidbody>().velocity = -transform.forward * speed;

        //定义一个类型为GameObject的变量go，值为...
        //用来存储Tag为GameController类型的游戏对象实例
        GameObject go = GameObject.FindWithTag("GameController");

        //如果游戏对象不为空
        if (go != null)
        {
            //对声明类型为GameManager的变量gameController进行赋值，以便之后对此GameManager的实例化对象进行更改
            gameController = go.GetComponent<UIManager>();
        }

        else
        {
            Debug.Log("找不到tag为GameController的对象");
        }

        //这个if的作用是判断gameController(Gamemanager类)是否为空
        if (gameController == null)
        {
            Debug.Log("找不到脚本GameController.cs");
        }
    }

    void OnTriggerEnter(Collider other)
    {

        //第一种情况：碰撞的为Boundary
        //如果不加这个if，则边界就会被Destroy掉
        if (other.tag == "Boundary")
        {
            return;
        }


        //第二种情况：碰撞的为子弹
        //将碰撞的游戏对象Destroy掉
        Destroy(other.gameObject);
        //将自己（小行星）Destroy掉
        Destroy(gameObject);
        //实例化子弹击中小行星后的粒子特效（小行星的位置）
        Instantiate(explosion, transform.position, transform.rotation);


        //第三种情况：碰撞的游戏对象为Player
        if (other.tag == "Player")
        {
            //实例化飞船与小行星相撞后的粒子特效
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);

            //游戏结束
            gameController.GameOver();
        }

        gameController.AddScore(scoreValue);
    }


}
