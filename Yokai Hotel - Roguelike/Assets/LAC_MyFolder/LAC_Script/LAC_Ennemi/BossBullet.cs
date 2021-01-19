using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BossBullet : MonoBehaviour
{
    public float speed;
    public Vector2 velocity;
    bool moving;
    public bool haveBounce;
    public int damage;

    public Rigidbody2D rb2D;
    public LayerMask blockMask;

    public Collider2D hitBox;
    CircleCollider2D cC2D;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cC2D = GetComponent<CircleCollider2D>();
        MoveProjectile(Vector2.right, 5, 2);
    }

    private void Update()
    {



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
        moving = false;
        haveBounce = !haveBounce;

    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // hurt player
        if (collision.CompareTag("PHurtBox"))
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if (player)
            {
                player.hurtDamage = damage;
                
            }

            Debug.Log("bulletHit");
            SetInactive();
        }



        Collider2D hit = Physics2D.OverlapCircle(transform.position, cC2D.radius, blockMask);
        if (hit)
            SetInactive();
    }

    public void SetInactive()
    {
        //trigger some particule effect
        
        this.gameObject.SetActive(false);
        
    }
}
