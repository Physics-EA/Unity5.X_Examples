using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
///用于不断地更新ScoreText对象上Text组件上的属性值
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    ///声明一个类型为int的静态变量score
    ///用于统计得分
    /// </summary>
    public static int score;

    /// <summary>
    /// 声明一个类型为Text的变量text
    /// 用于获得当前UI对象上的Text组件
    /// </summary>
    Text text;

    void Awake()
    {
        text = GetComponent<Text>();    //获得当前游戏对象上的Text组件
        score = 0;      //初始化得分为0
    }


    void Update()
    {
        text.text = "Score : " + score;     //更改当前游戏对象上Text组件的Text属性值
    }
}
