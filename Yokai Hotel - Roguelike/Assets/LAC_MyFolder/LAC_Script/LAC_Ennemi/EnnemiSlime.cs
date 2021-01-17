using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiSlime : EnnemiBehaviour
{
    #region Idle
    [Header("Idle")]
    public Vector2 idleDir;

    public float idleRad;
    public float idleDeg;

    bool[] checkDir = { false, false, false };
    bool[] triggerDir = { false, false, false };

    Vector2 lastIdleDir;

    public bool normalDir;
    public bool changeDir;

    [Range(0, 1)]
    public float pChangeDir;
    public float pFrequence;
    float pFreqTimer;

    public float wallDetectRadius = 1;
    public List<Vector2> wallDirs = new List<Vector2>();
    #endregion
    #region Chase
    [Header("Chase")]
    public float detectRadius;
    public float loadTimeBoost;
    public float lowSpeed;

    bool triggerLoadBoost;
    #endregion

    #region attack
    [Header("Attack")]
    public int attackDamage;
    public GameObject bulletObj;

    public int attackStock, maxAttackStock;
    public float minAttackDetect, maxAttackDetect;
    public List<Bullet> activeBullet;
 

    public float timeToLoad, timeBetweenAttack;
    float loadTimer;
    bool attackable, shootAnim;

    #endregion
    public override void Start()
    {
        base.Start();
        idleRad = Vector2.Angle(Vector2.right, idleDir) * Mathf.Deg2Rad;
    }
    // Update is called once per frame
    public override void Update()
    {
        if(healthPoints < 0)
        {
            foreach(Bullet b in activeBullet)
            {
                if(b != null)
                    Destroy(b.gameObject);
            }
        }

        // detect player
        float targetDistX = Mathf.Abs(target.transform.position.x - transform.position.x);
        float targetDistY = Mathf.Abs(target.transform.position.y - transform.position.y);

        bool detectMinDist = ( targetDistY < minAttackDetect ||  targetDistX < minAttackDetect);
        bool detectMaxDist = (targetDistY > maxAttackDetect || targetDistX > maxAttackDetect);

        // load bullet
        bool ChaseCond = (targetDist < detectRadius && (int)player.playerState == 2);

        if (ChaseCond)
        {
            if (triggerLoadBoost)
            {
                loadTimer += loadTimeBoost;
                triggerLoadBoost = false;
            }
        }
        else
            triggerLoadBoost = true;

        if (loadTimer >= timeToLoad)
        {
            attackStock = Mathf.Clamp(attackStock + 1, 0, maxAttackStock);
            loadTimer = 0;
        }
        loadTimer += Time.deltaTime;

        switch (ennemyState)
        {
            case EnnemyState.IDLE:
                {

                    StealthMove();
                    float currentSpeed = ChaseCond ? lowSpeed : speed;
                    SetVelocity(idleDir * currentSpeed, 0.1f);

                    if (detectMinDist && targetVisible)
                    {
                        attackable = false;

                        if(attackStock == 0)
                            attackStock++;

                        ennemyState = EnnemyState.ATTACK;
                        StartCoroutine(StartAttack(0.1f));
                    }
                        
          
                    break;
                }

            case EnnemyState.ATTACK:
                {
                    //Shoot();

                    if ((detectMaxDist && !detectMinDist && attackStock == 0) || !targetVisible)
                        ennemyState = EnnemyState.IDLE;

                    SetVelocity(Vector2.zero, 0.2f);
                    break;
                }
        }
        

        base.Update();
    }

    public void StealthMove()
    {
        //List<Vector2> checkDirs = new List<Vector2>();

        Vector2 secondDir = new Vector2(Mathf.Cos(idleRad - Mathf.PI / 2), Mathf.Sin(idleRad - Mathf.PI / 2));
        Vector2 thirdDir = new Vector2(Mathf.Cos(idleRad + Mathf.PI / 2), Mathf.Sin(idleRad + Mathf.PI / 2));

        Vector2[] dirToCheck = { idleDir, secondDir, thirdDir};

        for( int i = 0; i<dirToCheck.Length;i++)
        {
            Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + dirToCheck[i] * wallDetectRadius, 0.2f, obstructMask);
            checkDir[i] = hit;

            if (hit)
                triggerDir[i] = true;     
        }

        if(!checkDir[1] && triggerDir[1])
        {
            idleRad -= Mathf.PI / 2;
            idleDir = new Vector2(Mathf.Cos(idleRad), Mathf.Sin(idleRad));
            Debug.Log("turn");
            triggerDir[1] = false;
        }

        if (!checkDir[2] && triggerDir[2])
        {
            idleRad += Mathf.PI / 2;
            idleDir = new Vector2(Mathf.Cos(idleRad), Mathf.Sin(idleRad));
            triggerDir[2] = false;
        }

        if (checkDir[0])
        {
            if (checkDir[1])
                idleRad += Mathf.PI / 2;
            
            if (checkDir[2])
                idleRad -= Mathf.PI / 2;

            if(!checkDir[1] && !checkDir[2])
                idleRad -= Mathf.PI / 2;

            idleDir = new Vector2(Mathf.Cos(idleRad), Mathf.Sin(idleRad));
        }

        if (pFreqTimer >= pFrequence)
        {
            if(Random.value < pChangeDir)
                    idleDir = -idleDir;
            pFreqTimer = 0;
        }
        pFreqTimer += Time.deltaTime;

        idleDeg = idleRad * Mathf.Rad2Deg;
    }
    public IEnumerator StartAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        attackable = true;
    }
    public void Shoot()
    {
        if (attackable && attackStock > 0)
        {
            StartCoroutine(StartAttack(timeBetweenAttack));
            attackable = false;
            attackStock--;

            GameObject bullet = Instantiate(bulletObj, transform.position, transform.rotation);
            Bullet b = bullet.GetComponent<Bullet>();
            activeBullet.Add(b);

            if (b)
                b.dir = targetDir;

        }
    }

    private void OnDrawGizmos()
    {
        float idleRad = Vector2.Angle(Vector2.right, idleDir) * Mathf.Deg2Rad;
        Vector2 secondDir = new Vector2(Mathf.Cos(idleRad - Mathf.PI / 2), Mathf.Sin(idleRad - Mathf.PI / 2));
        Vector2 thirdDir = new Vector2(Mathf.Cos(idleRad + Mathf.PI / 2), Mathf.Sin(idleRad + Mathf.PI / 2));

        Vector2[] dirToCheck = { idleDir, secondDir, thirdDir };

        for (int i = 0; i < dirToCheck.Length; i++)
        {
            Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + dirToCheck[i] * wallDetectRadius, 0.2f, obstructMask);
            if (hit)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.white;
            Gizmos.DrawWireSphere((Vector2)transform.position + dirToCheck[i] * wallDetectRadius, 0.2f);

        }
    }
}
  