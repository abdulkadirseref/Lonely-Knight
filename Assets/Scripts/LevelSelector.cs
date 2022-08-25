using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    public SceneFader fader;



    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);


        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                levelButtons[i].interactable = false;
            }
        }

        if (!PlayerPrefs.HasKey("stars"))
        {
            FillStars();
        }

        ActivateStars();
    }

    public string starCounts;
    public void FillStars()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            starCounts += "0,";
        }

        PlayerPrefs.SetString("stars", starCounts);
        starCounts = PlayerPrefs.GetString("stars");
    }

    public string[] newStars;
    public void ActivateStars()
    {
        newStars = PlayerPrefs.GetString("stars").Split(',');

        for (int i = 0; i < levelButtons.Length; i++)
        {
            for (int j = 0; j < int.Parse(newStars[i]); j++)
            {
                levelButtons[i].transform.GetChild(1).GetChild(j).GetComponent<Image>().color = new Color(255, 255, 255);
            }
        }
    }


    public void SelectLevel(string levelName)
    {
        fader.FadeTo(levelName);
        SceneManager.LoadScene(levelName);
    }

    public void ReturnMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}

