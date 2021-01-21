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
    float randomOscil;
    public GameObject playerObj;
    InventoryManager inventory;
    PlayerController player;

    GameObject gameManager;
    AudioManager audioManager;

    Vector2 playerDir;
    public float playerDist;

    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
            player = playerObj.GetComponent<PlayerController>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        

        if (playerObj)
            inventory = playerObj.GetComponent<InventoryManager>();// get inventory component

        if (gameManager)
            audioManager = gameManager.GetComponent<AudioManager>(); //get audioManager component

        randomOscil = Random.value - 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // detect pickup and magnet
        //Debug.DrawRay(transform.position, playerDir * playerDist, Color.white);
        if (playerObj)
        {
            playerDir = (playerObj.transform.position - transform.position).normalized;
            playerDist = Vector2.Distance(transform.position, playerObj.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDir, playerDist, obstructMask);

            if (playerDist < magnetRange && !hit) 
                magnet = true;

            pickUp = (playerDist < pickUpRange);
        }

        // behaviour during basic State
        if (!magnet && !pickUp )
        {
            if (oscilate)
            {
                float oscill = Mathf.Sin(Time.time * oscilFreq + randomOscil) * oscilMag ;
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
        if (pickUp ) 
        {
            if (inventory)
            {
                switch (collectype)
                {
                    case Collectype.MONEY:
                        {

                            inventory.money += value;

                            if(audioManager)
                                audioManager.PlaySound("Own gold", 0);
                            break;
                        }

                    case Collectype.HEALTH:
                        {

                            if (player)
                                player.ChangeHealth(value);

                            if(audioManager)
                                audioManager.PlaySound("Player healing", 0);
                            break;
                        }

                    case Collectype.MAXHEALTH:
                        {
                            inventory.maxHealth += value;
                            break;
                        }

                    case Collectype.ATTACKBOOST:
                        {
                            inventory.attackBoost += value;
                            break;
                        }
                }
            }
            

            Destroy(gameObject);
        }
           
    }
}
