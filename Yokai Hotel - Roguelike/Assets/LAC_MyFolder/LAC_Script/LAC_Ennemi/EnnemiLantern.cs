﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiLantern : EnnemiBehaviour
{
    // Start is called before the first frame update

    #region Idle
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
    #endregion
    #region Chase
    [Header("Chase")]
    public float detectCycle;
    float detectTimer = 0;

    public float minDetectRadius;
    public float maxDetectRadius;
    #endregion
    #region Attack
    [Header("Attack")]
    public int attackDamage;

    public float minCacRadius;
    public float maxCacRadius;

    public float attackFrequence;
    public float breakDuration;
    public float aFrqcTimer = 0;

    #endregion;


    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Debug.DrawRay(transform.position, targetDir * targetDist, Color.white);

        Debug.DrawRay(transform.position, velocity*2, Color.blue);
        Debug.DrawRay(transform.position, idleDir*2, Color.red);

        switch (ennemyState)
        {
            case EnnemyState.IDLE:
                {
                    if (targetVisible && targetDist <= minDetectRadius)
                    {
                        SetVelocity(Vector2.zero, 0.1f);
                        ennemyState = EnnemyState.AGGRO;
                    }
                    else
                    Wandering();

                    break;
                }
            case EnnemyState.AGGRO:
                {

                    SetVelocity(targetDir * speed,0.1f);

                    if (targetDist > maxDetectRadius || !targetVisible)
                        ennemyState = EnnemyState.IDLE;

                    if (targetDist <= minCacRadius)
                        ennemyState = EnnemyState.ATTACK;

                    aFrqcTimer = attackFrequence;
                    break;
                }
            case EnnemyState.ATTACK:
                {
                    velocity = Vector2.zero;
                    aFrqcTimer += Time.deltaTime;

                    if (targetDist > maxCacRadius && aFrqcTimer < attackFrequence)
                        ennemyState = EnnemyState.IDLE;

                    break;
                }
        }
    }

    public void Wandering()
    {

        idleCycleTimer += Time.deltaTime;

        if (idleCycleTimer >= idleCycle)
        {
            idleCycleTimer = 0;

            idleDir = new Vector2((Random.value - 0.5f) + targetDir.x, (Random.value - 0.5f) + targetDir.y); // move random dir
            idlePoint = (Vector2)transform.position + idleDir * idleRadius;
        }

        //if (Vector2.Distance(transform.position, idlePoint) > 0.5f)
        if (idleTimer < idleTime)
        {
            float detectLength = cC2D.radius ;

            Vector2 currentDir = new Vector2((DetectBlock(detectLength, Vector2.right * idleDir.x, obstructMask)) ? -idleDir.x : idleDir.x,
                                             (DetectBlock(detectLength, Vector2.up * idleDir.y, obstructMask)) ? -idleDir.y : idleDir.y);

            SetVelocity(currentDir *speed, 0.1f);
            idleDir = currentDir;

            idleTimer += Time.deltaTime;
        }

        else
        {
            idleTimer = 0;
            SetVelocity(Vector2.zero, 0);
        }


    }
}
