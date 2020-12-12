
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2D;
    public enum PlayerState { FREE, DASH, ATTACK, HURT };
    public PlayerState playerState = PlayerState.FREE;

    [Header("Movement")]
    public float speed;
    public float acceleration, deceleration;

    float dirSensibility = 0.01f;
    [HideInInspector]
    public Vector2 lastDir;

    public Vector2 velocity;
    [HideInInspector]
    public Vector2 velocitySmoothing;

    [Header("Dash")]
    public float dashDistance;
    public float dashTime;

    [HideInInspector]
    public Vector2 dashDir;
    public float loadDashTime, recoilDashTime;
    Vector2 dashVelocity;
    bool dashable = true;

    public float recoilMultiplier, recoilResetTime, recoilDashLimit;
    float currentRecoilReset, currentRecoilDash;

    public float invincibleDelay, invincibleDuration;

    [Header("Attack")]

    [HideInInspector]
    public bool lightAttack;
    [HideInInspector]
    public bool heavyAttack;
    [HideInInspector]
    public bool shootAttack;


    public bool attackable = false;

    public float lA_Buffer, hA_Buffer, sA_Buffer;
    float lA_Time, hA_Time, sA_Time;

    public float[] lA_ComboDuration;

    public int combo;
    bool comboUpdate = true;

    public Vector2 firePoint, bulletDir;
    float firePointRadius = 0.7f;

    public int attackChoose = -1;
    public Vector2 attackDir;
    public Vector2 attackVelocity;

    public AttackManager attackM;

    [Header("HURT")]
    public int health;
    public int hurtDamage;
    public bool isHurt;

    public float hurtTime;
    public float invincibleTime;
    bool invincible;

    [Header("Shoot")]
    public float lAFillAmount;
    public float hAFillAmount;
    public float uAFillAmount;
    public float shootGaugeState;
    public float shootGaugeMax;

    [Header("Aim Assist")]
    public float radius;
    public float distance;
    Vector2 nearEnnemyPos;

    SpriteRenderer spriteT;

    AudioManager audioManager;
    GameObject gameManager;
    ScreenShake screenShake;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        attackM = GetComponentInChildren<AttackManager>();

        spriteT = GetComponentInChildren<SpriteRenderer>();

        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        GameObject cam = GameObject.FindGameObjectWithTag("VirtualCam");
        if(cam)
            screenShake = cam.GetComponent<ScreenShake>();

        if (gameManager)
            audioManager = gameManager.GetComponent<AudioManager>(); //get audioManager

        // set dash value
        currentRecoilDash = recoilDashTime;
        currentRecoilReset = recoilResetTime;
    }

    // Update is called once per frame
    void Update()
    {

        #region Input
        // movement
        Vector2 inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        if (inputAxis.magnitude > dirSensibility)
            lastDir = inputAxis;

        bool dash = Input.GetButtonDown("Dash");

        // attack
        if (Input.GetButtonDown("Attack1"))
            lA_Time = Time.time;

            
        if (Input.GetButtonDown("Attack2"))
            hA_Time = Time.time;


        if (Input.GetButtonDown("Shoot") && shootGaugeState == shootGaugeMax)
            sA_Time = Time.time;

            

        lightAttack = (Time.time - lA_Time < lA_Buffer);
        heavyAttack = (Time.time - hA_Time < hA_Buffer);
        shootAttack = (Time.time - sA_Time < sA_Buffer);

        //if (attackChoose > 1)
        //attackChoose = 1;

        #endregion

        // take damage
        if(hurtDamage != 0 )
        {
            if (!invincible)
            {
                Debug.Log("lose health");
                health -= hurtDamage;
                isHurt = true;
                invincible = true;

                StartCoroutine(Recovery());
                playerState = PlayerState.HURT;
            }

            hurtDamage = 0;
        }

        switch (playerState)
        {
            case PlayerState.FREE:
                {
                    // displacement
                    Vector2 targetVelocity = inputAxis.normalized * speed;
                    velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity.x, ref velocitySmoothing.x, 
                                                 (Mathf.Abs(velocity.x) <= Mathf.Abs(targetVelocity.x)) ? acceleration : deceleration);
                    velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocity.y, ref velocitySmoothing.y, 
                                                 (Mathf.Abs(velocity.y) <= Mathf.Abs(targetVelocity.y)) ? acceleration : deceleration);
                    // dash
                    if (dash && dashable)
                    {
                        //dashDir = lastDir.normalized;
                        dashVelocity = lastDir.normalized *(dashDistance / dashTime);
                        StartCoroutine(LoadDash());
                        dashable = false;

                        if(audioManager)
                            audioManager.PlaySound("Player dash", 0);
                    }
                        


                    // attack
                    if (Time.time - lA_Time >= lA_ComboDuration[combo])
                        combo = 0;

                    if (!lightAttack && !heavyAttack)
                        attackable = true;
                   
                    if ((lightAttack || heavyAttack|| shootAttack )&& attackable)
                    {
                       
                        comboUpdate = true;

                        attackDir = lastDir.normalized;

                        UpdateAttackChoose();

                        if (attackChoose != -1)
                        {
                            if ((int)attackM.attack[attackChoose].attackType == 0) 
                                attackVelocity = lastDir * attackM.attack[attackChoose].inertness;

                            if(attackChoose == 4)
                            {
                                
                                firePoint = lastDir.normalized * firePointRadius + (Vector2)transform.position;
                                firePoint.y += 0.5f;
                                bulletDir = lastDir.normalized;
                            }

                            playerState = PlayerState.ATTACK;
                        }

                    }

                    break;
                }

            case PlayerState.DASH:
                {
                    velocity = dashVelocity;
                    break;
                }

            case PlayerState.ATTACK:
                {
                    // reset velocity
                    if (attackM && attackChoose != -1)
                    {
                        float acceleration = attackM.attack[attackChoose].inertnessTime + 0.001f;
                        float distance = attackM.attack[attackChoose].inertness;

                        attackVelocity.x -= (distance/ acceleration) * Time.deltaTime * Mathf.Sign(attackVelocity.x);
                        attackVelocity.y -= (distance / acceleration) * Time.deltaTime * Mathf.Sign(attackVelocity.y);

                        velocity = attackVelocity;
                    }
                    else
                    velocity = Vector2.zero;

                    // set up combo
                    bool combo1 = ((Time.time - lA_Time < lA_ComboDuration[1]) && attackChoose == 1);
                    bool combo2 = ((Time.time - lA_Time < lA_ComboDuration[0]) && attackChoose == 0);

                    if ((combo1 || combo2) && comboUpdate)
                    {
                        combo++;
                        comboUpdate = false;
                    }

                    if(attackChoose == 2)
                        combo = 0;

                    break;
                }

            case PlayerState.HURT:
                {
                    Color col = Color.red;
                    col.a = Mathf.Sin(Time.time * 30f) * 255;

                    spriteT.color = col;
                    break;
                }

        }

        //maximize shoot gauge
        if(shootGaugeState > shootGaugeMax)
        {
            shootGaugeState = shootGaugeMax;
        }
        
    }

    private void FixedUpdate()
    {
        // move
        rb2D.velocity = velocity;
    }

    #region Dash
    public IEnumerator LoadDash()
    {
        yield return new WaitForSeconds(loadDashTime);
        playerState = PlayerState.DASH;

        yield return new WaitForSeconds(dashTime);
        dashVelocity = Vector2.zero;

        yield return new WaitForSeconds(recoilDashTime);
        dashable = true;
        playerState = PlayerState.FREE;
    }
    #endregion

    #region Attack
    public void UpdateAttackChoose()
    {
        int lastAttackChoose = attackChoose;

        if (lightAttack)
        {
            attackChoose = combo;
            shootGaugeState += lAFillAmount;
        

            if (audioManager)
                audioManager.PlaySound("Player fast attack", 0);
        }
            

        if (heavyAttack)
        {
            attackChoose = 3;
            shootGaugeState += hAFillAmount;

            if(screenShake)
                screenShake.isShaking = true;

            if (audioManager)
                audioManager.PlaySound("Player heavy attack", 0);
        }
          

        if (shootAttack)
        {
            attackChoose = 4;
            if (audioManager)
                audioManager.PlaySound("Player distance attack", 0);
        }

           

        if (lastAttackChoose != attackChoose && attackChoose != -1)
            attackable = false;
    }
    #endregion

    #region Health
    public IEnumerator Recovery()
    {
        yield return new WaitForSeconds(hurtTime);
        spriteT.color = Color.white;
        playerState = PlayerState.FREE;

        yield return new WaitForSeconds(invincibleTime - hurtTime);
        invincible = false;
    }
    #endregion

    #region Aiming Aid

    #endregion
}

