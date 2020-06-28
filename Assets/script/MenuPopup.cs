using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPopup : MonoBehaviour
{
    bool isStop = true; //判断游戏是否暂停
    public GameObject Menu;

    public void OnQuit() //退出游戏
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isStop = true;
    }

    public void OnGoon() //继续游戏
    {
        Time.timeScale = 1f;
        isStop = true;
        Menu.SetActive(false);
    }

    public void OnReStart() //重新开始
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    void Update() //游戏需要暂停，按下Esc键暂停并显示暂停界面
    {
        if (isStop == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                isStop = false;
                Menu.SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1f;
                isStop = true;
                Menu.SetActive(false);
            }
        }
    }
}
