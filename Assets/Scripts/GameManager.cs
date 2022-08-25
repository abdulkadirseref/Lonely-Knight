using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    public PlayerMovement playerMovement;

    public int levelToUnlock = 2;

    private void Update()
    {
        EndGame();
        playerMovement.SetStar(levelToUnlock - 1);
    }

    public void EndGame()
    {
        if (playerMovement.Health <= 0)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void WinLevel()
    {
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            playerMovement.TakeDamage(100);
        }
    }
}
