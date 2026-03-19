using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    private PlayerStats playerStats;
    private Slider slider;
    private float value = 0;
    public string optionName;

    void Start()
    {
        slider = GetComponent<Slider>();
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats != null)
            value = playerStats.GetValueOf(optionName);

        slider.value = value;
    }

    void Update()
    {
        playerStats.SetValueOf(optionName, slider.value);
    }
}
