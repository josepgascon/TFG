using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;

    public Button BSelect, BLogin, BStats, BSettings;
    public Button BLevel1, BLevel2, BLevel3, BBack;

    public void SelectClick()
    {
        Debug.Log("You have clicked the button select!");
        mainMenu.SetActive(false);
    }

    public void LoginClick()
    {
        Debug.Log("You have clicked the button login!");
        mainMenu.SetActive(false);

        throw new NotImplementedException();
    }

    public void StatsClick()
    {
        Debug.Log("You have clicked the stats login!");
    }

    public void SettingsClick()
    {
        throw new NotImplementedException();
    }


    public void Level1Click()
    {
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

    private void BackClick()
    {
        mainMenu.SetActive(true);
    }

    void Start()
    {
        BSelect.onClick.AddListener(SelectClick);
        BLogin.onClick.AddListener(LoginClick);
        BStats.onClick.AddListener(StatsClick);
        BSettings.onClick.AddListener(SettingsClick);
        BLevel1.onClick.AddListener(Level1Click);
        BLevel2.onClick.AddListener(Level2Click);
        BLevel3.onClick.AddListener(Level3Click);
        BBack.onClick.AddListener(BackClick);
    }

   

    void Update()
    {

    }

}

