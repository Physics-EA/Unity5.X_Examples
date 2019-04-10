

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AsteroidManager : MonoBehaviour
{

    /// <summary>
    /// 声明一个类型为GameObject的变量hazard
    /// 用来存放Asteroid（小行星）
    /// </summary>
    public GameObject hazard;

    /// <summary>
    /// 声明一个类型为Vector3的变量spawnValues
    /// 用来限制Asteroid的位置（范围）
    /// </summary>
    public Vector3 spawnValues;

    /// <summary>
    /// 定义一个类型为Vector3的变量spawnPosition，值为...
    /// 用来存放随机实例化的Asteroid的位置
    /// </summary>
    private Vector3 spawnPosition = Vector3.zero;

    /// <summary>
    /// 声明一个类型为Quaternion的变量spawnRotation
    /// 用来设置Asteroid的角度
    /// </summary>
    private Quaternion spawnRotation;

    /// <summary>
    /// 声明一个类型为int的变量hazardCount
    /// 用来确定随机产生小行星的数量
    /// </summary>
    public int hazardCount;

    /// <summary>
    /// 声明一个类型为float的变量spawnWait
    /// 用来表示每次生成小行星后延迟的时间
    /// </summary>
    public float spawnWait;

    /// <summary>
    /// 定义一个类型为float的变量startWait，值为...
    /// 用来表示开始生成小行星对象前等待的时间 
    /// </summary>
    public float startWait = 0.1f;

    /// <summary>
    /// 定义一个类型为float的变量，值为...
    /// 用来确定两批小行星阵列间的时间间隔
    /// </summary>
    public float waveWait = 1.0f;



    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {

            for (int i = 0; i < hazardCount; ++i)
            {
                spawnPosition.x = Random.Range(-spawnValues.x, spawnValues.x);
                spawnPosition.z = spawnValues.z;
                spawnRotation = Quaternion.identity;

                Instantiate(hazard, spawnPosition, spawnRotation);

                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }



}
