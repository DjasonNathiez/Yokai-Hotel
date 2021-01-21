using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BossBullet : MonoBehaviour
{
    bool active;
    public float speed;
    public Vector2 velocity;
    bool moving;
    public bool haveBounce;
    public int damage;

    public Rigidbody2D rb2D;
    public LayerMask blockMask;

    //public Collider2D hitBox;
    public CircleCollider2D cC2D;
    SpriteRenderer spriteR;

    PlayerController player;
    public GameObject explosionVFX;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cC2D = GetComponent<CircleCollider2D>();
        spriteR = GetComponent<SpriteRenderer>();
        SetActiveBullet(false);
    }

    private void Update()
    {
        // detectPlayer
        Collider2D hit = Physics2D.OverlapCircle(transform.position, cC2D.radius, blockMask);
        if (hit)
        {
            if(player == null)
                player = hit.GetComponentInParent<PlayerController>();

            if (player)
            {
                

                if (active)
                {
                    player.hurtDamage = damage;
                    Debug.Log("BossbulletHit");
                    GameObject particule = Instantiate(explosionVFX, transform.position, transform.rotation);
                    Destroy(particule, 2);
                }
                
            }
           
            SetActiveBullet(false);
            Debug.Log("BossBullet hit Player");
        }
           
    }

    private void FixedUpdate()
    {
        rb2D.velocity = velocity.normalized * speed;

    }
    public void MoveProjectile(Vector2 dir, float distance, float time)
    {
        if (!moving)
        {
            velocity = dir;
            speed = (distance / time);
            StartCoroutine(EndMove(time));
            moving = true;
        }
        
    }

    public IEnumerator EndMove(float delay)
    {
        yield return new WaitForSeconds(delay);
        velocity = Vector2.zero;
        speed = 0;
        GameObject particule = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(particule, 2);
        moving = false;
        haveBounce = !haveBounce;

    }
    /*private void OnTriggerEnter2D(Collider2D collision) 
    {
        // hurt player
        if (collision.tag == "PHurtBox")
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if (player)
            {
                player.hurtDamage = damage;
                Debug.Log("BossbulletHit");
            }

            //SetInactive();
        }

    }*/

    public void SetActiveBullet( bool state)
    {
        //trigger some particule effect

        if (!state)
        {
            active = false;
            cC2D.enabled = false;
            Color color = spriteR.color;
            color.a = 0;

            spriteR.color = color;
        }
        if (state)
        {
            active = true;
            cC2D.enabled = true;
            Color color = spriteR.color;
            color.a = 255;
            spriteR.color = color;
        }
 
    }
}
