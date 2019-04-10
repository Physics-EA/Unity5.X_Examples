using UnityEngine;
using System.Collections;


/// <summary>
/// 用来实现天鹅随机飞动的效果
/// </summary>
public class SwanSpawner : MonoBehaviour
{
    /// <summary>
    /// 声明一个类型为 Rigidbody2D 的变量 prop
    /// 表示待生成游戏对象上的刚体组件
    /// </summary>
    public Rigidbody2D prop;

    /// <summary>
    /// 声明一个类型为float的变量 leftSpawnPosX
    /// 表示在X轴上Swan对象能达到的左边界值，当天鹅飞行时，超过这边界值，就会被销毁
    /// </summary>
    float leftSpawnPosX = -24;

    /// <summary>
    /// 声明一个类型为float的变量 rightSpawnPosX
    /// 表示在X轴上Swan对象能达到的右边界值，当天鹅飞行时，超过这边界值，就会被销毁
    /// </summary>
    float rightSpawnPosX = 24;

    /// <summary>
    /// 声明一个类型为float的变量 minSpawnPosY
    /// 用来表示随机生成Swan对象的最小值
    /// </summary>
    float minSpawnPosY = 4;

    /// <summary>
    /// 声明一个类型为float的变量 minSpawnPosY
    /// 用来表示随机生成Swan对象的最小值
    /// </summary>          
    float maxSpawnPosY = 8;

    /// <summary>
    /// 声明一个类型为float的变量minTimeBetweenSpawns
    /// 用来表示随机生成Swan时间间隔的大小
    /// </summary>
    float minTimeBetweenSpawns = 2;

    /// <summary>
    /// 声明一个类型为float的变量minTimeBetweenSpawns
    /// 用来表示随机生成Swan时间间隔的大小
    /// </summary>
    float maxTimeBetweenSpawns = 8;

    /// <summary>
    /// 声明一个类型为float的变量minSpeed
    /// 用来表示随机生成Swan飞行速度的最小值
    /// </summary>
    float minSpeed = 5;

    /// <summary>
    /// 声明一个类型为float的变量minSpeed
    /// 用来表示随机生成Swan飞行速度的最小值
    /// </summary>
    float maxSpeed = 8;

    void Start()
    {
        //将随机数生成器的“种子”设定为当前时刻的毫秒数，这样可以防止随机数生成器在多次运行时生成相同的随机数序列
        Random.seed = System.DateTime.Today.Millisecond;

        //使用StartCoroutine调用协程函数Spawn
        StartCoroutine("Spawn");
    }


    IEnumerator Spawn()
    {
        //定义一个类型为float的变量waitTime，值为...
        //表示两次随机生成过程的时间间隔
        float waitTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

        //？？？
        yield return new WaitForSeconds(waitTime);

        //定义一个类型为bool的变量facingLeft，值为...
        //通过比较Random.Range(0, 2)随机生成的值是否为0，来模拟随机生成bool类型的值
        bool facingLeft = Random.Range(0, 2) == 0;


        //定义一个类型为float的变量posX，值为...
        //？？？
        float posX = facingLeft ? rightSpawnPosX : leftSpawnPosX;

        //定义一个类型为float的变量posY，值为...
        //？？？
        float posY = Random.Range(minSpawnPosY, maxSpawnPosY);


        //定义一个类型为Vector3的变量spawnPos，值为...
        Vector3 spawnPos = new Vector3(posX, posY, transform.position.z);

        //表示在随机位置spawnPos处实例化prop对象
        Rigidbody2D propInstance = Instantiate(prop, spawnPos, Quaternion.identity) as Rigidbody2D;


        if (!facingLeft)
        {

            Vector3 scale = propInstance.transform.localScale;
            scale.x *= -1;
            propInstance.transform.localScale = scale;
        }

        //定义一个类型为float的变量speed，值为...
        //用来表示速度
        float speed = Random.Range(minSpeed, maxSpeed);

        //控制飞行的方向
        speed *= facingLeft ? -1f : 1f;

        //将随机大小的speed值赋值给实例化刚体组件的velocity参数
        propInstance.velocity = new Vector2(speed, 0);

        //表示在协程Spwan内递归调用自身，连续不断的随机生成Swan对象
        StartCoroutine(Spawn());

        //如果Swan对象飞越左右边界将其销毁
        while (propInstance != null)
        {
            // ... and if it's facing left...
            if (facingLeft)
            {
                // ... and if it's beyond the left spawn position...
                if (propInstance.transform.position.x < leftSpawnPosX - 0.5f)
                    // ... destroy the prop.
                    Destroy(propInstance.gameObject);
            }
            else
            {
                // Otherwise, if the prop is facing right and it's beyond the right spawn position...
                if (propInstance.transform.position.x > rightSpawnPosX + 0.5f)
                    // ... destroy the prop.
                    Destroy(propInstance.gameObject);
            }

            // Return to this point after the next update.
            yield return null;
        }
    }
}
