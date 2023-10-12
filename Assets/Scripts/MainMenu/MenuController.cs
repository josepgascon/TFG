using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loginMenu;
    public GameObject registerMenu;
    public GameObject levelMenu;
    public GameObject MusicMenu;
    public GameObject StatsMenu;


    public void Level1Click()
    {
        Debug.Log("You have clicked the button level1");

        SceneManager.LoadScene("Level1");
    }

    public void Level2Click()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Level3Click()
    {
        Debug.Log("You have clicked the button level3");

        SceneManager.LoadScene("Level3");
    }

    public void MainMenuClick()
    {
        mainMenu.SetActive(true);
        loginMenu.SetActive(false);
        registerMenu.SetActive(false);
        levelMenu.SetActive(false);
        MusicMenu.SetActive(false);
        StatsMenu.SetActive(false); 
    }

    public void loginClick()
    {
        mainMenu.SetActive(false);
        loginMenu.SetActive(true);
        registerMenu.SetActive(false);
        levelMenu.SetActive(false);
    }
    public void registerClick()
    {
        mainMenu.SetActive(false);
        loginMenu.SetActive(false);
        registerMenu.SetActive(true);
        levelMenu.SetActive(false);
    }

    public void levelClick()
    {
        mainMenu.SetActive(false);
        loginMenu.SetActive(false);
        registerMenu.SetActive(false);
        levelMenu.SetActive(true);
        StatsMenu.SetActive(false);
    }

    public void musicClick()
    {
        //Debug.Log(Main.currentUser);
        //StartCoroutine(Main.Instance.DBController.GetUserLevelAttempt("1", "1"));
        //StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt("2", "2", "1", "1", "1", "1"));
        //StartCoroutine(Main.Instance.DBController.DeleteUserLevelAttempt("2", "2"));

        mainMenu.SetActive(false);
        MusicMenu.SetActive(true);
    }

    public void statsClick()
    {
        levelMenu.SetActive(false);
        StatsMenu.SetActive(true);
    }

    public void MenuClickdesdeLevel()
    {
        SceneManager.LoadScene("Menú");
    }

    public void DemoClick()
    {
        SceneManager.LoadScene("Demo");
    }
}
