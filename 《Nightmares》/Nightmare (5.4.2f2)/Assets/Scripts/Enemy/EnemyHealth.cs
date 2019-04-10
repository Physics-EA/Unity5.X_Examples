using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    /// <summary>
    /// 定义一个类型为int的变量startingHealth，其值为...
    /// 用来设定敌人开始时的生命值
    /// </summary>
    [HideInInspector]
    public int startingHealth = 100;

    /// <summary>
    /// 定义一个类型为int的变量currentHealth，其值为...
    /// 用来表示敌人当前的生命值
    /// </summary>
    [HideInInspector]
    public int currentHealth;

    /// <summary>
    /// 定义一个类型为float的变量sinkSpeed，其值为...
    /// 用于控制敌人下沉的速度
    /// </summary>
    [HideInInspector]
    public float sinkSpeed = 2.5f;

    /// <summary>
    /// 定义一个类型为int的变量scoreValue，其值为...
    /// 用来表示敌人所值得分数
    /// </summary>
    [HideInInspector]
    public int scoreValue = 10;

    /// <summary>
    /// 声明一个类型为Audioclip的变量deathClip
    /// 用来存放敌人死亡时播放的声音片段AudioClip
    /// </summary>
    public AudioClip deathClip;

    /// <summary>
    /// 声明一个类型为Animator的变量anim
    /// 用来存放当前敌人游戏对象上的Animator组件
    /// </summary>
    Animator anim;

    /// <summary>
    /// 声明一个类型为AudioSource的变量enemyAudio
    /// 用来存放AudioSource组件
    /// </summary>
    AudioSource enemyAudio;

    /// <summary>
    /// 声明一个类型为ParticleSystem的变量hitParticles
    /// 用来存放ParticleSystem组件
    /// </summary>
    ParticleSystem hitParticles;

    /// <summary>
    /// 声明一个类型为CapsuleCollider的变量capsuleCollider
    /// </summary>
    CapsuleCollider capsuleCollider;

    /// <summary>
    /// 声明一个类型为bool的变量isDead
    /// 用于判断敌人对象是否已死亡
    /// </summary>
    bool isDead;

    /// <summary>
    /// 声明一个类型为bool的变量isSinking
    /// 用于控制敌人对象下沉的功能
    /// </summary>
    bool isSinking;


    void Awake()
    {
        anim = GetComponent<Animator>();    //获得当前游戏对象上的Animator组件
        enemyAudio = GetComponent<AudioSource>();   //获得当前游戏对象上的AudioSource组件
        hitParticles = GetComponentInChildren<ParticleSystem>();    //获得当前游戏对象上所有子对象上的ParticleSys组件
        capsuleCollider = GetComponent<CapsuleCollider>();     //获得当前游戏对象上的CapsuleCollider碰撞组件
        currentHealth = startingHealth;       //初始化当前敌人生命值为startingHealth
    }


    void Update()
    {
        if (isSinking)      //允许敌人下沉
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);      //敌人的下沉的移动
        }
    }


    /// <summary>
    /// 用于表示玩家游戏对象开始攻击
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="hitPoint">粒子坐标</param>
    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead)
        {
            return;     //如果敌人已死亡，则返回
        }

        enemyAudio.Play();      //播放敌人游戏对象受到攻击时的音频文件

        currentHealth -= amount;    //减少敌人的生命值

        hitParticles.transform.position = hitPoint;     //设置播放粒子效果的位置，为碰撞的坐标
        hitParticles.Play();    //当敌人受到攻击时播放hitParticles粒子效果

        if (currentHealth <= 0)
        {
            Death();        //如果敌人的生命值小于零，则调用Death方法
        }
    }


    /// <summary>
    /// 用来控制敌人游戏对象死亡时的一些游戏状态
    /// </summary>
    void Death()
    {
        isDead = true;  //将判断敌人是否死亡的状态表示设置为true

        capsuleCollider.isTrigger = true;

        anim.SetTrigger("Dead");        //播放敌人游戏对象死亡时的动画片段

        enemyAudio.clip = deathClip;       //将Audio Source组件上的Audio Clip音频文件替换成死亡时的音频文件

        enemyAudio.Play();       //播放敌人死亡的声音
    }


    /// <summary>
    /// 销毁敌人对象的函数
    /// </summary>
    public void StartSinking()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;       //消除当前游戏对象上的NavMeshAgent组件（禁用NavMeshAgent组件）
        GetComponent<Rigidbody>().isKinematic = true;       //允许使用刚体特性
        isSinking = true;   //允许敌人下沉
        ScoreManager.score += scoreValue;       //更新分值，表示当敌人游戏对象被击中后，ScoreManager的静态成员变量score将被更新（加上scoreValue）
        Destroy(gameObject, 2f);       //2秒后销毁敌人游戏对象
    }
}
