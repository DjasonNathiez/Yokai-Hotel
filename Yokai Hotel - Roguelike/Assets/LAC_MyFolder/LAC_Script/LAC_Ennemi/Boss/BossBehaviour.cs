using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnnemiBehaviour
{
    public enum BossState {FREE,ATTACK,DASH,SHOOT};
    public BossState bossState = BossState.SHOOT;
    //public BoxCollider2D hurtBox;
    public GameObject hitParticle;
    [Header("Movement")]
    public Vector2 orient = new Vector2(1, 0);
    Vector2 velocitySmooth;

    public bool moving = false;
    public float dashDistance;
    public float dashSpeed;
    [HideInInspector]
    public float dashVelocity;

    [Header("Shoot")]
    public float bulletAngle;
    public float bounceDelay;
    public float bounceAngle;

    public BossBullet bossBullet;
    public Transform[] firePoints;

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
                    hurtBox.gameObject.SetActive(false);
                    break;
                }
            case BossState.ATTACK:
                {
                    
                    break;
                }
            case BossState.DASH:
                {
                    Vector2 targetVelocity = -orient.normalized * dashVelocity;
                    velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity.x, ref velocitySmooth.x,
                                                 (Mathf.Abs(velocity.x) <= Mathf.Abs(targetVelocity.x)) ? 0 : 0.1f);
                    velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocity.y, ref velocitySmooth.y,
                                                 (Mathf.Abs(velocity.y) <= Mathf.Abs(targetVelocity.y)) ?0 : 0.1f);
                    break;
                }
            case BossState.SHOOT:
                {
                    
                    break;
                }

        }
        if (bossState != BossState.FREE)
            hurtBox.gameObject.SetActive(true);
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

    public void Shoot()
    {

        Vector3 bulletP = transform.position;
        #region define bulletPosition
        if (orient == Vector2.right)
            bulletP = firePoints[0].position;

        if (orient == Vector2.up)
            bulletP = firePoints[1].position;

        if (orient == Vector2.left)
            bulletP = firePoints[2].position;

        if (orient == Vector2.down)
            bulletP = firePoints[3].position;
        #endregion

        bossBullet.transform.position = bulletP;
        bossBullet.SetActiveBullet(true);

        float bulletRad = (Mathf.Atan2(orient.y, orient.x) + Mathf.Deg2Rad * bulletAngle) % (2*Mathf.PI);
        float cBounceAngle = (orient == Vector2.right || orient == Vector2.left) ? -bounceAngle : bounceAngle;
        float bounceRad = (bulletRad + Mathf.Deg2Rad * cBounceAngle) % (2 * Mathf.PI);

        Vector2 bulletDir = new Vector2(Mathf.Cos(bulletRad), Mathf.Sin(bulletRad));
        Vector2 bounceDir = new Vector2(Mathf.Cos(bounceRad), Mathf.Sin(bounceRad));
        bossBullet.MoveProjectile(bulletDir, 3.5f, 0.25f);
        StartCoroutine(Bounce(bounceDir, 0.25f + bounceDelay));
        StartCoroutine(ShootEnd(0.7f + bounceDelay));
    }
    public IEnumerator Bounce( Vector2 dir, float delay)
    {
        yield return new WaitForSeconds(delay);
        bossBullet.MoveProjectile(dir, 5, 0.35f);
    }

    public IEnumerator ShootEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        bossBullet.SetActiveBullet(false);
    }

    public void PlayVFXAttack()
    {
        GameObject particle = Instantiate(hitParticle, transform.position, transform.rotation);
        Destroy(particle, 2);
    }
}
