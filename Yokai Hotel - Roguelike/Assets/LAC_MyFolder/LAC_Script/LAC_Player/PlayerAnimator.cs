using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    PlayerController player;
    Animator animator;

    [Header("Displacement")]
    public float moveSensibility;
    public enum AnimatorState { IDLE, MOVE, DASH, ATTACK};
    public AnimatorState animatorState;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();

        // initialize animator state
        animatorState = AnimatorState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimatorState();
        UpdateAnimatorValue();
    }

    void UpdateAnimatorState()
    {
        if ((int)player.playerState == 0)
        {
            if (player.velocity.magnitude > moveSensibility)
                animatorState = AnimatorState.MOVE;
            else
                animatorState = AnimatorState.IDLE;
        }

        if ((int)player.playerState == 1)
            animatorState = AnimatorState.DASH;

        if ((int)player.playerState == 2)
            animatorState = AnimatorState.ATTACK;

    }
    void UpdateAnimatorValue()
    {
        animator.SetInteger("AnimatorState", (int)animatorState);
        UpdateBlendTree(player.lastDir);
        animator.SetInteger("AttackChoose", player.attackChoose);  
    }
    void UpdateBlendTree(Vector2 velocity)
    {
        bool vertical = (Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x));
        
        animator.SetFloat("Vertical", (vertical) ?  Mathf.Sign(velocity.y) : 0);
        animator.SetFloat("Horizontal", (vertical)? 0 : Mathf.Sign(velocity.x));
    }
}
