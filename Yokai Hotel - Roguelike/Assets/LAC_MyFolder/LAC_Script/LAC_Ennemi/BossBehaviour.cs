using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnnemiBehaviour
{
    public enum BossState {P1,P2,P3,P4,AIM,DIE,TRANSITION };
    public BossState bossState;

    public Vector2 orient = new Vector2(0, 1);
    public bool moving = false;

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
            case BossState.P1:
                {
                    Displacement(5, -orient, speed);
                    break;
                }
            case BossState.P2:
                {
                    
                    break;
                }
            case BossState.P3:
                {
                    
                    break;
                }
            case BossState.P4:
                {
                    
                    break;
                }

        }
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
        //moving = false;
    }

}
