using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Other")]
    public Text moneyText;
    private Text winMoneyText;

    [Header("Stats")]
    public int money = 0;
    public float moveSpeed = 45f;
    public float drillSpeed = 15f;
    public float asteroidDeceleratorSpeed = 3f;
    public int maxHealth = 100;
    public float oxygenDepletionRate = 1.4f;

    [Header("Upgrades")]
    public int moveSpeedLevel = 0;
    public int drillSpeedLevel = 0;
    public int asteroidDeceleratorLevel = 0;
    public int maxHealthLevel = 0;
    public int oxygenTankLevel = 0;

    [Header("Controls")]
    public float cameraSensitivity = 1f;
    public float fieldOfView = 60f;

    public float GetValueOf(string variableName)
    {
        string name = variableName.ToLower();

        if (name == "sensitivity")
            return cameraSensitivity;
        else if (name == "fov")
            return fieldOfView;
        return 0;
    }

    public void SetValueOf(string variableName, float value)
    {
        string name = variableName.ToLower();

        if (name == "sensitivity")
            cameraSensitivity = value;
        else if (name == "fov")
            fieldOfView = value;
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (moneyText != null)
            moneyText.text = "$" + money;
    }

    void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            moneyText = FindObjectOfType<Text>();
        }
        else if (SceneManager.GetActiveScene().name == "Win")
        {
            money -= 10000;
            winMoneyText = FindObjectsOfType<Text>()[2];
            winMoneyText.text = "You brought back home $" + money + ".";
            winMoneyText = FindObjectsOfType<Text>()[1];
            winMoneyText.text = "Return to Main Menu";
        }

        PlayerStats[] stats = FindObjectsOfType<PlayerStats>();
        if (stats.Length == 2)
        {
            stats[1].cameraSensitivity = stats[0].cameraSensitivity;
            stats[1].fieldOfView = stats[0].fieldOfView;
            Destroy(stats[0].gameObject);
        }
    }

    public void Upgrade(string thingToUpgrade)
    {
        string thing = thingToUpgrade.ToLower();

        if (thing == "movespeed")
        {
            moveSpeedLevel++;
            moveSpeed += 7.5f;
        }
        else if (thing == "drillspeed")
        {
            drillSpeedLevel++;
            drillSpeed += 3;
        }
        else if (thing == "asteroiddecelerator")
        {
            asteroidDeceleratorLevel++;
            asteroidDeceleratorSpeed += 1;
        }
        else if (thing == "maxhealth")
        {
            maxHealthLevel++;
            maxHealth += 20;
        }
        else if (thing == "oxygentank")
        {
            oxygenTankLevel++;
            oxygenDepletionRate *= 0.8f;
        }
    }
}
