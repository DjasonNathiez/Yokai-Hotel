using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSystemPlayer : MonoBehaviour
{
    PlayerValues playerValues;
    LootSystemObject lootSystemObject;
    HUDManager hUDManager;

    private void Start()
    {
        playerValues = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerValues>();
        lootSystemObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LootSystemObject>();
        hUDManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<HUDManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gold"))
        {
            playerValues.currentGold += lootSystemObject.goldValue;
        }

        if (collision.CompareTag("NeckLife"))
        {
            playerValues.maxHealth += 1;
            hUDManager.healing = true;
        }

        if (collision.CompareTag("Heal"))
        {
            if(playerValues.currentHealth == playerValues.maxHealth)
            {

            }
            else
            {
               hUDManager.healActive = true;
            }

        }
    }
}
