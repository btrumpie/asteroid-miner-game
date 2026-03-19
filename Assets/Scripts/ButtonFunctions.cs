using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void LoadWin()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null && playerStats.money >= 10000)
        {
            SceneManager.LoadScene("Win");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
