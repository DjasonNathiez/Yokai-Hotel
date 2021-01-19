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
    private bool inRange;

    AudioManager audioM;


    // Start is called before the first frame update
    void Start()
    {
       
            audioM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();

        thisRoom = this.transform.root.GetComponent<Room>();
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
        inventoryM = FindObjectOfType<InventoryManager>();
        
    }

    private void Update()
    {
        if (inRange == true)
        {

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



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
        {
            interactb.SetActive(true);
            inRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
        {
            interactb.SetActive(false);
            inRange = false;
        }
            
    }

    public void KeysSound()
    {
        if(audioM)
        audioM.PlaySound("Keys", 0);
    }
}
