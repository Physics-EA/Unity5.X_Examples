using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    /// <summary>
    /// 定义一个int类型的变量，赋值为...
    /// 用来设定主角的攻击力
    /// </summary>
    [HideInInspector]
    public int damagePerShot = 50;

    /// <summary>
    /// 定义一个类型为float的变量timeBetweenBullets，其值为0.15s
    /// 用来设置两次攻击的时间间隔
    /// </summary>
    [HideInInspector]
    public float timeBetweenBullets = 0.15f;

    /// <summary>
    /// 定义一个类型为float的变量range，其值为...
    /// 用来表示激光束的长度，射击范围
    /// </summary>
  //[HideInInspector]
    public float range = 100;


    /// <summary>
    /// 声明一个类型为float的变量timer
    /// 用来统计计算机当前的时间
    /// </summary>
    float timer;

    /// <summary>
    /// 声明一个类型为Ray的变量shootRay
    /// 表示射击时子弹飞行的射线轨迹
    /// </summary>
    Ray shootRay;

    /// <summary>
    /// 射线撞到的东西
    /// </summary>
    RaycastHit shootHit;

    /// <summary>
    /// 射击层
    /// </summary>
    int shootableMask;

    /// <summary>
    /// 声明一个类型为ParticleSystem的变量gunParticles
    /// 用来存放当前游戏对象上的Particle System组件实例对象
    /// </summary>
    ParticleSystem gunParticles;

    /// <summary>
    /// 声明一个类型为LineRenderer的变量gunLine
    /// 用来存储当前游戏对象上面的Lind Renderer组件的实例对象
    /// </summary>
    LineRenderer gunLine;

    /// <summary>
    /// 声明一个类型为AudioSource的变量gunAudio
    /// 用来存放当前游戏对象上的Audio Source组件
    /// </summary>
    AudioSource gunAudio;

    /// <summary>
    /// 声明一个类型为Light的变量gunLight
    /// 用来控制枪发射子弹是的光
    /// </summary>
    Light gunLight;

    /// <summary>
    /// 定义一个类型为float的变量effesDisplayTime，其值为0.2
    /// 用来计算子弹消失的用时时间系数
    /// 尝试下面两句话的区别
    /// </summary>
    float effectsDisplayTime = 0.2f;
    // float effectsDisplayTime = 5f;


    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");     //射击层为Shootable
        gunParticles = GetComponent<ParticleSystem>();      //获得当前游戏对象的粒子系统组件
        gunLine = GetComponent<LineRenderer>();             //获得当前游戏对象的LineRenderer组件的实例对象
        gunAudio = GetComponent<AudioSource>();             //获得当前游戏对象上的声音控制组件
        gunLight = GetComponent<Light>();                   //获得当前游戏对象上的灯光控制组件
    }


    /// <summary>
    /// 实现了用户输入的控制
    /// </summary>
    void Update()
    {
        timer += Time.deltaTime;    //开始计时

        //若玩家按下Fire1键，并且间隔时间大于timeBetweenBullets，则调用Shoot函数
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot();       //射击
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();       //清除射击效果
        }
    }


    /// <summary>
    /// 清除射击效果
    /// </summary>
    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    /// <summary>
    /// 用来实现射击动作，并将计时器清零，重新开始
    /// </summary>
    void Shoot()
    {
        timer = 0f;

        gunAudio.Play();    //播放声音

        gunLight.enabled = true;    //在场景中模拟枪射击时产生的星光，使组件激活
        gunLine.enabled = true;     //在场景中模拟子弹，使组件激活

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.SetPosition(0, transform.position);        //用来设定Line Renderer的起点（顶点序号为0）

        shootRay.origin = transform.position;       //子弹的飞行起始位置为当前游戏对象的位置，shootRay是一个射线
        shootRay.direction = transform.forward;     //子弹的飞行方向为当前游戏对象的前向，shootRay是一个射线

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))      //若shootRay与Shootable层上的对象发生了碰撞
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();    //获得发生碰撞对象上的EnemyHealth脚本组件

            if (enemyHealth != null)     //若该组件不为空，则表示角色发射的子弹击中了敌人对象
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);      //将敌人对象的生命值减少damagePerShot
            }
            gunLine.SetPosition(1, shootHit.point);     //用来设定LineRenderer组件的终点（顶点序号为1）
        }
        else       //如果shootRay与Shootable层上的对象未发生碰撞
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);   //在射击方向上显示一段长度为range的激光束
        }
    }
}
