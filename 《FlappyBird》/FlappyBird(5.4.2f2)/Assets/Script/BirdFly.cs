//************************
//*
//*
//作者：Lee
//创建时间：2016年11月15日
//功能说明：Flappy Bird
//*
//*
//************************

using UnityEngine;
using System.Collections;

public class BirdFly : MonoBehaviour
{
    private Rigidbody2D rigibody;
    private bool isDie;//默认false 

    // Use this for initialization
    void Start()
    {
        rigibody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie == true) return;

        if (Input.anyKeyDown)
        {
            transform.eulerAngles = new Vector3(0, 0, 60);
            rigibody.velocity = Vector3.up * 6;
        }
        transform.Rotate(-Vector3.forward * 2);
    }

    void OnTriggerEnter2D(Collider2D e)
    {
        if (e.tag == "Piping" || e.tag == "Ground")
        {
            GameManage.instance.Die();
            GameManage.instance.GameOver();
            isDie = true;
            Time.timeScale = 0;
        }
    }


}
