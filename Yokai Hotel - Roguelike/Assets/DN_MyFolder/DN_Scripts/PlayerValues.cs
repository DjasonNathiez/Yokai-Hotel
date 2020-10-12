using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 10;
    public int currentGold;
    public int maxGold = 99;

    private void Update()
    {
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
