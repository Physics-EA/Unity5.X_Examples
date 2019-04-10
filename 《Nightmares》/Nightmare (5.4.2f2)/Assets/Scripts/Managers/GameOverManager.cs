using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    /// <summary>
    /// 声明一个类型为PlayerHealth的变量playerHealth
    /// 用来存放实例化的PlayerHealth脚本对象
    /// </summary>
    public PlayerHealth playerHealth;


    /// <summary>
    /// 定义一个类型float的变量restartDelay
    /// 用来控制游戏重新开始的等待时间
    /// </summary>
    public float restartDelay = 5f;

    /// <summary>
    /// 声明一个类型为Animator的变量anim
    /// 用来存放当前对象上的Animator组件
    /// </summary>
    Animator anim;

    /// <summary>
    /// 声明一个类型为float的变量restartTimer
    /// 用来计时重新开始的时间
    /// </summary>
    float restartTimer;


    void Awake()
    {
        anim = GetComponent<Animator>();       //获得当前游戏对象上的Animator组件
    }


    void Update()
    {
        //如果玩家的生命值小于零
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");    //播放游戏结束的动画

            restartTimer += Time.deltaTime;     //开始计时

            if (restartTimer >= restartDelay)       //如果计时器的时间大于等待的时间
            {
                Application.LoadLevel(Application.loadedLevel);     //重新加载场景
            }
        }
    }
}
