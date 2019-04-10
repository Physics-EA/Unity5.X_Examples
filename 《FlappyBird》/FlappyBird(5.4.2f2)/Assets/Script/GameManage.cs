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
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    private GameObject bird;
    public GameObject help;
    private bool isStart;
    public GameObject start;
    public GameObject gameover;
    public static GameManage instance;
    // Use this for initialization

    void Start()
    {
        instance = this;
        Time.timeScale = 1;
        bird = GameObject.FindGameObjectWithTag("Bird");
        bird.GetComponent<BirdFly>().enabled = false;
        GetComponent<PipingManage>().enabled = false;
        bird.GetComponent<Rigidbody2D>().isKinematic = true;
        start.SetActive(false);
        gameover.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !isStart)
        {
            bird.GetComponent<BirdFly>().enabled = true;
            GetComponent<PipingManage>().enabled = true;
            bird.GetComponent<Rigidbody2D>().isKinematic = false;
            help.SetActive(false);
            isStart = true;
        }
    }

    public void Die()
    {
        start.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Scene");
    }

    public void GameOver()
    {
        gameover.SetActive(true);
    }


}
