using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class BasiqueEnnemiCac : MonoBehaviour
{
    Rigidbody2D rb2D;
    public enum EnnemyState { IDLE, AGGRO, ATTACK, WAIT, HURT, DIE}
    public EnnemyState ennemyState;

    public int healthPoints;

    public float speed;
    Vector2 velocity;

    [Header("Idle")]
    public float idleCycle;
    public float idleRadius;
    float idleTimer = 0;
    Vector2 idleDir;
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

    [Header("Attack")]
    public float minCacRadius;
    public float maxCacRadius;

    public float attackDuration;
    public float breakDuration;
    float attackTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        ennemyState = EnnemyState.IDLE;
        // initialize target
        targetTag = target.tag;
        targetPos = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetPos();

        targetDist = Vector2.Distance(targetPos, transform.position);
        targetDir = ((Vector3)targetPos - transform.position).normalized;

        bool see = SeeEntities(maxDetectRadius, obstructMask);
        Debug.DrawRay(transform.position, targetDir * 5, Color.white);
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
        }
    }

    private void FixedUpdate()
    {
        rb2D.velocity = velocity;
    }

    public void Wandering()
    {
        idleTimer += Time.deltaTime;
        if(idleTimer >= idleCycle)
        {
            idleTimer = 0;

            idleDir = new Vector2((Random.value-0.5f) + targetDir.x, (Random.value - 0.5f) + targetDir.y);
            idlePoint = (Vector2)transform.position + idleDir * idleRadius;
        }

        if (Vector2.Distance(transform.position, idlePoint) > 0.5f)
            velocity = idleDir * speed;

        else
            velocity = Vector2.zero;

    }

    public bool SeeEntities(float radius, LayerMask obstructMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir, radius, obstructMask);
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
