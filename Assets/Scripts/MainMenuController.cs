using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;


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

}
