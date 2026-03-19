using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesMoneyText : MonoBehaviour
{
    public Text originalMoneyText;
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = originalMoneyText.text;
    }
}
