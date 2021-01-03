using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_KeyDesactivate : MonoBehaviour
{
    public GameObject player;
    public InventoryManager inventoryM;
    public GameObject prepareEnd;
    public KF_ActivateUnlock actlock;


    // Start is called before the first frame update
    void Start()
    {
        inventoryM = player.GetComponent<InventoryManager>();
        prepareEnd = GameObject.FindGameObjectWithTag("PrepareEndRoom");
        actlock = prepareEnd.GetComponent<KF_ActivateUnlock>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if ((inventoryM.keys > 1) && (Input.GetButtonDown("Interact")))
            {
                inventoryM.keys = inventoryM.keys - 1;
                Debug.Log("Key Removed");
                this.gameObject.SetActive(false);
                actlock.keycount = actlock.keycount - 1;
            }
            if ((inventoryM.keys < 1) && (Input.GetButtonDown("Interact")))
            {
                Debug.Log("Not enough Keys");
            }
        }
    }
}
