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
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // hurt player
        if (collision.CompareTag("PHurtBox") && CompareTag("BulletEnemy"))
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if (player)
            {
                player.hurtDamage = damage;
            }

            Debug.Log("bulletHit");
        }

        //hurt ennemy
        if(collision.CompareTag("Ennemi") && CompareTag("BulletAlly"))
        {
            BasiqueEnnemiCac ennemiCac = collision.GetComponentInParent<BasiqueEnnemiCac>();
            EnnemiSlime ennemiDist = collision.GetComponentInParent<EnnemiSlime>();

            if(ennemiCac)
            {
                ennemiCac.hitDamage = damage;
                Debug.Log("EnnemyCacHit");
            }
            if (ennemiDist)
            {
                ennemiDist.healthDamage = damage;
                Debug.Log("EnnemyDistHit");
            }

            Debug.Log("EnnemyHit");
        }

        // destroy on wall
        if (collision.gameObject.layer == LayerMask.NameToLayer("Block"))
            Destroy(gameObject);


    }


}
