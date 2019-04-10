using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 改脚本用于更新角色生命值的HealthSlider控件
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// 定义一个类型为int的变量startingHealth，值为...
    /// 用于表示玩家的总生命值；
    /// </summary>
    [HideInInspector]
    public int startingHealth = 100;

    /// <summary>
    /// 声明一个类型为int的变量currentHealth
    /// 玩家的当前生命值
    /// </summary>
    [HideInInspector]
    public int currentHealth;

    /// <summary>
    /// 声明一个类型为Slider的变量healthSlider
    /// 用来存储Slider组件实例对象
    /// </summary>
    [HideInInspector]
    public Slider healthSlider;

    /// <summary>
    /// 声明一个类型为Image的变量damageImage
    /// 用来存储DamageImageUI对象上的Image组件
    /// 角色被攻击，或没被攻击时的幕布颜色显示
    /// </summary>
    public Image damageImage;

    /// <summary>
    /// 声明一个类型为AudioClip的变量deathClip
    /// 死亡声音控件
    /// 这只是声音的一个Adudio Clip片段，并不能播放，需要放在Audio Source组件下才能播放
    /// </summary>
    public AudioClip deathClip;

    /// <summary>
    /// 定义一个类型为float的变量flashSpeed，值为...
    /// 用于Lerp插值之间的速度
    /// </summary>
    [HideInInspector]
    public float flashSpeed = 5f;

    /// <summary>
    /// 定义一个Color类型的变量flashColor，值为...
    /// 红色，最后一个值为Alpha
    /// 用于更改某个游戏对象或组件的颜色值
    /// </summary>
    public Color flashColour = new Color(1f, 0f, 0f, 1.0f);

    /// <summary>
    /// 声明一个类型为Animator的变量anim
    /// </summary>
    Animator anim;

    /// <summary>
    /// 声明一个类型为AudioSource的变量playerAudio
    /// 用来控制播放玩家的声音
    /// </summary>
    AudioSource playerAudio;

    /// <summary>
    /// 声明一个类型为PlayerMovement的变量playerMovement
    /// </summary>
    PlayerMovement playerMovement;

    /// <summary>
    /// 状态标志，用于判断角色死亡与否的状态标志（不一定角色死亡就是false，活着就是true，只是用来控制角色死亡或者活着的一些状态）
    /// 如果没有赋值，则初始状态为false
    /// </summary>
    bool isDead;

    /// <summary>
    /// 用于判断角色是否受到攻击
    /// 初始值为false
    /// </summary>
    bool damaged;


    public GameObject gunBarrelEnd;


    void Awake()
    {
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();      //给healthSlider引用变量赋值，找到HealthSlider游戏对象，获得上面的Slider组件

        anim = GetComponent<Animator>();        //得到玩家上面的Animator组件

        playerAudio = GetComponent<AudioSource>();      //初始化声音变量，得到角色身上的AudioSource组件

        playerMovement = GetComponent<PlayerMovement>();    //得到玩家上面的PlayerMovement脚本组件

        currentHealth = startingHealth;      //给当前角色的生命值赋值为初始值

    }

    /// <summary>
    /// 每一帧都会执行，每帧执行的时间...
    /// 这里主要用于更新DamageImage的Color属性
    /// </summary>
    void Update()
    {
        if (damaged)       //当damaged为false的时候不执行括号内的代码，而执行else中的代码
                           //当damaged为true的时候执行括号内的代码
        {
            damageImage.color = flashColour;    //表示角色正在受到攻击，则将幕布的颜色设置为之前设置好的颜色值
        }

        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);    //此时角色没有受到攻击，将幕布颜色从红色变为无色，时间为t，
                                                                                                            //此后幕布颜色一直为无色
        }

        damaged = false;
    }

    /// <summary>
    /// 用于当角色受到攻击时更新各变量的值
    /// 开始攻击
    /// </summary>
    /// <param name="amount">受到的攻击点数</param>
    public void TakeDamage(int amount)   //public表明可以在其他脚本中调用该函数，而事实是本脚本并没有调用该方法，而是在EnemyAttack函数中调用了该方法
                                         //表示在EnemyAttack函数中判断了角色是否受到攻击
    {
        damaged = true;     //表示在EnemyAttack函数中判断出角色受到了攻击，于是将damaged设置为true

        currentHealth -= amount;      //表示角色受到攻击后生命值减少了amoun，从而获得当前生命值

        healthSlider.value = currentHealth;      //生命值（血条）的值由currentHealth来控制

        playerAudio.Play();      //播放玩家当前状态下的声音，受到攻击时的声音

        if (currentHealth <= 0 && !isDead)  //满足这个条件则执行下面的代码，而不是说isDead为false或者true就代表角色死亡或者或者
        {
            Death();        //调用Death方法

            Destroy(gunBarrelEnd);       //销毁gunBarrelEnd游戏对象，目的是当角色死亡后，不能再进行开枪射击
        }
    }


    /// <summary>
    /// 用于当角色死亡时更新各变量的值
    /// </summary>
    void Death()
    {
        isDead = true;      //给isDead变量赋值为true，以便于在其他判断isDead为false的地方不能不能再进行执行接下来的代码

        anim.SetTrigger("Die");     //SetTrigger表示激活Trigger参数Die，触发后角色会进入Dealth状态

        playerAudio.clip = deathClip;   //将原先的声音元Player Hurt更改为deathClip

        playerAudio.Play();     //播放声音

        playerMovement.enabled = false;     //取消角色下面的PlayerMovement脚本组件

        ETCInput.SetControlActivated("LeftJoystick", false);
        ETCInput.SetControlActivated("RightJoystick", false);

    }
}
