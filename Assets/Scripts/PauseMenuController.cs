using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;

    public void PauseClick()
    {
        Debug.Log("You have clicked the button pause");
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void LevelMenuClickdesdeLevel()
    {
        SceneManager.LoadScene("LevelMenú");
    }
    public void ResumeClick()
    {
        Debug.Log("You have clicked the button resume");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void MenuClick()
    {
        SceneManager.LoadScene("Menú");
    }

    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void PlayLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void PlayLevel3()
    {
        SceneManager.LoadScene("Level3");
    }
    public void PlayLevel4()
    {
        SceneManager.LoadScene("Level4");
    }
    public void PlayLevel5()
    {
        SceneManager.LoadScene("Level5");
    }
    public void EndlessClick()
    {
        SceneManager.LoadScene("Endless");
    }
}
