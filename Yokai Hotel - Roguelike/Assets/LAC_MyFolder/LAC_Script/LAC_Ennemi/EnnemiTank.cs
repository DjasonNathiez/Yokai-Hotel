using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiTank : EnnemiBehaviour
{
    [Header("Idle")]
    float nearestEnemyDist = Mathf.Infinity;
    public LayerMask detectMask;

    float detectTimer, detectFreq = 1f;

    public GameObject nearestEnemy, lastEnemy;

    Vector2 enemyTargetPos, enemyTargetLastPos;
    public Vector2 followDir, enemyDir, moveDir;

    float timeBetweenPos = 0.5f;
    bool updatingPos = true;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        // Update nearest Enemy Object
        if (detectTimer >= detectFreq)
        {
            Collider2D[] enemyList = EnemyList(transform.position, 5f, detectMask);
            ClosestEnemy(transform.position, enemyList, ref nearestEnemyDist);

            detectTimer = 0;
        }
        else
            detectTimer += Time.deltaTime;
        // Update pos & last pos of enemy
        if (updatingPos)
            StartCoroutine(UpdateEnemyTargetPos(timeBetweenPos));

        UpdateMoveDir(nearestEnemy, ref moveDir,0.5f,2.5f);

        if(nearestEnemy)
            Debug.DrawLine(transform.position, enemyTargetPos);

        Debug.DrawRay(transform.position, moveDir*2f, Color.red);

        SetVelocity(moveDir * speed, 0.1f);
    }

    public Collider2D[] EnemyList( Vector2 origin, float detectRadius, LayerMask detectMask)
    {
        return Physics2D.OverlapCircleAll(origin, detectRadius, detectMask);
    }
    public void ClosestEnemy( Vector2 origin, Collider2D[] enemyList, ref float nearestDist)
    {
        if (enemyList.Length <= 2)
        {
            nearestDist = Mathf.Infinity;
            nearestEnemy = null;
        }
        else
        {
            for (int i = 0; i < enemyList.Length; i++)
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
}
