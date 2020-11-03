using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(DropSystem))]

public class BasiqueEnnemiCac : MonoBehaviour
{
    Rigidbody2D rb2D;
    CircleCollider2D cC2D;
    [HideInInspector]
    public PlayerController player;
    DropSystem drop;
    public enum EnnemyState { IDLE, AGGRO, ATTACK, WAIT, HURT, DIE}
    public EnnemyState ennemyState;
    EnnemyState lastState;

    public int healthPoints;
    public int hitDamage = 0;

    public SpriteRenderer spriteT;
    public Vector2 repulseForce;
    public float inertness;
    [HideInInspector]
    public float inertnessModifier;
    public float recovery;

    public float speed;
    Vector2 velocitySmoothing;
    public Vector2 velocity;

    [Header("Idle")]
    public float idleRadius;

    public float minSkinWidth;
    public float maxSkinWidth;

    public bool detectX = true;
    public bool detectY = true;

    public float idleTime;
    float idleTimer = 0;

    public float idleCycle;
    float idleCycleTimer = 0;


    public Vector2 idleDir;
    Vector2 idlePoint;

    [Header("Chase")]
    public GameObject target;

    float targetDist;
    Vector2 targetDir;
    Vector2 targetPos;
    string targetTag;

    public float detectCycle;
    float detectTimer = 0;

    public float minDetectRadius;
    public float maxDetectRadius;

    public LayerMask obstructMask;
    public LayerMask wallMask;

    [Header("Attack")]
    public int attackDamage;

    public float minCacRadius;
    public float maxCacRadius;

    public float attackDuration;
    public float breakDuration;
    float attackTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cC2D = GetComponent<CircleCollider2D>();

        spriteT = GetComponentInChildren<SpriteRenderer>();

        target = GameObject.FindGameObjectWithTag("Player");
        player = target.GetComponent<PlayerController>();

        drop = GetComponent<DropSystem>();
        ennemyState = lastState = EnnemyState.IDLE;
        // initialize target
        if (target != null)
        {
            targetTag = target.tag;
            targetPos = target.transform.position;
        }
        else
        {
            targetTag = null;
            targetPos = (Vector2)transform.position+Vector2.one;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetPos();

        targetDist = Vector2.Distance(targetPos, transform.position);
        targetDir = ((Vector3)targetPos - transform.position).normalized;

        bool see = SeeEntities(maxDetectRadius, targetDir, obstructMask);

        bool checkXMin = SeeEntities(cC2D.radius + minSkinWidth, Vector2.right * Mathf.Sign(targetDir.x), obstructMask);
        bool checkXMax = SeeEntities(cC2D.radius + maxSkinWidth, Vector2.right * Mathf.Sign(targetDir.x), obstructMask);

        bool checkYMin = SeeEntities(cC2D.radius + minSkinWidth, Vector2.up * Mathf.Sign(targetDir.y), obstructMask);
        bool checkYMax = SeeEntities(cC2D.radius + maxSkinWidth, Vector2.up * Mathf.Sign(targetDir.y), obstructMask);

        if (!checkYMax)
            detectY = true;

        if (!checkXMax)
            detectX = true;

        if(hitDamage != 0)
        {
            healthPoints -= hitDamage;
            hitDamage = 0;
        }

        Debug.DrawRay(transform.position, targetDir * 5, Color.white);
        Debug.DrawRay(transform.position,Vector2.right * (Mathf.Sign(targetDir.x) * cC2D.radius), Color.blue);
        Debug.DrawRay(transform.position, Vector2.up * (Mathf.Sign(targetDir.y) * cC2D.radius), Color.blue);

        // hurt condition
        if (repulseForce != Vector2.zero)
        {
            if (ennemyState != EnnemyState.HURT)
                lastState = ennemyState;

            ennemyState = EnnemyState.HURT;

            velocity = repulseForce;
            repulseForce = Vector2.zero;
        }
        if(ennemyState != EnnemyState.HURT)
            spriteT.color = Color.white;

        switch (ennemyState)
        {
            case EnnemyState.IDLE :
                {

                    if ( see && targetDist < minDetectRadius)
                    {
                        velocity = Vector2.zero;
                        ennemyState = EnnemyState.AGGRO;
                    }   
                    else
                        Wandering();

                    break;
                }
            case EnnemyState.AGGRO:
                {
                    
                    velocity = targetDir * speed;

                    if (targetDist > maxDetectRadius || !see)
                        ennemyState = EnnemyState.IDLE;

                    if ( targetDist <= minCacRadius)
                        ennemyState = EnnemyState.ATTACK;

                    break;
                }
            case EnnemyState.ATTACK:
                {
                    velocity = Vector2.zero;
                    attackTimer += Time.deltaTime;

                    if((attackTimer >= attackDuration)|| (attackTimer <= breakDuration && targetDist > maxCacRadius))
                    {
                        attackTimer = 0;
                        ennemyState = EnnemyState.AGGRO;
                    }

                    break;
                }
            case EnnemyState.HURT:
                {
                    velocity.x = Mathf.SmoothDamp(velocity.x, 0, ref velocitySmoothing.x, inertness * inertnessModifier);
                    velocity.y = Mathf.SmoothDamp(velocity.y, 0, ref velocitySmoothing.y, inertness * inertnessModifier);

                    if (velocity.magnitude < recovery)
                    {
                        if (healthPoints <= 0)
                        {
                            drop.SortItemPos(transform, transform.position, drop.dropRadius, obstructMask);
                            healthPoints = 3;
                            //Destroy(gameObject);
                        }


                        velocity = Vector2.zero;
                        inertnessModifier = 1;

                        

                        ennemyState = lastState;
                    }

                    Color col = Color.white;
                    col.a = Mathf.Sin(Time.time*30) * 255;
                    spriteT.color = col;

                    break;
                }
        }
    }

    private void FixedUpdate()
    {
        rb2D.velocity = velocity;
    }

    public void Wandering()
    {
        
        idleCycleTimer += Time.deltaTime;

        if(idleCycleTimer >= idleCycle)
        {
            idleCycleTimer = 0;

            idleDir = new Vector2((Random.value-0.5f) + targetDir.x, (Random.value - 0.5f) + targetDir.y); // move random dir
            idlePoint = (Vector2)transform.position + idleDir * idleRadius;
        }



        //if (Vector2.Distance(transform.position, idlePoint) > 0.5f)
        if (idleTimer < idleTime)
        {
            float detectLength = cC2D.radius*1.2f;
            Vector2 currentDir = new Vector2((SeeEntities(detectLength, Vector2.right * idleDir.x, obstructMask)) ? idleDir.x : -idleDir.x,
                                             (SeeEntities(detectLength, Vector2.up * idleDir.y, obstructMask)) ? idleDir.y : -idleDir.y);
          
            velocity = currentDir * speed;
            idleDir = currentDir;

            idleTimer += Time.deltaTime;
        }

        else
        {
            idleTimer = 0;
            velocity = Vector2.zero;
        }
            

    }

   

    public bool SeeEntities(float radius, Vector2 dir ,LayerMask obstructMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, radius, obstructMask);
        return (!hit);
    }
    public void UpdateTargetPos()
    {
        detectTimer += Time.deltaTime;

        if (detectTimer >= detectCycle)
        {
            detectTimer = 0;
            targetPos = target.transform.position;
        }
    }

    


}
