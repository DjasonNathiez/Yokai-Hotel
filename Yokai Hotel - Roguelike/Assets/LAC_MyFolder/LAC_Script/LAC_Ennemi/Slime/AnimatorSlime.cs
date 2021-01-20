using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSlime : MonoBehaviour
{
    EnnemiSlime ennemi;
    public Animator animator;

    AudioManager audioM;
    public enum AnimatorState { IDLE, AGGRO, ATTACK, WAIT, HURT, DIE }
    public AnimatorState animatorState;

    Vector2 aVelocity;
    // Start is called before the first frame update
    void Start()
    {
        GameObject audioManager = GameObject.FindGameObjectWithTag("GameManager");
            if(audioManager)
                audioM = audioManager.GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        ennemi = GetComponentInParent<EnnemiSlime>();

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
                animatorState = AnimatorState.ATTACK;

        }
        else
            animatorState = AnimatorState.IDLE;
    }
    void UpdateAnimatorValue()
    {
        if ((int)ennemi.ennemyState == 2)
            UpdateBlendTree(ennemi.targetDir);
        else
            UpdateBlendTree(aVelocity);

        animator.SetInteger("State", (int)animatorState);
    }
    void UpdateBlendTree(Vector2 velocity)
    {
        bool vertical = (Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x));

        animator.SetFloat("Vertical", (vertical) ? Mathf.Sign(velocity.y) : 0);
        animator.SetFloat("Horizontal", (vertical) ? 0 : Mathf.Sign(velocity.x));
    }

    public void ShootEvent()
    {
        ennemi.Shoot();
    }

    public void Death()
    {
        ennemi.Death();
    }

    public void DistanceEnnemyAttack()
    {
        if (audioM)
            audioM.PlaySound("Distance ennemy attack", 0);
    }

    public void DistanceEnnemyDeath()
    {
        if(audioM)
        audioM.PlaySound("Distance ennemy death", 0);
    }
}
