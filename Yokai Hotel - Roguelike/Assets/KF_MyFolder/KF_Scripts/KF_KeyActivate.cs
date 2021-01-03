using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_KeyActivate : MonoBehaviour
{
    public GameObject player;
    public InventoryManager inventoryM;
    public Room thisRoom;
    public int keysinroom;


    // Start is called before the first frame update
    void Start()
    {
       
        if (thisRoom.keyToSpawn == true)
        {
            this.gameObject.SetActive(true);
            keysinroom = 1;
        }
        else
        {
            this.gameObject.SetActive(false);
            keysinroom = 0;
        }
            
    }



    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!inventoryM)
                inventoryM = collision.gameObject.GetComponent<InventoryManager>();

            if (Input.GetButtonDown("Interact"))
            {
                inventoryM.keys ++;
                Debug.Log("Key Added");
                //this.gameObject.SetActive(false);
            }
        }
    }
}
