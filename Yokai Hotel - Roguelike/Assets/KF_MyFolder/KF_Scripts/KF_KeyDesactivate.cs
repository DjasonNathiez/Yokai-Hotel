using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_KeyDesactivate : MonoBehaviour
{
    public InventoryManager inventoryM;
    public GameObject prepareEnd;
    public KF_ActivateUnlock actlock;


    // Start is called before the first frame update
    void Start()
    {
        prepareEnd = GameObject.FindGameObjectWithTag("PrepareEndRoom");
        actlock = prepareEnd.GetComponent<KF_ActivateUnlock>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!inventoryM)
                inventoryM = collision.gameObject.GetComponent<InventoryManager>();

            if ((inventoryM.keys >= 1) && (Input.GetButtonDown("Interact")))
            {
                inventoryM.keys = inventoryM.keys - 1;
                Debug.Log("Key Removed");
                this.gameObject.SetActive(false);
                actlock.keycount = actlock.keycount - 1;
            }
            else 
                if ((inventoryM.keys == 0) && (Input.GetButtonDown("Interact")))
                {
                    Debug.Log("Not enough Keys");
                }
        }
    }
}
