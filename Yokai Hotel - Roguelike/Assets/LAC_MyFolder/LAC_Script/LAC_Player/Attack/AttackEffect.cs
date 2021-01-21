using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AttackEffect : MonoBehaviour
{
    public PlayerController player;
    AttackManager attackM;
    Animator animator;
    public BoxCollider2D col2D;

    public LayerMask detectMask;
    public GameObject bulletPrefab;

    [Header("Attack Boost")]
    public float heavyBoost = 1;
    public float lightBoost = 1, sA_boost = 1;

    public float attackBoost = 1;
    public float theftLifeProba = 0;
    public float dropBoost = 1;

    [HideInInspector]
    public bool kill_L;
    [HideInInspector]
    public bool kill_H;

    float killbuffer = 0.2f;
    [Header("VFX")]
    public float rotation;
    public ParticleSystem smallImpact;
    public ParticleSystem bigImpact;

    [Header("Cam")]
    public GameObject cam;
    public CinemachineBrain camBrain;
    public CinemachineVirtualCamera virtualCam;
    float screenShakeCount;

    AudioManager audioM;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        attackM = GetComponent<AttackManager>();
        col2D = GetComponent<BoxCollider2D>();

        cam = GameObject.FindGameObjectWithTag("MainCamera");
        camBrain = cam.GetComponent<CinemachineBrain>();

        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");

        if(gameManager)
            audioM = gameManager.GetComponent<AudioManager>();

        
    }

    private void OnDrawGizmos()
    {
        if(col2D != null)
        {
            Vector2 size = new Vector2(col2D.bounds.max.x - col2D.bounds.min.x, col2D.bounds.max.y - col2D.bounds.min.y);
        }
            
        //Gizmos.DrawCube(col2D.offset + (Vector2)transform.position, size);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ennemi" && player.attackChoose != -1)
        {

            EnnemiBehaviour ennemi = collider.GetComponentInParent<EnnemiBehaviour>();
            if (ennemi)
            {
                EnnemiTank eTank = ennemi.GetComponentInParent<EnnemiTank>();
                bool deflectShield = false;

                int bossScreenShake = (ennemi.GetComponentInParent<BossBehaviour>() == null) ? 1 : 0;
                float attackTypeBoost = 1;
                if (attackM.attack[player.attackChoose].attackType == Attack.AttackType.HEAVY)
                    attackTypeBoost *= heavyBoost;
                if (attackM.attack[player.attackChoose].attackType == Attack.AttackType.LIGHT)
                    attackTypeBoost *= lightBoost;

                float damage = attackM.attack[player.attackChoose].damage * attackBoost * attackTypeBoost;
                Vector2 repulseDir = (ennemi.transform.position - player.transform.position).normalized;
                if (eTank)
                {
                    if (eTank.shield != null)
                    {
                        Debug.Log("Hit shield");
                        Vector2 enemyDir = (collider.transform.position - transform.position).normalized;
                        float enemyAngle = ((Mathf.Atan2(enemyDir.y, enemyDir.x) * Mathf.Rad2Deg) + 180) % 360;
                        if (Mathf.Sign(enemyAngle) != Mathf.Sign(eTank.shieldAngle))
                            enemyAngle = (Mathf.Sign(enemyAngle) < 0) ? (enemyAngle + 360) % 360 : (enemyAngle - 360) % 360;

                        Debug.Log("ennemyAngle" + enemyAngle);
                        Debug.Log("shieldAngle" + eTank.shieldAngle);
                        if (Mathf.Abs(enemyAngle - eTank.shieldAngle) < eTank.defelectDegree)
                            deflectShield = true;
                    }
                }

                if (deflectShield)
                {
                    if (player.attackChoose == 3)
                        damage -= 1;

                    eTank.shieldPoint -= damage;
                    damage = 0;

                    Debug.Log("damage shield");
                }
                else if (damage > 0)
                {

                    // feedBack & kill effect
                    ScreenShake(attackM.attack[player.attackChoose].screenShakeAmp * bossScreenShake, attackM.attack[player.attackChoose].screenShakeFreq *bossScreenShake, attackM.attack[player.attackChoose].screenShakeTime);
                    if (ennemi.healthPoints <= damage)
                    {
                        if (player.attackChoose == 3)
                            kill_H = true;
                        if (player.attackChoose == 0 || player.attackChoose == 1 || player.attackChoose == 2)
                            kill_L = true;
                        if(dropBoost != 1)
                        {
                            DropSystem dropEnnemi = ennemi.GetComponent<DropSystem>();
                            dropEnnemi.dropRateG *= dropBoost;
                        }
                       
                        if (attackM.attack[player.attackChoose].screenShakeAmp == 0)
                            ScreenShake(0.5f, 4, 0.3f);
                        StartCoroutine(KillReset(killbuffer));
                        SlowTime(0.2f, 0.3f);
                        

                    }
                    PlayVFX(ennemi.gameObject, player.attackChoose);

                    // apply damage & recoil
                    Debug.Log("Hit ennemy");
                    ennemi.healthDamage = damage;
                    ennemi.inertnessModifier = attackM.attack[player.attackChoose].knockBackModifier;
                    ennemi.repulseForce = attackM.attack[player.attackChoose].knockBackValue * repulseDir;
                    ennemi.stunTime = attackM.attack[player.attackChoose].stunTime;

                    damage = 0;
                    
                }

            }
            if(audioM)
                audioM.PlaySound("Ennemy hurt", 0);
        }

    }

    public void FREE_StateTranstion()
    {
        player.playerState = PlayerController.PlayerState.FREE;
        ResetAttack();
        player.attackable = true;
    }
    public void AttackEnd()
    {
        player.velocity = Vector2.zero;
        ResetAttack();
    }
    public void AttackStart()
    {

    }

    public void SetInertness()
    {
        if(player.attackChoose != -1)
        {
            player.attackVelocity = player.lastDir * attackM.attack[player.attackChoose].inertness;
        }
    }
    public void ResetAttack()
    {
        player.attackChoose = -1;
    }
    public void Attackable(bool allow)
    {
        player.attackable = allow;
    }

    public void InstantiateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, player.firePoint, transform.rotation);
        Bullet b = bullet.GetComponent<Bullet>();
        if (b)
        {
            b.dir = player.bulletDir;
            player.shootGaugeState = 0;
        }
            
    }


    /*public IEnumerator Inertness(float time)
    {
       

        yield return new WaitForSeconds(time);
        //player.velocity = Vector2.zero;
    }*/
    #region Combo

    /*
    public void SetCombo()
    {
        if (player.attackChoose != -1)
        {
            int comboLevel = attackM.attack[player.attackChoose].comboLevel;
            float comboTime = attackM.attack[player.attackChoose].comboDuration;

            player.combo = comboLevel;
            StartCoroutine(ResetCombo(comboTime));
        }
        
    }
    public IEnumerator ResetCombo(float time)
    {
        yield return new WaitForSeconds(time);
        player.combo = 0;
    }*/
    #endregion

    IEnumerator InitializeCam()
    {
        yield return null;
        virtualCam = camBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }
    public void ScreenShake(float amp, float freq,  float time)
    {
        virtualCam = camBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (virtualCam)
        {
            screenShakeCount++;
            CinemachineBasicMultiChannelPerlin basicPerlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (basicPerlin)
            {
                basicPerlin.m_AmplitudeGain = amp;
                basicPerlin.m_FrequencyGain = freq;
                StartCoroutine(shakeTime(basicPerlin, time));
            }
            else
                Debug.Log("can't shake");
            
        }
        
    }
    public IEnumerator shakeTime(CinemachineBasicMultiChannelPerlin basicPerlin, float time)
    {
        
        yield return new WaitForSeconds(time);
        screenShakeCount--;
        if (screenShakeCount <= 0)
        {
            basicPerlin.m_AmplitudeGain = 0;
            basicPerlin.m_FrequencyGain = 0;
        }
            
    }
    public void SlowTime( float slowFactor, float slowDelay)
    {
        Time.timeScale = slowFactor;
        StartCoroutine(SlowDuration(slowDelay));
    }
    public IEnumerator SlowDuration( float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1;
    }
    public IEnumerator KillReset(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        kill_L = kill_H = false;
    }

    public void LightAttackSound()
    {
        if(audioM)
            audioM.PlaySound("Player fast attack", 0);
    }

    public void HeavyAttackSound()
    {
        if (audioM)
            audioM.PlaySound("Player heavy attack", 0);
    }

    public void DeathSound()
    {
        if (audioM)
            audioM.PlaySound("Player death", 0);
    }

    // VFX
    public void PlayVFX(GameObject target, int attackType)
    {
        // define vfx type
        ParticleSystem particule = ((attackType < 3 && lightBoost > 1) || (attackType == 3 && heavyBoost > 1) || attackBoost > 1) ? bigImpact : smallImpact;
            
        // define vfx position
        Vector2 dir = (target.transform.position - transform.position);
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, dir.normalized, dir.magnitude, detectMask);
        Vector2 pos = (Vector2)transform.position + dir.normalized * ((hit) ? hit.distance : dir.magnitude);
        

        // play particule
        bool vertical = (Mathf.Abs(dir.y) > Mathf.Abs(dir.x));
        float particuleRotation = (vertical ? 1.5f : 0) + Random.Range(-0.5f,0.5f) ;

        if(particule != null)
        {
            var main = particule.main;

            main.startRotation = particuleRotation;
            particule.transform.position = pos;
            particule.Play();
        }
        
        //smallImpact.startRotation = 
        
        Debug.Log("Yay VFX won't work : " + particuleRotation);
    }
}
