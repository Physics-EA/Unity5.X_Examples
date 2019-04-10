using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    /// <summary>
    /// 声明一个类型为GameObject的变量player
    /// 用来存放目标对象
    /// </summary>
    GameObject player;

    /// <summary>
    ///  声明一个类型为PlayerHealth的变量playerHealth
    ///  用来存放玩家的生命值脚本
    /// </summary>
    PlayerHealth playerHealth;

    /// <summary>
    /// 声明一个类型为EnemyHealth的变量enemyHealth
    /// 用来存放敌人的生命值脚本
    /// </summary>
    EnemyHealth enemyHealth;

    /// <summary>
    /// 声明一个类型为NavMeshAgent的变量nav
    /// 用来存放当前游戏对象上的Nav Mesh Agent 组件
    /// </summary>
    UnityEngine.AI.NavMeshAgent nav;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");    //初始化目标对象
        playerHealth = player.GetComponent<PlayerHealth>();     //得到玩家对象上的PlayerHealth脚本组件
        enemyHealth = GetComponent<EnemyHealth>();      //得到当前敌人游戏对象上的EnemyHealth脚本组件
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();     //得到当前游戏对象上的Nav Mesh Agent组件
    }


    void Update()
    {
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)    //如果敌人和玩家的生命值都大于0
        {
            nav.SetDestination(player.transform.position);      //将敌人游戏对象向玩家游戏对象的位置移动
        }
        else
        {
            nav.enabled = false;    //使组件失效
        }
    }
}
