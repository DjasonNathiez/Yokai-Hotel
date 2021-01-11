using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LAC_EnchantManager : MonoBehaviour
{
    public PlayerController player;

    public List<Enchant> enchants;
    public float lightBoost = 1, heavyBoost = 1, shootBoost = 1;
    private void Update()
    {
        float boostL = 1;
        float boostH = 1;
        float boostS = 1;

        foreach(Enchant e in enchants)
        {
           for(int i = 0; i < e.enchantEffects.Length; i++)
            {
                EnchantEffect currentEnchant = e.enchantEffects[i];
                

                // check match conditions
                bool lightAttack = (player.lightAttack && (int)currentEnchant.actionType == 0);
                bool heavyAttack = (player.heavyAttack && (int)currentEnchant.actionType == 1);
                bool shootAttack = (player.shootAttack && (int)currentEnchant.actionType == 2);
                bool dash = player.dash && (int)currentEnchant.actionType == 3;

                bool effectCond = (lightAttack || heavyAttack || shootAttack || dash);

                // apply duration 
                if ( effectCond && e.enchantEffects[i].duration < 0)
                    e.enchantEffects[i].duration = currentEnchant.effectDuration;

                // decrease duration by time & apply boost effect
                if (e.enchantEffects[i].duration >= 0)
                {
                    if ((int)currentEnchant.effectType == 0)
                        boostL *= currentEnchant.boostValue;

                    if ((int)currentEnchant.effectType == 1)
                        boostH *= currentEnchant.boostValue;

                    if ((int)currentEnchant.effectType == 2)
                        boostS *= currentEnchant.boostValue;

                    e.enchantEffects[i].duration -= Time.deltaTime;
                }
                    
           }
        }

        lightBoost = boostL;
        heavyBoost = boostH;
        shootBoost = boostS;
    }
}


