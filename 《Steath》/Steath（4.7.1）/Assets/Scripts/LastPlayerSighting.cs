using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour
{
    /// <summary>
    /// 初始位置
    /// </summary>
    public Vector3 position = new Vector3(1000f, 1000f, 1000f);
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);
    /// <summary>
    /// 高亮度
    /// </summary>
    public float lightHighIntensity = 0.25f;
    /// <summary>
    /// 低亮度
    /// </summary>
    public float lightLowIntensity = 0f;
    public float fadeSpeed = 7f;
    public float musicFadeSpeed = 1f;

    /// <summary>
    /// AlarmLight脚本对象
    /// </summary>
    private AlarmLight alarmScript;

    /// <summary>
    /// 主灯光上的Light对象
    /// </summary>
    private Light mainLight;

    /// <summary>
    /// 背景音乐
    /// </summary>
    private AudioSource music;

    /// <summary>
    /// 当角色处于危险处境时播放的音乐
    /// </summary>
    private AudioSource panicAudio;

    /// <summary>
    /// 警报音乐
    /// </summary>
    private AudioSource[] sirens;

    /// <summary>
    /// 静音音量
    /// </summary>
    private const float muteVolume = 0f;

    /// <summary>
    /// 正常音量
    /// </summary>
    private const float normalVolume = 0.8f;


    void Awake()
    {
        //警报灯对象上的AlarmLight脚本组件
        alarmScript = GameObject.FindWithTag(Tags.AlarmLight).GetComponent<AlarmLight>();
        //主灯光上的Light组件
        mainLight = GameObject.FindWithTag(Tags.MainLight).GetComponent<Light>();
        //主背景英语
        music = GetComponent<AudioSource>();
        //secondary_music对象上的AudioSource组件
        panicAudio = transform.Find("secondary_music").GetComponent<AudioSource>();
        //警报声源对象数组
        GameObject[] sirenGameObjects = GameObject.FindGameObjectsWithTag(Tags.Siren);
        sirens = new AudioSource[sirenGameObjects.Length];
        for (int i = 0; i < sirens.Length; ++i)
        {
            sirens[i] = sirenGameObjects[i].GetComponent<AudioSource>();
        }
    }


    void Update()
    {
        SwitchAlarms();
        MusicFading();
    }


    /// <summary>
    /// 用来切换警报声源
    /// </summary>
    void SwitchAlarms()
    {
        //表示角色位置变动，进入了危险状态
        alarmScript.alarmOn = (position != resetPosition);
        float newIntensity;
        //当角色处于危险状态时，均匀调整主灯光的Intensity到lightLowIntensity
        if (position != resetPosition)
        {
            newIntensity = lightLowIntensity;
        }
        //当角色处于安全状态，均匀调整主灯光的Intensity到lightHighIntensity
        else
        {
            newIntensity = lightHighIntensity;
        }
        mainLight.intensity = Mathf.Lerp(mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);

        //遍历sirens音频列表，当角色处于危险状态切音频为播放时，则播放音频，否则停止播放
        for (int i = 0; i < sirens.Length; i++)
        {
            if (position != resetPosition && !sirens[i].isPlaying)
            {
                sirens[i].Play();
            }
            else if (position == resetPosition)
            {
                sirens[i].Stop();
            }
        }

    }


    /// <summary>
    /// 实现音量大小渐变的效果
    /// </summary>
    void MusicFading()
    {
        //当角色不在安全位置时，则播放紧张音乐，并且停止播放背景音乐
        if (position != resetPosition)
        {
            music.volume = Mathf.Lerp(music.volume, muteVolume, musicFadeSpeed * Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, normalVolume, musicFadeSpeed * Time.deltaTime);
        }
        //当角色处于安全位置时，则播放正常的背景音乐
        else
        {
            music.volume = Mathf.Lerp(music.volume, normalVolume, musicFadeSpeed * Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, muteVolume, musicFadeSpeed * Time.deltaTime);
        }

    }


}
