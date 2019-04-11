using UnityEngine;
using System.Collections;

public class LaserBlinking : MonoBehaviour
{
    /// <summary>
    /// 间隔onTime后灯灭
    /// </summary>
    public float onTime;
    /// <summary>
    /// 间隔offTime后灯灭
    /// </summary>
    public float offTime;
    /// <summary>
    /// 记录流逝的时间
    /// </summary>
    private float timer;
    /// <summary>
    /// Laser对象上的Renderer组件对象
    /// </summary>
    private Renderer laserRenderer;
    /// <summary>
    /// Laser对象上的Light组件
    /// </summary>
    private Light laserLight;

    void Awake()
    {
        laserRenderer = GetComponent<Renderer>();
        laserLight = GetComponent<Light>();
        timer = 0f;
    }

    void SwitchBeam()
    {
        timer = 0f;
        laserRenderer.enabled = !laserRenderer.enabled;
        laserLight.enabled = !laserLight.enabled;
    }

       
    void Update()
    {
        timer += Time.deltaTime;
        if (laserRenderer.enabled && timer >= onTime)
        {
            SwitchBeam();
        }
        if (!laserRenderer.enabled && timer >= offTime)
        {
            SwitchBeam();
        }
    }
}
