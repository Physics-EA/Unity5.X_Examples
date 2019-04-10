using UnityEngine;
using System.Collections;

public class AlarmLight : MonoBehaviour
{
    /// <summary>
    /// 警报灯光亮度渐变速度
    /// </summary>
    public float fadeSpeed = 2f;

    /// <summary>
    /// 警报灯的亮度最大值
    /// </summary>
    public float highIntensity = 2f;

    /// <summary>
    /// 警报灯的亮度最小值
    /// </summary>
    public float lowIntensity = 0.5f;

    /// <summary>
    /// 阈值，警报灯的当前亮度与targetIntensity的差值
    /// </summary>
    public float changeMargin = 0.2f;

    /// <summary>
    /// 表示是否开启警报灯
    /// </summary>
    public bool alarmOn;

    /// <summary>
    /// 目标亮度值
    /// </summary>
    private float targetIntensity;

    /// <summary>
    /// 当前场景中的警报灯对象
    /// </summary>
    private Light alarmLight;


    void Start()
    {
        //取得绑定在游戏对象上的Light组件
        alarmLight = GetComponent<Light>();
        //初始化亮度值为0
        alarmLight.intensity = 0f;
        //目标亮度初始值为最大亮度值
        targetIntensity = highIntensity;
    }


    void Update()
    {
        //当开启警报灯时，警报灯的亮度值从当前值均匀插值到targetIntensity，若接近最大亮度值时，targetIntensity切换为最小亮度值，再将当前值均匀插值到最小亮度值
        if (alarmOn)
        {
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
            CheckTargetIntensity();
        }
        //如果未开启警报灯，则从当前亮度值均匀插值到0
        else
        {
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, 0f, fadeSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 如果当前亮度值与目标亮度值得差与阈值比较
    /// </summary>
    void CheckTargetIntensity()
    {
        if (Mathf.Abs(targetIntensity - alarmLight.intensity) < changeMargin)
        {
            if (targetIntensity == highIntensity)
            {
                targetIntensity = lowIntensity;
            }
            else
            {
                targetIntensity = highIntensity;
            }
        }
    }

}
