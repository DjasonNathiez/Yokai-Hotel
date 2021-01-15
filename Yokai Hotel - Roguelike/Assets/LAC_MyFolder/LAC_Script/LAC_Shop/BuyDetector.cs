using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BuyDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public bool onTrigger, Buy;

    private void Update()
    {
        if (onTrigger)
            Buy = (Input.GetButtonDown("Interact")); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("enter");
        if (collision.tag == "Player")
            onTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit");
        if (collision.tag == "Player")
            onTrigger = false;
        
    }

}
