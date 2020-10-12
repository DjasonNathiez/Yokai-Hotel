using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class KF_AssetLayerOrdering : MonoBehaviour
{
    private GameObject thisObject;
    private SpriteRenderer thisSP;

    private void Start()
    {
        thisObject = this.gameObject;
        thisSP = thisObject.GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            thisSP.sortingOrder = -1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            thisSP.sortingOrder = 1;
        }
    }
}
