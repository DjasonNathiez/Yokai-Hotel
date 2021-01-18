﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniamtorBoss : MonoBehaviour
{
    BossBehaviour boss;
    Animator animator;
    public enum AnimatorState { FREE, ATTACK, DASH, SHOOT };
    public AnimatorState animState = AnimatorState.FREE;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponentInParent<BossBehaviour>();
        animator = GetComponent<Animator>();
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
    public void ReturnFreeState()
    {
        boss.bossState = BossBehaviour.BossState.FREE;
    }
    void UpdateBlendTree(Vector2 velocity)
    {
        bool vertical = (Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x));

        animator.SetFloat("Vertical", (vertical) ? - Mathf.Sign(velocity.y) : 0);
        animator.SetFloat("Horizontal", (vertical) ? 0 : - Mathf.Sign(velocity.x));
    }
}
