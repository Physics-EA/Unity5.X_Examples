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

public class PipingMove : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, 6);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector2.right * 2 * Time.deltaTime);
    }


}
