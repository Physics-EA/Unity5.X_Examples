using UnityEngine;
using System.Collections;

public class MyAnimatorUI : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUILayout.Label("运动时按Fire1键（左Ctrl键）实现跳跃动作。按Fire2键（左Alt键），实现打招呼的动作。");
    }
}
