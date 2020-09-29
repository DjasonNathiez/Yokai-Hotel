using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HUDManager : MonoBehaviour
{
    [Serializable]
    private class HealthBarUI
    {
        public string name;
        public Image image;
        public int healthValue;
    }

    [SerializeField] HealthBarUI[] healthBarUI;

    TextMeshProUGUI goldText;
    public int goldAmount;
    int goldAmountMax = 999;

    private void Start()
    {
        goldText = GameObject.FindGameObjectWithTag("GoldUI").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        Health();
        Gold();
    }

    void Health() //update the health bar
    {
        ///IF : PV < HealthValue => sprite[associé].color = new Color(100, 100, 100, 255)
    }
    void Gold() //gold counter with limit
    {
        goldText.text = goldAmount.ToString(); //the text

        if(goldAmount > goldAmountMax) //creat a limit for the text
        {
            goldText.text = "+" + goldAmountMax.ToString();
        }
    }
}
