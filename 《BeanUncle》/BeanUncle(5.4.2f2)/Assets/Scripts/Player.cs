using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 定义一个类型为float的变量moveForce，值为...
    /// 用作力的大小
    /// </summary>
    [HideInInspector]
    public float moveForce = 365f;

    /// <summary>
    /// 定义一个类型为float的变量maxSpeed，值为...
    /// 用来表示主角运动的最大速度
    /// </summary>
    [HideInInspector]
    public float maxSpeed = 5f;

    /// <summary>
    /// 用来表示主角跳跃时力的大小
    /// </summary>
    [HideInInspector]
    public float jumpForce = 1000f;

    /// <summary>
    /// 用来检测角色是否在地面上的对象
    /// </summary>
    private Transform groundCheck;

    /// <summary>
    /// 定义一个类型为bool的变量grounded，值为...
    /// 判断角色是否在地面上
    /// </summary>
    private bool grounded = false;

    /// <summary>
    /// 角色对象上的Animator组件
    /// </summary>
    private Animator anim;

    /// <summary>
    /// 主角是否朝向右侧
    /// </summary>
    [HideInInspector]
    public bool facingRight = true;

    /// <summary>
    /// 主角是否在跳跃
    /// </summary>
    [HideInInspector]
    public bool jump = false;


    // Use this for initialization
    void Start()
    {
        groundCheck = transform.Find("groundCheck");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(h));
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (h * rb.velocity.x < maxSpeed)
        {
            rb.AddForce(Vector2.right * h * moveForce);
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        if (h > 0 && !facingRight)
        {
            Flip();
        }
        else if (h < 0 && facingRight)
        {
            Flip();
        }

        if (jump)
        {
            anim.SetTrigger("Jump");
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }


    /// <summary>
    /// 用来实现对主角的反转操作
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
