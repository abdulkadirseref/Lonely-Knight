using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Animator anim;

    private int levelToLoad;

    public GameManager gameManager;

    public void FadeToLevel(int sceneIndex)
    {
        levelToLoad = sceneIndex;
        anim.SetTrigger("FadeOut");
    }


    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void FadeTo(string sceneName)
    {
        anim.SetTrigger("FadeOut");
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            FadeToNextLevel();
            gameManager.WinLevel();
        }
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
