using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnimationRecorder : MonoBehaviour
{

    Animator m_Animator;
    public Texture Play;
    public Texture Next;
    public Texture Prev;
    public Texture Pause;
    const int FrameCount = 500;
    public bool isRecording;
    float m_TimeLinePixelSize;
    const float buttonBorderWidth = 4;
    Dictionary<int, string> m_StateDictionnary = new Dictionary<int, string>();
    List<int> samples = new List<int>();




    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        StartRecord();
        InitStateDictionnary();
    }





    // Update is called once per frame
    void Update()
    {
        if (isRecording)
        {
            if (samples.Count == (FrameCount - 1))
            {
                samples.RemoveAt(0);
            }
            samples.Add(m_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
        }
    }


    private void OnGUI()
    {
        if (isRecording)
        {
            if (GUILayout.Button(Pause))
            {
                StopRecord();
            }
        }
        else
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Prev, GUILayout.ExpandWidth(false)))
            {
                m_Animator.playbackTime -= 0.03f;
            }

            if (GUILayout.Button(Play, GUILayout.ExpandWidth(false)))
            {
                StartRecord();
                return;
            }
            if (GUILayout.Button(Next, GUILayout.ExpandWidth(false)))
            {
                m_Animator.playbackTime += 0.03f;
            }
            GUILayout.EndHorizontal();
            m_TimeLinePixelSize = Screen.width - 10;
            m_Animator.playbackTime = GUILayout.HorizontalSlider(m_Animator.playbackTime, m_Animator.recorderStartTime, m_Animator.recorderStopTime, GUILayout.Width(m_TimeLinePixelSize));
        }


        if (GUI.Button(new Rect(Screen.width - 65, 0, 65, 20), "重置"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }


    private void InitStateDictionnary()
    {
        m_StateDictionnary[Animator.StringToHash("Base Layer.Locomotion.Idle")] = "Idle";
        m_StateDictionnary[Animator.StringToHash("Base Layer.Locomotion.Run")] = "Run";
        m_StateDictionnary[Animator.StringToHash("Base Layer.Locomotion.TurnOnSpot")] = "TurnOnSpot";
        m_StateDictionnary[Animator.StringToHash("Base Layer.Locomotion.Slide")] = "Slide";
        m_StateDictionnary[Animator.StringToHash("Base Layer.Locomotion.Vault")] = "Vault";
        m_StateDictionnary[Animator.StringToHash("Base layer.Locomotion.Dying")] = "Dying";
        m_StateDictionnary[Animator.StringToHash("Base layer.Locomotion.Death")] = "Death";
        m_StateDictionnary[Animator.StringToHash("Base layer.Locomotion.Reviving")] = "Reviving";


    }


    private void StartRecord()
    {
        isRecording = true;
        samples.Clear();
        m_Animator.StopPlayback();
        m_Animator.StartRecording(FrameCount);
    }


    private void StopRecord()
    {
        isRecording = false;
        m_Animator.StopRecording();
        m_Animator.StartPlayback();
    }
}
