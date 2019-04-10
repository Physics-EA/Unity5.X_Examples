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

public class GameBtn : MonoBehaviour
{
    void OnMouseDown()
    {
        GameManage.instance.StartGame();
    }


}
