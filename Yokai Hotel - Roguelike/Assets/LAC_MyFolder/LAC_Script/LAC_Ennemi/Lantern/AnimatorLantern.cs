using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorLantern : MonoBehaviour
{
    EnnemiLantern ennemi;
    public Animator animator;
    AudioManager audioM;

    public enum AnimatorState { IDLE, AGGRO, ATTACK, WAIT, HURT, DIE }
    public AnimatorState animatorState;

    Vector2 aVelocity;
    // Start is called before the first frame update
    void Start()
    {
        audioM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        ennemi = GetComponentInParent<EnnemiLantern>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PHurtBox"))
        {
            ennemi.player.hurtDamage = ennemi.attackDamage;
            Debug.Log("hitPlyer2");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (ennemi.velocity != Vector2.zero)
            aVelocity = ennemi.velocity;

        UpdateEnnemySate();
        UpdateAnimatorValue();
    }

    void UpdateEnnemySate()
    {
        if ((int)ennemi.ennemyState != 0)
        {
            if ((int)ennemi.ennemyState == 5)
                animatorState = AnimatorState.DIE;

            if ((int)ennemi.ennemyState == 2)
            {
                if(ennemi.aFrqcTimer > ennemi.attackFrequence)
                    animatorState = AnimatorState.ATTACK;
                else
                    animatorState = AnimatorState.IDLE;
            }
                
        }
        else
            animatorState = AnimatorState.IDLE;
    }
    void UpdateAnimatorValue()
    {
        UpdateBlendTree(aVelocity);
        animator.SetInteger("State", (int)animatorState);
    }
    void UpdateBlendTree(Vector2 velocity)
    {
        bool vertical = (Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x));

        animator.SetFloat("Vertical", (vertical) ? Mathf.Sign(velocity.y) : 0);
        animator.SetFloat("Horizontal", (vertical) ? 0 : Mathf.Sign(velocity.x));
    }

    public void EndAttack()
    {
        
        if(ennemi.targetDist > ennemi.maxCacRadius)
            ennemi.ennemyState = EnnemiBehaviour.EnnemyState.IDLE;
        ennemi.aFrqcTimer = 0;
    }

    public void LanternAttackSound()
    {
        audioM.PlaySound("Ennemy lantern attack", 0);
    }

    public void Death()
    {
        ennemi.Death();
    }
}
