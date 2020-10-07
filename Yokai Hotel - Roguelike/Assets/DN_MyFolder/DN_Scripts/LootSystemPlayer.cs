using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSystemPlayer : MonoBehaviour
{
    PlayerValues playerValues;
    LootSystemObject lootSystemObject;

    private void Start()
    {
        playerValues = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerValues>();
        lootSystemObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LootSystemObject>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gold"))
        {
            playerValues.currentGold += lootSystemObject.goldValue;
        }
    }
}
