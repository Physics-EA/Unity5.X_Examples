using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    /// <summary>
    /// 定义一个类型位float变量timeBetweenAttacks，值为...
    /// 敌人能攻击角色的时间间隔
    /// </summary>
    [HideInInspector]
    public float timeBetweenAttacks = 0.5f;

    /// <summary>
    /// 定义一个类型为int的变量attackDamage，值为...
    /// 敌人攻击角色的伤害数值
    /// </summary>
    [HideInInspector]
    public int attackDamage = 10;

    /// <summary>
    /// 声明一个类型为Animator的变量anim
    /// </summary>
    Animator anim;

    /// <summary>
    /// 声明一个类型为Gameob的变量player
    /// </summary>
    GameObject player;

    /// <summary>
    /// 声明一个类型为PlayerHealth的变量playerHealth
    /// 用来存放玩家对象上面PlayerHealth脚本的实例对象
    /// </summary>
    PlayerHealth playerHealth;

    /// <summary>
    /// 声明一个类型为EnemyHealth的变量enemyHealth
    /// </summary>
    EnemyHealth enemyHealth;

    /// <summary>
    /// 声明一个类型为bool的变量playerInRange
    /// 状态标志，用于判断玩家是否进入攻击范围
    /// 初始值为false
    /// </summary>
    bool playerInRange;

    /// <summary>
    /// 计时器
    /// 用于控制敌人进行攻击的时间间隔
    /// </summary>
    float timer;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");    //获得玩家游戏对象
        playerHealth = player.GetComponent<PlayerHealth>();     //获得当前玩家游戏对象上的PlayerHealth脚本组件
        enemyHealth = GetComponent<EnemyHealth>();              //获得当前敌人游戏对象上的EnemyHealth脚本组件
        anim = GetComponent<Animator>();                        //获得当前敌人游戏对象上的Animator组件
    }


    /// <summary>
    /// 用于判断玩家已经进入攻击范围，触发攻击条件
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)      //判断进入触发器的碰撞游戏对象是否为player，因为有可能是其他enemy
        {
            playerInRange = true;
        }
    }



    /// <summary>
    /// 用于判断玩家已经离开攻击范围，触发非攻击条件
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)          //判断离开触发器的碰撞游戏对象是否为player，因为有可能是其他enemy
        {
            playerInRange = false;
        }
    }


    /// <summary>
    /// 每帧都执行，用于更新敌人攻击玩家后的各状态变量
    /// 用于控制敌人如何攻击角色
    /// </summary>
    void Update()
    {
        timer += Time.deltaTime;       //计时器，累计距离上一次攻击玩家后的总时间

        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)     //满足三个条件，1间隔时间大于一定值，2玩家在敌人的攻击范围内，3敌人的当前生命值大于0
        {
            Attack();   //调用Attack函数
                        //敌人对象什么都不用管，只要满足上面三个条件就启动开始攻击函数，有且只有一个TakeDamage函数
        }

        if (playerHealth.currentHealth <= 0)        //如果玩家的当前生命值小于0，则播放玩家死亡动画
        {
            anim.SetTrigger("PlayerDead");      //设置Animator State Machines下的Trigger参数，使之满足播放死亡动画的条件
        }
    }


    /// <summary>
    /// 用于实现敌人对角色的攻击
    /// </summary>
    void Attack()
    {
        timer = 0f;     //每次敌人攻击玩之后计时器进行重新计时（即当敌人攻击一下后，timer立马变为0）

        if (playerHealth.currentHealth > 0)        //如果palyerHealth脚本中的currentHeather值大于0（即玩家的生命值大于0），则执行以下代码
        {
            playerHealth.TakeDamage(attackDamage);         //调用palyerHealth脚本中的TakeDamage方法，更新玩家当前状态下个变量的值
        }
    }
}
