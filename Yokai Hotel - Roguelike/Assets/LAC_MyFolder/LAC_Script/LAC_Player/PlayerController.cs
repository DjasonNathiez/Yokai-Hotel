
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2D;
    public enum PlayerState { FREE, DASH, ATTACK, HURT, MANAGE , DIE};
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
    public bool dash;
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
    public int maxHealth;
    public int hurtDamage;
    public bool isHurt;
    public bool isDead;

    public float hurtTime;
    public float invincibleTime;
    bool invincible;
    [Header("Manage Setting")]
    public string[] startTriggerM ;
    public string[] endTriggerM ;

    public float manageSpeed;
    public Vector2 manageDir;


    [Header("Shoot")]
    public float lAFillAmount;
    public float hAFillAmount;
    public float uAFillAmount;
    public float shootGaugeState;
    public float shootGaugeMax;

    [Header("Aim Assist")]
    public Collider2D[] hit;
    public GameObject currentTarget;
    public float radius;
    public float detectFreq;
    float detectTimer;
    public LayerMask ennemyLayer;
    public Vector2 aimDirection;

    [Header("FielOfView")] //angle de vision du personnage
    public GameObject up;
    public GameObject down;
    public GameObject right;
    public GameObject left;
    public bool ennemyDetected;

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
        if (cam)
            screenShake = cam.GetComponent<ScreenShake>();

        if (gameManager)
            audioManager = gameManager.GetComponent<AudioManager>(); //get audioManager

        // set dash value
        currentRecoilDash = recoilDashTime;
        currentRecoilReset = recoilResetTime;

        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        FielOfView();

        #region Input
        // movement
        Vector2 inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        if (inputAxis.magnitude > dirSensibility)
            lastDir = inputAxis;

         dash = Input.GetButtonDown("Dash");

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
        // debug
        if (Input.GetKeyDown(KeyCode.K))
            ChangeHealth(-health);
        // take damage
        if (hurtDamage != 0)
        {
            if (!invincible)
            {
                //Debug.Log("lose health");
                ChangeHealth(-hurtDamage);
                isHurt = true;
                invincible = true;

                StartCoroutine(Recovery());
                playerState = PlayerState.HURT;

                // death condition
                if (health <= 0)
                    Death();
            }

            hurtDamage = 0;
        }
        if (health <= 0)
        {
            playerState = PlayerState.DIE;
            isDead = true;
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
                        dashVelocity = lastDir.normalized * (dashDistance / dashTime);
                        StartCoroutine(LoadDash());
                        dashable = false;

                        if (audioManager)
                            audioManager.PlaySound("Player dash", 0);
                    }



                    // attack
                    if (Time.time - lA_Time >= lA_ComboDuration[combo])
                        combo = 0;

                    if (!lightAttack && !heavyAttack)
                        attackable = true;

                    if ((lightAttack || heavyAttack || shootAttack) && attackable)
                    {

                        comboUpdate = true;

                        attackDir = lastDir.normalized;

                        UpdateAttackChoose();

                        if (attackChoose != -1)
                        {
                            if ((int)attackM.attack[attackChoose].attackType == 0)
                                attackVelocity = lastDir * attackM.attack[attackChoose].inertness;

                            if (attackChoose == 4)
                            {

                                firePoint = lastDir.normalized * firePointRadius + (Vector2)transform.position;
                                firePoint.y += 0.5f;
                                bulletDir = aimDirection.normalized;
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

                        attackVelocity.x -= (distance / acceleration) * Time.deltaTime * Mathf.Sign(attackVelocity.x);
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

                    if (attackChoose == 2)
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
            case PlayerState.MANAGE:
                {
                    Vector2 targetVelocity = manageDir.normalized * manageSpeed;
                    velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity.x, ref velocitySmoothing.x,
                                                 (Mathf.Abs(velocity.x) <= Mathf.Abs(targetVelocity.x)) ? acceleration : deceleration);
                    velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocity.y, ref velocitySmoothing.y,
                                                 (Mathf.Abs(velocity.y) <= Mathf.Abs(targetVelocity.y)) ? acceleration : deceleration);
                    break;
                }

        }

        # region shootSection
        //maximize shoot gauge
        if (shootGaugeState > shootGaugeMax)
        {
            shootGaugeState = shootGaugeMax;
        }

        //timer detection cible
        if (detectTimer > detectFreq)
        {
            CheckTarget();
            detectTimer = 0;
        }

        detectTimer += Time.deltaTime;

        if (currentTarget && ennemyDetected == true)
        {
            aimDirection = (currentTarget.transform.position - transform.position).normalized;
        }
        else
        {
            aimDirection = lastDir.normalized;
        }
        #endregion
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


        }


        if (heavyAttack)
        {
            attackChoose = 3;
            shootGaugeState += hAFillAmount;

            if (screenShake)
                screenShake.isShaking = true;

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

    void CheckTarget()
    {
        float minDist = Mathf.Infinity;
        hit = Physics2D.OverlapCircleAll(transform.position, radius, ennemyLayer);

        if (hit.Length > 0)
        {
            foreach (Collider2D e in hit)
            {
                float distanceTarget = Vector2.Distance(transform.position, e.transform.position);

                if (minDist > distanceTarget)
                {
                    minDist = distanceTarget;
                    currentTarget = e.gameObject;
                }
            }
        }
        else
        {
            currentTarget = null;
        }

    }

    void FielOfView()
    {
        #region activation direction
        if (up || down || right || left)
        {

            if (lastDir.x == 0 && lastDir.y == 1)
            {
                up.SetActive(true);
                down.SetActive(false);
                right.SetActive(false);
                left.SetActive(false);
            }

            if (lastDir.x == 1 && lastDir.y == 0)
            {
                up.SetActive(false);
                down.SetActive(false);
                right.SetActive(true);
                left.SetActive(false);
            }

            if (lastDir.x == 0 && lastDir.y == -1)
            {
                up.SetActive(false);
                down.SetActive(true);
                right.SetActive(false);
                left.SetActive(false);
            }

            if (lastDir.x == -1 && lastDir.y == 0)
            {
                up.SetActive(false);
                down.SetActive(false);
                right.SetActive(false);
                left.SetActive(true);
            }
        }
       

        #endregion



    }
    #endregion

    // Manage cond
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(startTriggerM.Length > 0 && endTriggerM.Length > 0)
        {
            foreach(string s in startTriggerM)
            {
                if(collision.tag == s)
                    playerState = PlayerState.MANAGE;
            }

            foreach (string s in endTriggerM)
            {
                if (collision.tag == s)
                    playerState = PlayerState.FREE;
            }

        }
        
    }

    public void ChangeHealth( int value)
    {
        Debug.Log((value>= 0)? "player heal": "player lose health");
        health = Mathf.Clamp(health + value, 0, maxHealth);

        if (health <= 0)
            Death();
    }
    public void ChangeMaxHealth(int value)
    {
        Debug.Log((value >= 0) ? "player + MaxHealth" : "player - MaxHealth");
        maxHealth += value;
    }

    public void Death()
    {
        velocity = Vector2.zero;
        playerState = PlayerState.DIE;
        Debug.LogError("Nice try");
        StartCoroutine(Restart(4));
    }

    public IEnumerator Restart(float delay)
    {
        yield return new WaitForSeconds(delay);

        int restartIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(restartIndex);
    }

}

