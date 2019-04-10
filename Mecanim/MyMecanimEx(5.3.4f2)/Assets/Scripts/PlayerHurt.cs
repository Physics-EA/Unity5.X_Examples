﻿using UnityEngine;
using System.Collections;

public class PlayerHurt : MonoBehaviour
{
    const float m_WounderDampTime = 0.15f;
    Animator m_Animator;
    float m_Damage = 0;

    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Animator.SetFloat("Wounded", m_Damage, m_WounderDampTime, Time.deltaTime);
        float wounded = m_Animator.GetFloat("Wounded");
        m_Animator.SetLayerWeight(2, Mathf.Clamp01(wounded));
        AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Base Layer.Dying"))
        {
            m_Animator.SetBool("Dead", true);
        }
        else if (info.IsName("Base Layer.Reviving"))
        {
            m_Animator.SetBool("Dead", false);
        }
        else if (info.IsName("Base Layer.Death") && info.normalizedTime > 3)
        {
            m_Damage = 0;
        }
    }


    public void DoDamage(float damage)
    {
        m_Damage += damage;
    }
}
