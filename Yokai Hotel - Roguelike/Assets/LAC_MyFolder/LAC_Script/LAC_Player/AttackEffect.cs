using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    PlayerController player;
    AttackManager attackM;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        attackM = GetComponent<AttackManager>();
       
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

    public void ResetAttack()
    {
        player.attackChoose = -1;
    }
    public void Attackable(bool allow)
    {
        player.attackable = allow;
    }
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
