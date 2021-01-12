using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantManager : MonoBehaviour
{
    public PlayerController player;

    public List<Enchant> enchants;
    public int maxEnchant;
    public int chooseIndex;
    public float chooseDelay;
    float chooseDuration;

    [Header("Boost")]
    public float lightBoost = 1;
    public float heavyBoost = 1, shootBoost = 1;
    private void Start()
    {
        foreach(Enchant e in enchants)
        {
            Debug.Log(e.GenerateDescription());
        }
    }
    private void Update()
    {
        UpdateBoost(ref enchants);
        ChooseEnchant();
    }

    public void UpdateBoost( ref List<Enchant> enchants)
    {
        float boostL = 1;
        float boostH = 1;
        float boostS = 1;

        foreach (Enchant e in enchants)
        {
            for (int i = 0; i < e.enchantEffects.Length; i++)
            {
                EnchantEffect currentEnchant = e.enchantEffects[i];

                // check match conditions
                bool lightAttack = (player.lightAttack && (int)currentEnchant.actionType == 0);
                bool heavyAttack = (player.heavyAttack && (int)currentEnchant.actionType == 1);
                bool shootAttack = (player.shootAttack && (int)currentEnchant.actionType == 2);
                bool dash = player.dash && (int)currentEnchant.actionType == 3;

                bool effectCond = (lightAttack || heavyAttack || shootAttack || dash);

                // apply duration 
                if (effectCond && e.enchantEffects[i].duration < 0)
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

    public void AddEnchant(ref List<Enchant> enchants, Enchant enchant)
    {
        if (enchants.Count < maxEnchant)
            enchants.Add(enchant);
        else
        {
            enchants.RemoveAt(chooseIndex);
            enchants.Add(enchant);
        }
    }
    public void ChooseEnchant()
    {
        if (enchants.Count < maxEnchant)
            chooseIndex = enchants.Count;

        else if (chooseDuration < 0)
        {
            chooseIndex = (chooseIndex + 1) % maxEnchant;
            chooseDuration = chooseDelay;
        }

        chooseDuration -= Time.deltaTime;
    }

}


