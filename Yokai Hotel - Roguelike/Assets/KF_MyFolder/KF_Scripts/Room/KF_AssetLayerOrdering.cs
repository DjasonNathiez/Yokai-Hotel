using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class KF_AssetLayerOrdering : MonoBehaviour
{
    public GameObject thisObject;
    private SpriteRenderer thisSP;
    public bool manualCollider;

    private void Start()
    {
        if (manualCollider == false)
            thisObject = this.gameObject;
        else
            if (thisObject == null)
            thisObject = this.gameObject;

        thisSP = thisObject.GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ennemi"))
        {
            thisSP.sortingOrder = -1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ennemi"))
        {
            thisSP.sortingOrder = 1;
        }
    }
}
