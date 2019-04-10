using UnityEngine;
using System.Collections;

public class NPCName : MonoBehaviour
{
    /// <summary>
    /// 定义一个类型为string的变量lableText，值为“ ”
    /// 表示NPC上方显示的标签文本
    /// </summary>
    public string lableText = "";

    /// <summary>
    /// 声明一个类型为GUISkin的变量customSkin
    /// 表示标签设置外观的GUISkin对象
    /// </summary>
    public GUISkin customSkin = null;

    /// <summary>
    /// 定义一个类型为string的变量styleName，值为Box
    /// 表示GUISkin中标签对应的样式名称
    /// </summary>
    public string styleName = "Box";

    /// <summary>
    /// 声明一个类型为Camera的变量guiCamera，其值为空
    /// </summary>
    public Camera guiCamera = null;

    /// <summary>
    /// 定义一个类型为float的变量fadeDistance，值为12
    /// 表示当Player与NPC的距离小于此值时则显示标签
    /// </summary>
    public float fadeDistance = 12.0f;

    /// <summary>
    /// 定义一个类型为float的变量hideDistance，值为17
    /// 表示当Player与NPC的距离大于此值时则隐藏标签
    /// </summary>
    public float hideDistance = 17.0f;

    /// <summary>
    /// 定义一个类型为float的变量maxVeiwAngle，值为90
    /// 表示Player对象最大的观察角度
    /// </summary>
    public float maxViewAngle = 90.0f;

    private void OnGUI()
    {
        useGUILayout = false;       //表示在函数OnGUI中不使用函数GUI.Window或GUILayout

        //只在EventType.Repaint事件中才绘制标签，忽略其他事件
        if (Event.current.type != EventType.Repaint)
        {
            return;
        }

        Vector3 worldPosition = GetComponent<Collider>().bounds.center + Vector3.up * GetComponent<Collider>().bounds.size.y * 0.5f;

        Vector3 distance = worldPosition - guiCamera.transform.position;

        float cameraDistance = distance.magnitude;

        //当cameraDistance大于hideDistance或摄像机的前向与distance的夹角大于maxViewAngle时，不绘制标签
        if (cameraDistance > hideDistance || Vector3.Angle(guiCamera.transform.forward, distance) > maxViewAngle)
        {
            return;
        }

        //当cameraDistance大于fadeDistance时，与NPC的距离越大，则标签的Alpha值越小
        if (cameraDistance > fadeDistance)
        {
            GUI.color = new Color(1.0f, 1.0f, 1.0f - (cameraDistance - fadeDistance) / (hideDistance - fadeDistance));
        }

        Vector2 position = guiCamera.WorldToScreenPoint(worldPosition);     //表示将WorldPosition从世界做标系转换到屏幕坐标系

        position = new Vector2(position.x, Screen.height - position.y);

        GUI.skin = customSkin;

        string contents = string.IsNullOrEmpty(lableText) ? gameObject.name : lableText;

        Vector2 size = GUI.skin.GetStyle(styleName).CalcSize(new GUIContent(contents));     //根据指定的样式计算标签的尺寸

        Rect rect = new Rect(position.x - size.x * 0.5f, position.y - size.y, size.x, size.y);

        GUI.skin.GetStyle(styleName).Draw(rect, contents, false, false, false, false);      //表示实现标签的绘制
    }



}
