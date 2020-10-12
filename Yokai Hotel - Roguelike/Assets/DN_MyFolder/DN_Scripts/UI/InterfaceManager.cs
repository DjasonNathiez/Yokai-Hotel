using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    InventoryManager inventory;

    [Header("Health")]
    public RawImage[] allTextureLantern;
    public Texture hpActivate;
    public Texture hpDown;
    /// Texture hpNotActive;

    int currentHealth;
    int maxHealth;

    [Header("Money")]
    TextMeshProUGUI moneyText;
    int currentMoney;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>(); //initialization du script InventoryManager
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
        currentMoney = inventory.money;
        moneyText.text = currentMoney.ToString();
    }

}
