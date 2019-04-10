using UnityEngine;
using System.Collections;



/// <summary>
/// 该脚本用于用于实现摄像机跟随角色运动
/// </summary>
public class CameraFollow : MonoBehaviour
{
    /// <summary>
    /// 声明一个类型为GameObject的变量target
    /// 目标位置，即角色位置
    /// </summary>
    [HideInInspector]
    public GameObject target;

    /// <summary>
    /// 定义一个类型为float的变量smoothing，值为...
    /// 用于插值时控制速度的系数
    /// </summary>
    [HideInInspector]
    public float smoothing = 7f;

    /// <summary>
    /// 角色与摄像机之间的向量差，将被用于根据角色当前的位置来更新摄像机的位置
    /// </summary>
    Vector3 offset;


    void Start()
    {
        target = GameObject.Find("Player");     //  获得目标游戏对象Player
        offset = transform.position - target.transform.position;    //摄像机与游戏对象之间的“初始/固定”向量差

    }

    private void FixedUpdate()
    {
        Vector3 camPos = target.transform.position + offset;       //定义一个类型为Vector3的变量camPos用来存放摄像机的当前位置
        transform.position = Vector3.Lerp(transform.position, camPos, smoothing * Time.deltaTime);      //就是以一定的速度比例t，平滑的从start位置移动到end位置；
                                                                                                        //试比较没有Lerp的情况，直接将start位置设置为到end位置；
                                                                                                        //transform.position = camPos;

    }

}
