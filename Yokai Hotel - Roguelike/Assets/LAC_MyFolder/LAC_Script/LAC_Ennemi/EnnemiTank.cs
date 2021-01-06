using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiTank : EnnemiBehaviour
{
    [Header("Idle")]

    public Vector2 moveDir;
    Vector2 enemyTargetPos, enemyTargetLastPos;
    Vector2 followDir, enemyDir;

    public LayerMask detectMask;

    public List<Collider2D> enemyList = new List<Collider2D>();
    public GameObject nearestEnemy;
    float nearestEnemyDist = Mathf.Infinity;

    float detectTimer, detectFreq = 1f;
    float timeBetweenPos = 0.5f;
    bool updatingPos = true;

    [Header("Chase")]
    public float timeToAttack;
    float loadAttackTimer;

    [Header("Attack")]
    public int attackDamage;
    public float attackSpeed;
    public float attackDuration;

    public float minCacRadius;
    public float maxCacRadius;

    Vector2 attackDir;

    [Header("Shield")]
    public GameObject shieldPrefab;
    public GameObject shield;

    public float distToShield;
    public float approxToShield;

    public GameObject bulletToFocus;
    public LayerMask bulletMask;
    public float bulletDelay;
    float bulletTimer;

    public float shieldPoint;
    public float shieldRadius, shieldSpeed, shieldAngle, defelectDegree;
    public Vector2 shieldOrigin;
    Vector2 shieldDir;
    

    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        moveDir = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized;

        // initialize shield
        Vector2 instantiatePos = ( Vector2)transform.position + shieldOrigin + new Vector2(0, shieldRadius);
        shield = Instantiate(shieldPrefab,instantiatePos , transform.rotation);


        EnnemiShield shieldEffect = shield.GetComponent<EnnemiShield>();
        if (shieldEffect)
            shieldEffect.objShielded = gameObject;

        shield.transform.SetParent(transform);
        shieldAngle = Mathf.Abs(Vector2.Angle(Vector2.right, shieldDir)) % 360;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        #region detect enemy
        // Update nearest Enemy Object
        if (detectTimer >= detectFreq)
        {
            EnemyList(ref enemyList,transform.position, 6f, detectMask);
            ClosestEnemy(transform.position, enemyList, ref nearestEnemyDist);

            detectTimer = 0;
        }
        else
            detectTimer += Time.deltaTime;
        // Update pos & last pos of enemy
        if (updatingPos)
            StartCoroutine(UpdateEnemyTargetPos(timeBetweenPos));
        #endregion

        switch (ennemyState)
        {
            case EnnemyState.IDLE:
                {
                    // idle move
                    UpdateMoveDir(nearestEnemy, ref moveDir, 2f, 5f);
                    SetVelocity(moveDir * speed, 0.1f);

                    // shield player or bullet
                    if (shield)
                    {
                        DetectBullet(transform.position,distToShield,bulletMask);
                        if(bulletTimer <= 0)
                        {
                            if (targetDist <= distToShield)
                                shieldAngle = (Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg) % 360;
                            else if (targetDist > distToShield + approxToShield)
                                RotateShield(ref shield, shieldSpeed);
                        }
                        bulletTimer -= Time.deltaTime;

                        if (bulletToFocus != null)
                        {
                            bulletTimer = bulletDelay;
                            Vector2 dir = bulletToFocus.transform.position - (transform.position + (Vector3)shieldOrigin);
                            shieldAngle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
                        }   
                    }
                    
                    // aggro cond
                    if (targetDist < minCacRadius && targetVisible)
                        ennemyState = EnnemyState.AGGRO;
                    break;
                }
            case EnnemyState.AGGRO:
                {
                    // aggro
                    SetVelocity(Vector2.zero, 0.2f);
                    loadAttackTimer += Time.deltaTime;

                    //idle cond
                    if (targetDist > maxCacRadius)
                    {
                        ennemyState = EnnemyState.IDLE;
                        loadAttackTimer = 0;
                    }

                    // attack cond
                    if(loadAttackTimer >= timeToAttack)
                    {
                        ennemyState = EnnemyState.ATTACK;
                        attackDir = targetDir.normalized;

                        moveDir = attackDir;

                        StartCoroutine(AttackDuration());
                        loadAttackTimer = 0;
                    }

                    break;
                }
            case EnnemyState.ATTACK:
                {
                    SetVelocity(attackDir *attackSpeed, 0.2f);
                    break;
                }
        }

        // shield
        UpdateShieldPos();
        //shield.transform.position = transform.position
    }


    // Move
    public void EnemyList( ref List<Collider2D> enemyList, Vector2 origin, float detectRadius, LayerMask detectMask)
    {
        Collider2D[] enemyArray = Physics2D.OverlapCircleAll(origin, detectRadius, detectMask);

        enemyList.Clear();
        foreach (Collider2D e in enemyArray)
        {
           //float dist = Vector2.Distance(e.transform.position, transform.position);
           //Vector2 dir = (e.transform.position - transform.position).normalized;

            Transform parent = e.transform.parent;
            if (parent? parent.tag != tag : e.tag != tag)
                enemyList.Add(e);
        }
    }
    public void ClosestEnemy( Vector2 origin, List<Collider2D> enemyList, ref float nearestDist)
    {
        if (enemyList.Count == 0)
        {
            nearestDist = Mathf.Infinity;
            nearestEnemy = null;
        }
        else
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if(enemyList[i] != null && enemyList[i].transform.parent != transform && enemyList[i] != cC2D)
                {
                    float dist = Vector2.Distance(enemyList[i].transform.position, origin);
                    if(dist < nearestDist)
                    {
                        nearestEnemy = enemyList[i].gameObject;
                        nearestDist = dist;
                    }
                }
            }
        }
        
    }

    public void UpdateMoveDir(GameObject nearestEnemy, ref Vector2 moveDir, float minBorn, float maxBorn)
    {
        if (nearestEnemy )
        {
            if (enemyTargetLastPos != enemyTargetPos)
                followDir = (enemyTargetPos - enemyTargetLastPos).normalized;

           enemyDir = (nearestEnemy.transform.position - transform.position).normalized;

            float bornDist = (maxBorn - minBorn);
            float currentDist = Vector2.Distance(transform.position,nearestEnemy.transform.position) - minBorn;

            float tDir = Mathf.Clamp(currentDist / bornDist, 0, 1);
            moveDir = (enemyDir * tDir + (1 - tDir) * followDir).normalized;
        }
        else
        {
            float detectLength = cC2D.radius +0.1f;

            Vector2 currentDir = new Vector2((DetectBlock(detectLength, Vector2.right * moveDir.x, obstructMask)) ? -moveDir.x : moveDir.x,
                                             (DetectBlock(detectLength, Vector2.up * moveDir.y, obstructMask)) ? -moveDir.y : moveDir.y);
            SetVelocity(currentDir * speed, 0.1f);
            moveDir = currentDir;
        }
            

    }
    IEnumerator UpdateEnemyTargetPos( float delay)
    {
        updatingPos = false;
        yield return new WaitForSeconds(delay);
        if (nearestEnemy)
        {
            enemyTargetLastPos = enemyTargetPos;
            enemyTargetPos = nearestEnemy.transform.position;
        }
        updatingPos = true;
    }
    // shield
    public void UpdateShieldPos()
    {
        if(shield != null)
        {
            shieldDir = new Vector2(Mathf.Cos(shieldAngle * Mathf.Deg2Rad), Mathf.Sin(shieldAngle * Mathf.Deg2Rad)).normalized;
            shield.transform.position = transform.position + (Vector3)shieldOrigin + (Vector3)shieldDir * shieldRadius;

            EnnemiShield shieldEffect = shield.GetComponent<EnnemiShield>();
            Vector2 testDir = new Vector2(Mathf.Cos((shieldAngle * Mathf.Deg2Rad)), Mathf.Sin((shieldAngle * Mathf.Deg2Rad)));
            shieldEffect.shieldDir = testDir;
            Debug.DrawRay(transform.position + (Vector3)shieldOrigin, testDir, Color.blue);
            shieldEffect.shieldPoint = shieldPoint;
        }
       
    }
    public void RotateShield( ref GameObject shield, float rotateSpeed )
    {
        if (shield != null)
        {
            float nextdAngle = (shieldAngle + rotateSpeed * Time.deltaTime) % 360;
            shieldAngle = nextdAngle;

        }
    }
    public void DetectBullet(Vector2 origin, float detectRadius, LayerMask detectMask)
    {
        Collider2D[] bulletArray = Physics2D.OverlapCircleAll(origin, detectRadius, detectMask);

        Collider2D bullet = null;
        foreach (Collider2D b in bulletArray)
        {
            if (b.tag == "BulletAlly" && bullet == null)
                bullet = b;
        }

        bulletToFocus = (bullet)? bullet.gameObject : null;
    }
    //attack
    IEnumerator AttackDuration()
    {
        yield return new WaitForSeconds(attackDuration);
        if (ennemyState == EnnemyState.ATTACK)
            ennemyState = EnnemyState.IDLE;
    }
}
