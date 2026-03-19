using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    public Text splashText;
    private float fadeDuration = 1f;
    private float holdDuration = 1f;
    private float timer;
    private bool fadingIn = true;
    private bool fadingOut = false;

    void Start()
    {
        SetAlpha(0f);
        Invoke("StartFadeOut", fadeDuration + holdDuration);
        if (SceneManager.GetActiveScene().name == "Splash")
            Invoke("LoadStudio", 3.5f);
        else
            Invoke("LoadMainMenu", 3.5f);
    }

    void Update()
    {
        if (fadingIn)
        {
            timer += Time.deltaTime;
            SetAlpha(Mathf.Clamp01(timer / fadeDuration));
        }
        else if (fadingOut)
        {
            timer += Time.deltaTime;
            SetAlpha(1f - Mathf.Clamp01(timer / fadeDuration));
        }
    }

    void StartFadeOut()
    {
        fadingIn = false;
        fadingOut = true;
        timer = 0f;
    }

    void SetAlpha(float alpha)
    {
        Color c = splashText.color;
        c.a = alpha;
        splashText.color = c;
    }

    void LoadStudio()
    {
        SceneManager.LoadScene("Studio");
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
