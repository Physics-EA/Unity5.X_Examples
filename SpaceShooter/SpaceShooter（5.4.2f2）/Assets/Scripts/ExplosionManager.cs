

//1、用来对爆炸粒子生命周期进行管理


using UnityEngine;
using System.Collections;

public class ExplosionManager : MonoBehaviour
{
    /// <summary>
    /// 定义一个类型为float的变量lifetime，值为...
    /// 用来管理爆照例子的生命周期
    /// </summary>
    private  float lifetime = 2.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

}
