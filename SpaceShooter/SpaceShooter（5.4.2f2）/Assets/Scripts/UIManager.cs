

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// 声明一个类型为Text的变量gameOverText
    /// 用来在中央显示游戏结束时间
    /// </summary>
    public Text gameOverText;

    /// <summary>
    /// 声明一个类型为bool的变量gameOver
    /// 用来判断是否结束游戏
    /// </summary>
    private bool gameOver;

    /// <summary>
    /// 声明一个类型为Text的变量restarText
    /// 用来显示重新开始信息
    /// </summary>
    public Text restarText;

    /// <summary>
    /// 声明一个类型为bool的变量restart
    /// 用来判断是否重新开始游戏
    /// </summary>
    private bool restart;

    /// <summary>
    /// 声明一个类型为Text的变量scoreText
    /// 用来显示计分
    /// </summary>
    public Text scoreText;

    /// <summary>
    /// 声明一个类型为int的变量score
    /// 用来统计计分
    /// </summary>
    private int score;




    IEnumerator SpawnWaves()
    {

        while (true)
        {
            if (gameOver)
            {
                restarText.text = "按【R】重新开始";
                restart = true;
                break;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }


    void Start()
    {
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
        gameOverText.text = "";
        gameOver = false;
        restarText.text = "";
        restart = false;
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "得分:" + score;
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverText.text = "游戏结束";
    }


}
