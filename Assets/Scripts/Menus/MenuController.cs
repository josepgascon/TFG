using UnityEngine;
using UnityEngine.SceneManagement;

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
