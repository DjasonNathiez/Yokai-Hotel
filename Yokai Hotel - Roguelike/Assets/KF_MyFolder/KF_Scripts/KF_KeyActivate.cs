using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_KeyActivate : MonoBehaviour
{
    public InventoryManager inventoryM;
    public Room thisRoom;
    public int keysinroom;
    public GameObject interactb;
    private bool bActif;
    private Animator anim;


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
        interactb.SetActive(false);
        bActif = true;
        anim = this.gameObject.GetComponent<Animator>();
        thisRoom = transform.root.GetComponent<Room>();
    }



    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
        {
            interactb.SetActive(true);
            if (!inventoryM)
                inventoryM = collision.gameObject.GetComponent<InventoryManager>();

            if (Input.GetButtonDown("Interact"))
            {
                inventoryM.keys++;
                Debug.Log("Key Added");
                anim.SetBool("gotKey", true);
                Destroy(interactb);
                bActif = false;  
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
            interactb.SetActive(false);
    }
}
