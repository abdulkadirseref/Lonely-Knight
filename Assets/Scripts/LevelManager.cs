using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int levelToUnlock = 2;
    public SceneFader sceneFader;
    public void WinLevel()
    {
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
       
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
