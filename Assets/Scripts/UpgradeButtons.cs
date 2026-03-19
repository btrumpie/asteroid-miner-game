using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtons : MonoBehaviour
{
    private PlayerStats playerStats;
    private Text upgradeText;
    private Text infoText;
    private int level = 0;

    public string thingToUpgrade;
    public int cost = 300;
    public float costMultiplier = 1.5f;
    public int costAddition = 0;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        upgradeText = GetComponentInChildren<Text>();
        infoText = GetComponentInParent<Text>();
    }

    public void Upgrade()
    {
        if (playerStats.money < cost) return;

        playerStats.money -= cost;
        playerStats.Upgrade(thingToUpgrade);
        cost = (int)((float)cost * costMultiplier + costAddition);
        upgradeText.text = "Upgrade ($" + cost + ")";
        level++;
        infoText.text = infoText.text.Substring(0, infoText.text.IndexOf(':')) + ": " + level;
    }
}
