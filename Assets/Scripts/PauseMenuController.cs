using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;


    public void PauseClick()
    {
        Debug.Log("You have clicked the button pause");
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ResumeClick()
    {
        Debug.Log("You have clicked the button resume");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }


    public void MenuClick()
    {
        SceneManager.LoadScene("Men�");

    }

}