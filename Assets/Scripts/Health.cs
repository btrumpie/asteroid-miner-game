using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Player Only")]
    public Image healthSlider;
    public int suffocationDamagePerSecond = 10;

    [Header("Variables")]
    public int maxHealth = 100;
    public int health = 100;
    private float healthPercent = 100f;
    private Oxygen oxygen;
    private int oxygenTimer = 50;

    void Start()
    {
        health = maxHealth;
        oxygen = GetComponent<Oxygen>();
    }

    void Update()
    {
        // Calculate health percent
        healthPercent = 100 * (health / (float)maxHealth);

        // Manipulate health slider for player
        if (healthSlider != null)
        {
            healthSlider.rectTransform.sizeDelta = new Vector2(healthPercent * 2.45f, 41f);
            healthSlider.rectTransform.anchoredPosition = (Vector2.right * healthPercent * 2.45f / 2f) + Vector2.right * 2f;
        }
    }

    void FixedUpdate()
    {
        // Damage player every second if oxygen is zero
        if (oxygen != null && oxygen.oxygen <= 0)
        {
            oxygenTimer -= 1;
            if (oxygenTimer <= 0)
            {
                oxygenTimer = 50;
                health -= suffocationDamagePerSecond;
            }
        }
        else if (oxygen != null && oxygen.oxygen > 0)
        {
            oxygenTimer = 50; // Reset oxygen damage timer if player has oxygen
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
