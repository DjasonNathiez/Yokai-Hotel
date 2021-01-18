using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnnemiBehaviour
{
    public enum BossState {FREE,ATTACK,DASH,SHOOT};
    public BossState bossState = BossState.SHOOT;

    public Vector2 orient = new Vector2(1, 0);
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
                    
                    break;
                }
            case BossState.SHOOT:
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
