using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_KeyDesactivate : MonoBehaviour
{
    public InventoryManager inventoryM;
    public GameObject prepareEnd;
    public KF_ActivateUnlock actlock;
    public GameObject interactb;
    private bool bActif;
    public Animator anim;

    public AudioManager audioM;


    // Start is called before the first frame update
    void Start()
    {
        audioM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
        prepareEnd = GameObject.FindGameObjectWithTag("PrepareEndRoom");
        actlock = prepareEnd.GetComponent<KF_ActivateUnlock>();
        anim = this.gameObject.GetComponent<Animator>();
        interactb.SetActive(false);
        bActif = true;

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player")) && (bActif == true))
        {
            if (!inventoryM)
                inventoryM = collision.gameObject.GetComponent<InventoryManager>();

            interactb.SetActive(true);
            if ((inventoryM.keys >= 1) && (Input.GetButtonDown("Interact")))
            {
                inventoryM.keys = inventoryM.keys - 1;
                Debug.Log("Key Removed");
                anim.SetBool("keyRemoved", true);
                bActif = false;
                Destroy(interactb);
                actlock.keycount = actlock.keycount - 1;
            }
            else 
                if ((inventoryM.keys == 0) && (Input.GetButtonDown("Interact")))
                {
                    Debug.Log("Not enough Keys");
                }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
            interactb.SetActive(false);
    }

    public void ActivateLanternSound()
    {
        if (audioM)
            audioM.PlaySound("Active Keys", 0);
    }
    public void FireSound()
    {
        if (audioM)
            audioM.PlaySound("Fire", 0);
    }
}
