using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{

    public float speed;
    public Vector2 dir;
    
    public int damage;
    
    public bool bulletAlly;
    public bool bulletEnnemy;

    public Rigidbody2D rb2D;
    public LayerMask blockMask;

    //public Collider2D hitBox;
    CircleCollider2D cC2D;
    PlayerController player;

    public GameObject visual;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cC2D = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // TES GARDES FOUS !!!
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

        if (visual)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }
    }

    private void FixedUpdate()
    {
        rb2D.velocity = dir.normalized * speed;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // hurt player
        if (collision.CompareTag("PHurtBox") && (tag == "BulletEnemy"))
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
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
            if (collision.CompareTag("Ennemi"))
            {
                EnnemiBehaviour ennemi = collision.GetComponentInParent<EnnemiBehaviour>();
                if (ennemi)
                {
                    ennemi.healthDamage = damage;
                    Debug.Log("EnnemyDistHit");
                    Destroy(gameObject);
                }
            }
            if (collision.CompareTag("Shield"))
            {
                EnnemiShield shield = collision.GetComponent<EnnemiShield>();
                if (shield)
                {
                    dir = shield.shieldDir.normalized;
                    tag = "BulletEnemy";
                }
            }
        }

        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.2f, blockMask);
        if (hit)
            GameObject.Destroy(gameObject);
    }

    

}
