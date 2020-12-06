using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]  
public class EnnemiShield : MonoBehaviour
{
    [HideInInspector]
    public GameObject objShielded;
    public int shieldPoint;


    // Update is called once per frame
    void Update()
    {
        if (shieldPoint <= 0)
            Destroy(gameObject);
    }
}
