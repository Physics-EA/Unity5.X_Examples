using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    /// <summary>
    /// 声明一个类型为Rigidbody2D的变量rocket
    /// 用来存放炮弹对象
    /// </summary>
    public Rigidbody2D rocket;


    /// <summary>
    /// 定义一个类型为float的变量Speed，值为...
    /// 表示炮弹发射后飞行的速度
    /// </summary>
    [HideInInspector]
    public float speed = 20f;

    /// <summary>
    /// 声明一个类型为Player的变量playerCtrl
    /// 表示主角上绑定的PlayerController脚本组件
    /// </summary>
    private Player playerCtrl;


    /// <summary>
    /// 声明一个类型为Animator的变量anim
    /// 表示主角上绑定的Animator组件
    /// </summary>
    private Animator anim;


    // Use this for initialization
    void Start()
    {
        anim = transform.root.gameObject.GetComponent<Animator>();
        playerCtrl = transform.root.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Shoot");
            if (playerCtrl.facingRight)
            {
                Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                bulletInstance.velocity = new Vector2(speed, 0);
            }
            else
            {
                Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
                bulletInstance.velocity = new Vector2(-speed, 0);
            }
        }
    }
}
