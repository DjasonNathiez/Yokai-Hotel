using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]  
public class EnnemiShield : MonoBehaviour
{
    [HideInInspector]
    public GameObject objShielded;
    public float shieldPoint;
    public Vector2 shieldDir;


    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(shieldDir.y, shieldDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        if (shieldPoint <= 0)
            Destroy(gameObject);
    }
}
