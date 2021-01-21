using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KF_ShopInteract : MonoBehaviour
{
    public KF_ShopDialogue objectToBuy;
    public UnityEvent itemDescAppear;
    public UnityEvent itemBuy;
    public bool showState, lastShowState;
    public bool inRange;
    public bool objectBought; // this bools is true when object is bought

    private GameObject thisObjectInteract;
    private KF_ShopInteractManager shopIntM;
    private GameObject thisItem;
    private bool inDialogue;
    private bool onlyOnce;

    // Start is called before the first frame update
    void Start()
    {
        objectBought = false;
        shopIntM = FindObjectOfType<KF_ShopInteractManager>();
        foreach (GameObject objects in shopIntM.objectsInLevel)
        {
            if (objects.Equals(this.gameObject))
                thisItem = objects;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if ((inRange == true) && (objectBought == false))
        {
            //if (onlyOnce == false)
                //ShowItemDesc();
            if (Input.GetButtonDown("Interact"))
            {
                objectBought = true;
                Debug.Log("Item Purchased");
                //RemoveDesc();
            }
            onlyOnce = true;
        }
        if (inDialogue == true)
        {
            //RemoveDesc();
            inDialogue = false;
            onlyOnce = false;
        }*/
        if (showState != lastShowState)
        {
            if(showState)
                ShowItemDesc();
            else
                RemoveDesc();

            lastShowState = showState;
        }
           
    }

    public void ShowItemDesc()
    {
        shopIntM.StartShopInteract(objectToBuy);
    }

    public void RemoveDesc()
    {
        inRange = false;
        shopIntM.WalkedAway();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
            foreach (GameObject objects in shopIntM.objectsInLevel)
            {
                if (objects.Equals(this.gameObject))
                    continue;
                objects.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inDialogue = true;
            onlyOnce = false;
            foreach (GameObject objects in shopIntM.objectsInLevel)
            {
                objects.SetActive(true);
            }

        }
    }
}
