using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Boss")]
    public int globalBossHp;
    public int currentBossHp;
    public int singleBossHp;

    public int totalDamageToDeal;
    
    public BossBehaviour[] bossArray;
    public GameObject player;
    public LayerMask thatMask;

    [Header("Phase")]
    public bool phaseTwo;

    public int pTwoHp;
    public bool start;
    public bool end;

    [Header("Pattern Offset")]
    public Vector2 attackOffset;
    public Vector2 dashOffset;
    public Vector2 shootOffset;

    [Header("Pattern Timing")]
    public float paternFreq;

    float[] paternTimer;
    public float timerOffset;

    float paternReducProba;
    int lastPatern = 0;
    // Start is called before the first frame update
    void Start()
    {
        bossArray = GetComponentsInChildren<BossBehaviour>();
        paternTimer = new float[bossArray.Length];

        currentBossHp = globalBossHp ;
        singleBossHp = ((globalBossHp * 2)+6);
        for (int i = 0; i < bossArray.Length; i++)
        {
            bossArray[i].healthPoints = singleBossHp;
            paternTimer[i] = paternFreq -timerOffset * i;
            bossArray[i].gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateBossHP();

        for(int i = 0; i < bossArray.Length; i++)
        {
            if(bossArray[i]!= null && !end)
            {

                //define patern
                paternTimer[i] -= Time.deltaTime;
                if (paternTimer[i] < 0)
                {
                    // initialize
                    if (!bossArray[i].isActiveAndEnabled)
                    {
                        if((i == 1 && phaseTwo) || i == 0)
                            bossArray[i].gameObject.SetActive(true);

                    }
                        

                    // apply pattern
                    StartPosPatern(bossArray[i], 1, 1, 1);
                    paternTimer[i] = paternFreq;
                }
            }
        }

       
    }
  

    void StartPosPatern(BossBehaviour boss, float pAttack, float pDash, float pShoot)
    {
        List<Vector2> checkDir = new List<Vector2> { Vector2.right , Vector2.up, Vector2.left, Vector2.down };
        Vector2 startPos = Vector2.zero;
        Vector2 orient = Vector2.zero;

        #region Define Pattern
        int paternIndex = DefinePatern(pAttack, pDash, pShoot);
        
        Vector2 offSet = Vector2.zero;

        if (paternIndex == 1)
        {
            offSet = attackOffset;
            Debug.Log("Boss Attack");
        }
           
        if (paternIndex == 2)
        {
            offSet = dashOffset;
            Debug.Log("Boss Dash");
        }
            
        if (paternIndex == 3)
        {
            offSet = shootOffset;
            Debug.Log("Boss Shoot");
        }
        #endregion

        // choose & check pos
        bool findDir = false;
        while(!findDir)
        {
            Vector2 chooseDir = checkDir[Random.Range(0, checkDir.Count)];
            float dirRad = Mathf.Atan2(offSet.y, offSet.x) + Mathf.Atan2(chooseDir.y, chooseDir.x);
            Vector2 choosePos = new Vector2(Mathf.Cos(dirRad), Mathf.Sin(dirRad)).normalized * offSet.magnitude;

            choosePos = CheckDir(choosePos, 0.8f, thatMask);
           
            if (choosePos != Vector2.zero)
            {
                startPos = choosePos + (Vector2)player.transform.position;
                orient = chooseDir;
                //Debug.Log("dir found : " + chooseDir);
                findDir = true;
            }
            else
            {
                checkDir.Remove(chooseDir);
                //Debug.Log("remove dir : " + chooseDir);
            }
                

            if (checkDir.Count == 0)
            {
                //Debug.Log("no dir found: " + chooseDir);
                findDir = true;
            }

        }

        if(startPos != Vector2.zero)
        {
            boss.transform.position = startPos;
            boss.orient = orient;
            boss.bossState = (BossBehaviour.BossState)paternIndex;
        }
    }

    int DefinePatern(float pAttack, float pDash, float pShoot)
    {

        float pTotal = pAttack + pDash + pShoot;
        pAttack = pAttack/pTotal;
        pDash = pDash/ pTotal;
        pShoot = pShoot / pTotal;

        float rValue = Random.value;
        int patternIndex = 1;

        if (rValue >= 0 && rValue <= pAttack)
            patternIndex = 1;

        if (rValue > pAttack && rValue <= pAttack + pDash)
            patternIndex = 2;

        if (rValue > pAttack + pDash && rValue <= pTotal)
            patternIndex = 3;

        //Debug.Log("random value" + rValue);
        //Debug.Log("pattern index" + patternIndex);
        return patternIndex;
    }
    Vector2 CheckDir( Vector2 dir, float minDistMultiplier, LayerMask mask)
    {
        Vector2 vToReturn = Vector2.zero;
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, dir.normalized, dir.magnitude, mask);
        Debug.DrawRay(player.transform.position, dir, Color.blue,2f);
        if (hit)
        {
            if (hit.distance > dir.magnitude * minDistMultiplier)
                vToReturn = dir.normalized * hit.distance;
        }
        else 
           vToReturn = dir;

        return vToReturn;
        

    }

    public void UpdateBossHP()
    {
       int tempBossHp = globalBossHp;
       for(int i = 0; i < bossArray.Length; i++)
       {
            tempBossHp -= ((bossArray[i] != null) ? singleBossHp : 0) - (int)bossArray[i].healthPoints;
       }
        currentBossHp = tempBossHp;

        if (currentBossHp <= pTwoHp)
            phaseTwo = true;

        if (currentBossHp <= 0)
            Debug.LogError("Weakness : To strong !");
    }
  
}
