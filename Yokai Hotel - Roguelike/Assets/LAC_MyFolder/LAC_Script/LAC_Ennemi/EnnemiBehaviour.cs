using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(DropSystem))]
public class EnnemiBehaviour : MonoBehaviour
{
    // basic stuff
    public Rigidbody2D rb2D;

    public CircleCollider2D cC2D;

    public SpriteRenderer spriteT;

    public PlayerController player;
    DropSystem drop;

    #region state system
    public enum EnnemyState { IDLE, AGGRO, ATTACK, WAIT, STUN, DIE }
    public EnnemyState ennemyState;
    EnnemyState lastState;
    #endregion
    #region health
    [Header("health")]
    public float healthPoints;
    public float healthDamage = 0;
    public ParticleSystem hurtParticule;
    public GameObject deathParticule;
    bool deathTrigger;
    [HideInInspector]
    public bool hurt;
    public BoxCollider2D hurtBox;
    #endregion
    #region recoil
    [Header("recoil")]
    public int stunPoints;
    public float stunMultiplier = 1, stunTime;
    public float constantStunTime;
  

    public Vector2 repulseForce;
    public float inertness, stunInertness;


    public float inertnessModifier = 1;
    public float recovery;
    #endregion
    #region move
    [Header("move")]
    public float speed;
    Vector2 velocitySmoothing;
    [HideInInspector]
    public Vector2 velocity;
    #endregion
    #region target
    public LayerMask obstructMask;
    [HideInInspector]
    public GameObject target;
    public Vector2 targetAdjustement = new Vector2(0,0f);

    [HideInInspector]
    public float targetDist;
    [HideInInspector]
    public Vector2 targetDir;
    [HideInInspector]
    public bool targetVisible;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cC2D = GetComponent<CircleCollider2D>();

        drop = GetComponent<DropSystem>();

        spriteT = GetComponentInChildren<SpriteRenderer>();
 
    }
    public virtual void Start()
    {

        target = GameObject.FindGameObjectWithTag("Player");
        player = target.GetComponent<PlayerController>();

        if (hurtParticule == null)
            hurtParticule = GetComponentInChildren<ParticleSystem>();

    }

    // Update is called once per frame
    public virtual void Update()
    {
        // Update target position
        Vector2 targetPos =(Vector2)player.transform.position + targetAdjustement;
        targetDist = Vector2.Distance(targetPos, transform.position);
        targetDir = ((Vector3)targetPos - transform.position).normalized;

        targetVisible = !DetectBlock(targetDist, targetDir, obstructMask);


        // hurt part 
        if (healthDamage != 0)
        {
            healthPoints -= healthDamage;
            healthDamage = 0;

            if (ennemyState != EnnemyState.STUN)
                lastState = ennemyState;

            ennemyState = EnnemyState.STUN;

            velocity = repulseForce;
            repulseForce = Vector2.zero;

            if (hurtParticule != null)
                hurtParticule.Play();
        }


        if (ennemyState != EnnemyState.STUN)
            spriteT.color = Color.white;

        // state machine
        switch (ennemyState)
        {

            case EnnemyState.STUN:
                {
                    SetVelocity(Vector2.zero, inertness * inertnessModifier);

                    if (velocity.magnitude <= recovery)
                    {
                        StartCoroutine(StunRecovery(stunTime + constantStunTime));
                        Debug.Log("apply stun");
                        velocity = Vector2.zero;
                        inertnessModifier = 1;
                    }

                    // show hurt
                    Color col = Color.white;
                    col.a = Mathf.Sin(Time.time * 30) * 255;
                    spriteT.color = col;

                    break;
                }
            
        }
        if(ennemyState != EnnemyState.STUN)
        {
            spriteT.color = Color.white;
        }
      
        if (healthPoints <= 0)
        {
            if (!deathTrigger)
            {
                drop.SortItemPos(transform, transform.position, drop.dropRadius, obstructMask);
                Debug.Log("DropItem");

                deathTrigger = true;
            }
            if (hurtBox != null)
                hurtBox.enabled = false;
            
            ennemyState = EnnemyState.DIE;
            
            //healthPoints = 3;
        }
    }
    private void FixedUpdate()
    {
        rb2D.velocity = velocity;
    }
    public void SetVelocity( Vector2 targetVelocity, float accleration )
    {
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity.x, ref velocitySmoothing.x, accleration);
        velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocity.y, ref velocitySmoothing.y, accleration);
    }

    public bool DetectBlock(float length, Vector2 dir, LayerMask obstructMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, length, obstructMask);
        //Debug.Log("Detection Use");
        return (hit);
    }

    public IEnumerator StunRecovery(float delay)
    {
        yield return new WaitForSeconds(delay);
        stunTime = 0;
        ennemyState = EnnemyState.IDLE;
    }

    public void Death()
    {

        Destroy(gameObject);
    }

    public void DeathVFXorigin()
    {
        if (deathParticule != null)
        {
            GameObject particule = Instantiate(deathParticule, transform.position, transform.rotation);
            Destroy(particule, 5);
        }
    }

}
