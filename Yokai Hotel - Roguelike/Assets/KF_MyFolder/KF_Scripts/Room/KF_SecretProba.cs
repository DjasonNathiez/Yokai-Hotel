using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_SecretProba : MonoBehaviour
{
    public GameObject secret;
    public int probability;
    private bool runOnce;
    private int proba;

    private void Start()
    {
        secret.SetActive(false);

        proba = Random.Range(0, probability);    // 1 in a [probability] chance to spawn.
        if (proba == 0)
        {
            Debug.Log("Secret Actif");
            secret.SetActive(true);
        }

        
        
    }
}
