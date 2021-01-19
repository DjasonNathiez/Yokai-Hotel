using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnnemiBehaviour
{
    public enum BossState {FREE,ATTACK,DASH,SHOOT};
    public BossState bossState = BossState.SHOOT;

    public Vector2 orient = new Vector2(1, 0);
    Vector2 velocitySmoothing;

    public bool moving = false;
    public float dashDistance;
    public float dashSpeed;
    [HideInInspector]
    public float dashVelocity;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        switch (bossState)
        {
            case BossState.FREE:
                {
                    //Displacement(5, -orient, speed);
                    break;
                }
            case BossState.ATTACK:
                {
                    
                    break;
                }
            case BossState.DASH:
                {
                    Vector2 targetVelocity = -orient.normalized * dashVelocity;
                    velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity.x, ref velocitySmoothing.x,
                                                 (Mathf.Abs(velocity.x) <= Mathf.Abs(targetVelocity.x)) ? 0 : 0.1f);
                    velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocity.y, ref velocitySmoothing.y,
                                                 (Mathf.Abs(velocity.y) <= Mathf.Abs(targetVelocity.y)) ?0 : 0.1f);
                    break;
                }
            case BossState.SHOOT:
                {
                    
                    break;
                }

        }
    }

    public void DashMove()
    {
        
    }
    void Displacement(float dist, Vector2 dir, float speed)
    {
        if (!moving)
        {
            SetVelocity(dir.normalized * speed, 0f);
            StartCoroutine(MoveDelay(dist/speed));
        }
        moving = true;
    }
    public IEnumerator MoveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        velocity = Vector2.zero;
        moving = false;
    }

}
