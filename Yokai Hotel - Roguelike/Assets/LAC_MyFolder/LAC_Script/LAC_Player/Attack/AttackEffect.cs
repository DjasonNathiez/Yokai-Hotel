using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    PlayerController player;
    AttackManager attackM;
    Animator animator;
    public BoxCollider2D col2D;

    public LayerMask detectMask;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        attackM = GetComponent<AttackManager>();
        col2D = GetComponent<BoxCollider2D>();
       
    }
    private void Update()
    {
      
            /*Vector2 maxBound = new Vector2(col2D.bounds.max.x, col2D.bounds.max.y);
            Vector2 minBound = new Vector2(col2D.bounds.min.x, col2D.bounds.min.y);

            Collider2D hit = Physics2D.OverlapArea(maxBound, minBound, detectMask);
            if (hit && col2D.enabled == true)
            {
                if(hit.tag == "Ennemi" && player.attackChoose != -1)
                {
                    EnnemiBehaviour ennemiB = hit.GetComponentInParent<EnnemiBehaviour>();
                    if (ennemiB)
                    {
                        Debug.Log("Hit ennemy");
                        Vector2 repulseDir = (ennemiB.transform.position - player.transform.position).normalized;

                        ennemiB.healthDamage = attackM.attack[player.attackChoose].damage;

                        ennemiB.inertnessModifier = attackM.attack[player.attackChoose].knockBackModifier;
                        ennemiB.repulseForce = attackM.attack[player.attackChoose].knockBackValue * repulseDir;
                    }
                }
            }*/
        
       

    }

    private void OnDrawGizmos()
    {
        Vector2 size = new Vector2(col2D.bounds.max.x - col2D.bounds.min.x, col2D.bounds.max.y - col2D.bounds.min.y);
        //Gizmos.DrawCube(col2D.offset + (Vector2)transform.position, size);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Ennemi" && player.attackChoose != -1)
        {
            BasiqueEnnemiCac ennemi = collider.GetComponent<BasiqueEnnemiCac>();
            EnnemiBehaviour ennemiB = collider.GetComponentInParent<EnnemiBehaviour>();
            if (ennemi)
            {
                Vector2 repulseDir = (ennemi.transform.position - player.transform.position).normalized;

                ennemi.hitDamage = attackM.attack[player.attackChoose].damage;

                ennemi.inertnessModifier = attackM.attack[player.attackChoose].knockBackModifier;
                ennemi.repulseForce = attackM.attack[player.attackChoose].knockBackValue * repulseDir;

            }

            if (ennemiB)
            {
                Debug.Log("Hit ennemy");
                Vector2 repulseDir = (ennemiB.transform.position - player.transform.position).normalized;

                ennemiB.healthDamage = attackM.attack[player.attackChoose].damage;

                ennemiB.inertnessModifier = attackM.attack[player.attackChoose].knockBackModifier;
                ennemiB.repulseForce = attackM.attack[player.attackChoose].knockBackValue * repulseDir;
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
