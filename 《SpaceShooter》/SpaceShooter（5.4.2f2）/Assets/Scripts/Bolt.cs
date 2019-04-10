

using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour
{
    /// <summary>
    /// 声明一个类型为float的变量speed
    /// 用作控制子弹的速度；
    /// </summary>
    public float speed;

    void Start()
    {
        //子弹上有刚体组件Rigidbody，通过GetComponent方法获得组件Rigidbody(类)，设置刚体Bolt的速度vleocity
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
}
