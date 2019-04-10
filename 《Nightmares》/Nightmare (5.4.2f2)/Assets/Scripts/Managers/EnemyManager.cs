


using UnityEngine;



/// <summary>
/// 用来不断地动态的实例化新的敌人对象
/// </summary>
public class EnemyManager : MonoBehaviour
{

    /// <summary>
    /// 声明一个类型为PlayerHealth的变量playerHealth
    /// 用来获得该脚本挂着的游戏对象
    /// </summary>
    public PlayerHealth playerHealth;

    /// <summary>
    /// 声明一个类型为GameObject的变量enemy
    /// </summary>
    public GameObject enemy;

    /// <summary>
    /// 定义一个类型为float的游戏对象spawnTime，其值为...
    /// 用来控制敌人对象间隔产生的时间
    /// </summary>
    // [HideInInspector]
    public float spawnTime = 5f;

    /// <summary>
    /// 声明一个类型为Transform的数组spawnPoints
    /// 用来设置敌人游戏对象产生的位置
    /// </summary>
    public Transform[] spawnPoints;


    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);         //不断的重复执行Spawn函数
    }


    /// <summary>
    /// 生成地热游戏对象
    /// </summary>
    void Spawn()
    {
        if (playerHealth.currentHealth <= 0f)   //如果玩家对象的当前生命值小于0，则停止生成敌人对象
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);      //设置敌人游戏对象随机产生的位置

        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);       //克隆敌人游戏对象
    }
}
