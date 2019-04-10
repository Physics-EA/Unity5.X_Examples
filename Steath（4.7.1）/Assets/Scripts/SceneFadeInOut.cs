using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFadeInOut : MonoBehaviour
{

    public float fadeSpeed = 0.5f;
    private bool sceneStarting = true;
    private RawImage rawImage = null;


    void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }


    void Update()
    {
        if (sceneStarting)
        {
            StartScene();
        }
    }

    /// <summary>
    /// 将RawImage的颜色从初始值均匀变成（0，0，0，0）
    /// </summary>
    void FadeToClear()
    {
        rawImage.color = Color.Lerp(rawImage.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 将RawImage的颜色从（0，0，0，0）均匀变成（0，0，0，1）
    /// </summary>
    void FadeToBlack()
    {
        rawImage.color = Color.Lerp(rawImage.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 通过调用FadeToClear函数实现了在场景开始时将RawImage的Alpha值渐变为0
    /// </summary>
    void StartScene()
    {
        FadeToClear();
        if (rawImage.color.a <= 0.05)
        {
            rawImage.color = Color.clear;
            rawImage.enabled = false;
            sceneStarting = false;
        }
    }


    /// <summary>
    /// 在场景结束时通过调用FadeToBlack函数将Alpha值渐变为1
    /// </summary>
    public void EndScene()
    {
        rawImage.enabled = true;
        FadeToBlack();
        if (rawImage.color.a >= 0.95f)
        {
            Application.LoadLevel(0);
        }
    }

}
