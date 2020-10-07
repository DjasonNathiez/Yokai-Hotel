using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSystemObject : MonoBehaviour
{
    public int goldValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            StartCoroutine(WaitForDestroy());
        }
    }


    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(1/2);
        Destroy(gameObject);
    }
}
