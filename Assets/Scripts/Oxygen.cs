using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    public Image oxygenSlider;

    public int oxygen = 100;
    public float oxygenReplenishRate = 30;
    public bool inOxygenatedArea = false;

    private PlayerStats playerStats;
    private float oxygenDepletionRate = 3;
    private float timeUntilNextDepletion = 1;
    private float timeUntilNextReplenish = 1;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        oxygenDepletionRate = playerStats.oxygenDepletionRate;

        if (!inOxygenatedArea && timeUntilNextDepletion <= 0 && oxygen > 0)
        {
            oxygen -= 1;
            timeUntilNextDepletion = 1f / oxygenDepletionRate;

            oxygenSlider.rectTransform.sizeDelta = new Vector2(oxygen * 2.45f, 41f);
            oxygenSlider.rectTransform.anchoredPosition = (Vector2.right * oxygen * 2.45f / 2f) + Vector2.right * 2f;
        }
        else if (inOxygenatedArea && timeUntilNextReplenish <= 0 && oxygen < 100)
        {
            oxygen += 1;
            timeUntilNextReplenish = 1f / oxygenReplenishRate;

            oxygenSlider.rectTransform.sizeDelta = new Vector2(oxygen * 2.45f, 41f);
            oxygenSlider.rectTransform.anchoredPosition = (Vector2.right * oxygen * 2.45f / 2f) + Vector2.right * 2f;
        }

        timeUntilNextReplenish -= Time.deltaTime;
        timeUntilNextDepletion -= Time.deltaTime;
    }
}
