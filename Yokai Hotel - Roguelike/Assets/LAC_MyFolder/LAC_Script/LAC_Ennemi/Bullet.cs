using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    public float speed;
    public Vector2 dir;

    public int damage;

    public Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
       
    }

    private void FixedUpdate()
    {
        rb2D.velocity = dir.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // hurt player
        if (collision.CompareTag("PHurtBox"))
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if(player)
                player.hurtDamage = damage;

            Debug.Log("bulletHit");
        }

        // destroy on wall
        if (collision.gameObject.layer == LayerMask.NameToLayer("Block"))
            Destroy(gameObject);


    }


}
