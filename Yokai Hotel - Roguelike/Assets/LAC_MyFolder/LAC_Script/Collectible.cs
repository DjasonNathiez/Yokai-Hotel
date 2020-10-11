using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;

public class Collectible : MonoBehaviour
{
    
    public enum Collectype { MONEY, HEALTH, MAXHEALTH, ATTACKBOOST}
    public Collectype collectype; //define collectible type
    public int value; // choose value

    // magnet range -> collectible was attract by the player
    public float magnetRange, magnetSpeed, magnetAccel, pickUpRange;
    public LayerMask obstructMask;
    bool magnet, pickUp;

    float speed;
    public bool  oscilate;
    public float oscilMag, oscilFreq;
    GameObject playerObj;
    InventoryManager inventory;
    HUDManager hudManager;

    Vector2 playerDir;
    float playerDist;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        hudManager = GameObject.FindGameObjectWithTag("Menu").GetComponent<HUDManager>():

        if (playerObj)
            inventory = playerObj.GetComponent<InventoryManager>();// get inventory component
    }

    // Update is called once per frame
    void Update()
    {
        // detect pickup and magnet
        if (playerObj)
        {
            playerDir = (playerObj.transform.position - transform.position).normalized;
            playerDist = Vector2.Distance(transform.position, playerObj.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDir, magnetRange, obstructMask);

            if (playerDist < magnetRange && !hit) 
                magnet = true;

            pickUp = (playerDist < pickUpRange);
        }

        // behaviour during basic State
        if (!magnet && !pickUp )
        {
            if (oscilate)
            {
                float oscill = Mathf.Sin(Time.time * oscilFreq) * oscilMag;
                transform.position += Vector3.up * oscill * Time.deltaTime;
            }
            
        }

        // apply magnet effect
        if (magnet && !pickUp)
        {
            speed = Mathf.Clamp(speed + (magnetSpeed / magnetAccel) * Time.deltaTime, 0, magnetSpeed); 
           
            transform.position = (Vector2)transform.position + (playerDir * speed * Time.deltaTime);
        }

        //pickup obj & apply different effect on Inventory manager
        if (pickUp && inventory) 
        {
            switch (collectype) 
            {
                case Collectype.MONEY:
                    {
                        inventory.money += value;
                        break;
                    }

                case Collectype.HEALTH:
                    {
                        inventory.health+= value;
                        hudManager.healActive = true;
                        break;
                    }

                case Collectype.MAXHEALTH:
                    {
                        inventory.maxHealth+= value;
                        break;
                    }

                case Collectype.ATTACKBOOST:
                    {
                        inventory.attackBoost+= value;
                        break;
                    }
            }

            Destroy(gameObject);
        }
           
    }
}
