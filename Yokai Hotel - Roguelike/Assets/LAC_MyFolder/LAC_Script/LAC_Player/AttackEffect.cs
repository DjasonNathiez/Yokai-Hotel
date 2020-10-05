using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    PlayerController player;
    AttackManager attackM;
    Animator animator;
    BoxCollider2D col2D;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        attackM = GetComponent<AttackManager>();
        col2D = GetComponent<BoxCollider2D>();
       
    }
    private void Update()
    {
        //col2D.offset = player.lastDir.normalized * 10;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Ennemi" && player.attackChoose != -1)
        {
            BasiqueEnnemiCac ennemi = collider.GetComponent<BasiqueEnnemiCac>();
            if (ennemi)
            {
                ennemi.hitDamage = attackM.attack[player.attackChoose].damage;
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
        //SetCombo();
        ResetAttack();
    }
    public void AttackStart()
    {

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
