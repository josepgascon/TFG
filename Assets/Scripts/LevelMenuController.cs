using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelMenuController : MonoBehaviour
{
    public GameObject mainMenu;



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

    public void BackClick()
    {
        mainMenu.SetActive(true);
    }

}
