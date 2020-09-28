﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2D;
    public enum PlayerState { FREE, DASH, ATTACK, HURT};
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

    public float recoilMultiplier, recoilResetTime, recoilDashLimit;
    float currentRecoilReset, currentRecoilDash;

    public float invincibleDelay, invincibleDuration;
    [Header("Attack")]
    public int combo;
    public int attackChoose = -1;

    public AttackManager attackM;

    public Vector2 AttackDir;
    public bool activeDamage;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        attackM = GetComponent<AttackManager>();

        // set dash value
        currentRecoilDash = recoilDashTime;
        currentRecoilReset = recoilResetTime;
    }

    // Update is called once per frame
    void Update()
    {
        #region Input
        // movement
        Vector2 inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (inputAxis.magnitude > dirSensibility)
            lastDir = inputAxis;
        bool dash = Input.GetButtonDown("Jump");

        // attack
        bool lightAttack = (Input.GetButtonDown("Fire1"));
        bool heavyAttack = (Input.GetButtonDown("Fire2"));

        bool dashAttack = (dash && lightAttack);
        bool counter = (dash && heavyAttack);
        bool stunAttack = (lightAttack && heavyAttack);

        // select attack
        if(!counter && !dashAttack && !stunAttack)
        {
            if (lightAttack)
                attackChoose = combo;
            if (heavyAttack)
                attackChoose = 3;
        }

        if (stunAttack && !dash)
            attackChoose = 4;

        #endregion

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

                    if (dash)
                        StartCoroutine(LoadDash());

                    if (lightAttack || heavyAttack)
                    {
                        playerState = PlayerState.ATTACK;
                        AttackDir = lastDir;
                    }

                    break;
                }

            case PlayerState.DASH:
                {
                    float dashSpeed = dashDistance / dashTime;
                    velocity = dashDir.normalized * dashSpeed;
                    break;
                }

            case PlayerState.ATTACK:
                {
                    // reset velocity
                    velocity = Vector2.zero;
                    break;
                }

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
        dashDir = lastDir;
        yield return new WaitForSeconds(loadDashTime);
        playerState = PlayerState.DASH;

        yield return new WaitForSeconds(recoilDashTime);
        playerState = PlayerState.FREE;
    }
    #endregion

    #region Attack
    public void SetInertness( float inesrtness)
    {
        velocity = lastDir * inesrtness;
    }
    #endregion

}
