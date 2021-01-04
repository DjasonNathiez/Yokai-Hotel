using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public PlayerController player;
<<<<<<< Updated upstream
    public int money, health, maxHealth,currentHealth, attackBoost, keys;
=======
    public float money, health, maxHealth,currentHealth, attackBoost;
    public Enchantement[] enchantements;
>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        maxHealth = player.health;

    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0 && player.health < maxHealth)
        {
            player.health += health;
            health = 0;
        }
        currentHealth = player.health;

    }
}
