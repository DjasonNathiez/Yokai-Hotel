using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniamtorBoss : MonoBehaviour
{
    BossBehaviour boss;
    Animator animator;

    AudioManager audioM;
    public enum AnimatorState { FREE, ATTACK, DASH, SHOOT };
    public AnimatorState animState = AnimatorState.FREE;

    public int attackDamage = 1;
    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponentInParent<BossBehaviour>();
        animator = GetComponent<Animator>();

        audioM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimatorState();
        UpdateAniamtorValue();
    }
    void UpdateAnimatorState()
    {
        if ((int)boss.bossState == 0)
            animState = AnimatorState.FREE;

        if ((int)boss.bossState == 1)
            animState = AnimatorState.ATTACK;

        if ((int)boss.bossState == 2)
            animState = AnimatorState.DASH;

        if ((int)boss.bossState == 3)
            animState = AnimatorState.SHOOT;
    }
    void UpdateAniamtorValue()
    {
        animator.SetInteger("BossState", (int)animState);
        UpdateBlendTree(boss.orient);
    }
    public void StartDash()
    {
        boss.dashVelocity = boss.dashSpeed;
    }
    public void EndDash()
    {
        boss.dashVelocity = 0;
    }

    public void Shoot()
    {
        boss.Shoot();
    }
    public void ReturnFreeState()
    {
        boss.bossState = BossBehaviour.BossState.FREE;
    }

    public void PlayVFXAttack()
    {
        boss.PlayVFXAttack();
    }
    void UpdateBlendTree(Vector2 velocity)
    {
        bool vertical = (Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x));

        animator.SetFloat("Vertical", (vertical) ? - Mathf.Sign(velocity.y) : 0);
        animator.SetFloat("Horizontal", (vertical) ? 0 : - Mathf.Sign(velocity.x));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PHurtBox")
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if (player)
            {
                player.hurtDamage = attackDamage;
                Debug.Log("Boss hurt player");
            }
        }
    }

    public void DashAttackSound()
    {
        if (audioM)
            audioM.PlaySound("Boss dash attack", 0);
    }

    public void DistanceAttackSound()
    {
        if (audioM)
            audioM.PlaySound("Boss distance attack", 0);
    }

    public void HeavyAttackSound()
    {
        if (audioM)
            audioM.PlaySound("Boss heavy attack", 0);
    }
}
