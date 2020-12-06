using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public PlayerController player;
    AttackManager attackM;
    Animator animator;
    public BoxCollider2D col2D;

    public LayerMask detectMask;
    public GameObject bulletPrefab;
   


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        attackM = GetComponent<AttackManager>();
        col2D = GetComponent<BoxCollider2D>();
       
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
        if(collider.tag == "Ennemi" && player.attackChoose != -1)
        {

            EnnemiBehaviour ennemi = collider.GetComponentInParent<EnnemiBehaviour>();
            if (ennemi)
            {
                Debug.Log("Hit ennemy");
                Vector2 repulseDir = (ennemi.transform.position - player.transform.position).normalized;

                ennemi.healthDamage = attackM.attack[player.attackChoose].damage;

                ennemi.inertnessModifier = attackM.attack[player.attackChoose].knockBackModifier;
                ennemi.repulseForce = attackM.attack[player.attackChoose].knockBackValue * repulseDir;
            }
        }

        if (collider.tag == "BulletEnemy" && player.attackChoose == 3)
        {
            Bullet bullet = collider.GetComponentInParent<Bullet>();

            if (bullet && bullet.tag == "BulletEnemy")
            {
                Debug.Log("hitBullet");
                bullet.speed = -2*bullet.speed;
                bullet.tag = "BulletAlly";
            }
            
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
}
