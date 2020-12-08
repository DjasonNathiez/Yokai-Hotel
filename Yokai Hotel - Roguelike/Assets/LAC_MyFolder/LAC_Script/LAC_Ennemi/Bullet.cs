using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    public float speed;
    public Vector2 dir;
    
    public int damage;
    
    public bool bulletAlly;
    public bool bulletEnnemy;

    public Rigidbody2D rb2D;
    public LayerMask blockMask;

    public Collider2D hitBox;

    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
    }

    private void Update()
    {
        #region SwitchState
        if (CompareTag("BulletAlly"))
        {
            bulletAlly = true;
            bulletEnnemy = false;
        }

        if (CompareTag("BulletEnemy"))
        {
            bulletAlly = false;
            bulletEnnemy = true;
        }

        #endregion

    }

    private void FixedUpdate()
    {
        rb2D.velocity = dir.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitbox)
    {
        // hurt player
        if (hitbox.CompareTag("PHurtBox") && (tag == "BulletEnemy"))
        {
            PlayerController player = hitbox.GetComponentInParent<PlayerController>();
            if (player)
            {
                player.hurtDamage = damage;
                Destroy(gameObject);
            }

            Debug.Log("bulletHit");
        }

        //hurt ennemy
        if (tag == "BulletAlly")
        {
            if (hitbox.CompareTag("Ennemi"))
            {
                EnnemiBehaviour ennemi = hitbox.GetComponentInParent<EnnemiBehaviour>();
                if (ennemi)
                {
                    ennemi.healthDamage = damage;
                    Debug.Log("EnnemyDistHit");
                    Destroy(gameObject);
                }
            }

            if (hitbox.CompareTag("Shield"))
            {
                
                EnnemiShield shield = hitbox.GetComponent<EnnemiShield>();
                if (shield)
                {
                    dir = shield.shieldDir.normalized;
                    tag = "BulletEnemy";
                }
               
            }
        }

        // destroy on wall
        if (hitbox.gameObject.layer == blockMask)
            Destroy(gameObject);


    }

}
