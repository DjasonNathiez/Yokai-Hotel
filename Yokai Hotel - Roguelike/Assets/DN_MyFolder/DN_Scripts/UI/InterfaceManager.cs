﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    InventoryManager inventory;
    PlayerController player;
    public Slider fillSlider;

    [Header("Health")]
    public RawImage[] allTextureLantern;
    public Texture hpActivate;
    public Texture hpDown;
    /// Texture hpNotActive;

    int currentHealth;
    int maxHealth;

    float shootValue;

    [Header("Money")]
    TextMeshProUGUI moneyText;
    int currentMoney;

    private void Start()
    {
        if(inventory)
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>(); //initialization du script InventoryManager

        if(moneyText)
        moneyText = GameObject.FindGameObjectWithTag("MoneyText").GetComponent<TextMeshProUGUI>();

        allTextureLantern = GetComponentsInChildren<RawImage>();

        //initialization HealthBar
        for(int i = 0; i < allTextureLantern.Length; i++)
        {
            if(i != 0)
            {
                allTextureLantern[i].enabled = true;
            }
        }

    }

    private void Update()
    {

        UpdateHealth();
        UpdateMoney();
        UpdateShoot();
    }

    void UpdateHealth()
    {
        if (inventory)
        {
            currentHealth = inventory.currentHealth;
            maxHealth = inventory.maxHealth;
        }

        for(int i = 0; i < allTextureLantern.Length; i++)
        {
            if(i != 0)
            {
                allTextureLantern[i].texture = (currentHealth >= i) ? hpActivate : hpDown; 
            }
        }
    }

    void UpdateMoney()
    {
        if (inventory)
            currentMoney = inventory.money;

        if(moneyText)
            moneyText.text = currentMoney.ToString();
    }

    void UpdateShoot()
    {
        if (player)
            fillSlider.value = player.shootGaugeState;
    }
}
