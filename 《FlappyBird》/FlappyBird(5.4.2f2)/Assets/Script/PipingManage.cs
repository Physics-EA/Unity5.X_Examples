//************************
//*
//*
//作者：Lee
//创建时间：YYYY年MM月DD日
//功能说明：
//*
//*
//************************

using UnityEngine;
using System.Collections;

public class PipingManage : MonoBehaviour
{
    public GameObject piping;
    private float time;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time <= 1.5f) return;

        time = 0;
        GameObject g = Instantiate(piping, new Vector3(3f, Random.Range(-2, 3), 0), Quaternion.identity) as GameObject;

        g.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 2);
    }


}
