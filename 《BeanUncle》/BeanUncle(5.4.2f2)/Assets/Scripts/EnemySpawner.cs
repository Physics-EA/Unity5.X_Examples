using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    /// <summary>
    /// 定义一个类型为float的变量spawnTime，值为...
    /// 表示重复调用函数Spawn的时间间隔
    /// </summary>
    [HideInInspector]
    public float spawnTime = 5f;

    /// <summary>
    /// 定义一个类型为float的变量spawnDelay，值为...
    /// 表示初次调用函数Spawn的延迟时间
    /// </summary>
    [HideInInspector]
    public float spawnDelay = 3f;


    /// <summary>
    /// 声明一个类型为GameObject的数组变量enemies
    /// 表示待实例化的敌人数组
    /// </summary>
    public GameObject[] enemies;


    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Spawn", spawnDelay, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Spawn()
    {
        int enemyIndex = Random.Range(0, enemies.Length);
        Instantiate(enemies[enemyIndex], transform.position, transform.rotation);
    }
}
