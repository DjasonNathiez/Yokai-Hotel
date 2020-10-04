using System.Collections;
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

    bool lightAttack;
    bool heavyAttack;
    public float lA_Buffer, hA_Buffer;
    
    float lA_Time, hA_Time;

    public bool attackable = false; 

   
    public int combo;
    bool comboUpdate = true;

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
        if (Input.GetButtonDown("Fire1"))
            lA_Time = Time.time;
        if (Input.GetButtonDown("Fire2"))
            hA_Time = Time.time;

        lightAttack = (Time.time - lA_Time < lA_Buffer);
        heavyAttack = (Time.time - hA_Time < hA_Buffer);

       
            

        //if (attackChoose > 1)
        //attackChoose = 1;





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
                    // dash
                    if (dash)
                        StartCoroutine(LoadDash());


                    // attack

                    if (!lightAttack && !heavyAttack)
                        attackable = true;
                   
                    if ((lightAttack || heavyAttack)&& attackable)
                    {
                        UpdateAttackChoose();
                        comboUpdate = true;

                        AttackDir = lastDir;

                        playerState = PlayerState.ATTACK;
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

                    // set up combo
                    if ((Time.time - lA_Time < lA_Buffer * 8) && comboUpdate && attackChoose ==1)
                    {
                        combo++;
                        comboUpdate = false;
                    }

                    if ((Time.time - lA_Time < lA_Buffer * 10) && comboUpdate && attackChoose == 0)
                    {
                        combo++;
                        comboUpdate = false;
                    }

                    if ((Time.time - lA_Time > lA_Buffer * 10))
                    {

                        combo = 0;
                    }

                    if (attackChoose == 2)
                        combo = 0;

                    
                    //if (attackable)
                        //UpdateAttackChoose();

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
    public void UpdateAttackChoose()
    {
        int lastAttackChoose = attackChoose;

        if (lightAttack)
            attackChoose = combo;
            

        if (heavyAttack)
            attackChoose = 3;

        if (lastAttackChoose != attackChoose && attackChoose != -1)
            attackable = false;
    }
    #endregion

}
