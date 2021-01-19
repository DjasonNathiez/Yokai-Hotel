using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantManager : MonoBehaviour
{
    public PlayerController player;
    public InventoryManager inventory;
    public AttackEffect attackEffect;

    [Header("Enchant")]
    public List<Enchant> enchants;
    public int maxEnchant;
    [HideInInspector]
    public int chooseIndex;
    public bool choosing ;
    float chooseDelay = 2;
    float chooseDuration;

    [Header("Boost Cond")]
    public bool lightAttack;
    public bool heavyAttack;
    public bool dash;

    public float healthValue;
    //public float moneyValue;

   

    [Header("Boost")]
    public float lightBoost = 1;
    public float heavyBoost = 1;
    public float attackBoost = 1;

    public float theftLifeProba;

    public float dropBoost;
    public float moneyBoost;
    public float moneyReduc;
    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
        inventory = GetComponentInParent<InventoryManager>();

        attackEffect = GetComponent<AttackEffect>();
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
        float boostG = 1;

        float boostDrop = 1;
        float reducMoney = 1;
        float boostTheftHealth = 0;

        
        foreach (Enchant e in enchants)
        {
            for (int i = 0; i < e.enchantEffects.Length; i++)
            {
                
                EnchantEffect currentEnchant = e.enchantEffects[i];

                // check match conditions
                switch (currentEnchant.conditionType)
                {
                    case EnchantEffect.ConditionType.LIGHT_ATTACK:
                        {
                            #region Active Condition
                            if ( player.lightAttack && e.enchantEffects[i].duration < 0 && e.enchantEffects[i].effectDuration > 0)
                            {
                                e.enchantEffects[i].duration = currentEnchant.effectDuration;
                                e.enchantEffects[i].active = true;
                            }

                            if (e.enchantEffects[i].effectDuration > 0)
                                e.enchantEffects[i].duration -= Time.deltaTime;

                            if (e.enchantEffects[i].duration < 0)
                                e.enchantEffects[i].active = false;
                            #endregion

                            break;
                        }
                    case EnchantEffect.ConditionType.HEAVY_ATTACK:
                        {
                            #region Active Condition
                            if (player.heavyAttack && e.enchantEffects[i].duration < 0 && e.enchantEffects[i].effectDuration > 0)
                            {
                                e.enchantEffects[i].duration = currentEnchant.effectDuration;
                                e.enchantEffects[i].active = true;
                            }

                            if (e.enchantEffects[i].effectDuration > 0)
                                e.enchantEffects[i].duration -= Time.deltaTime;

                            if (e.enchantEffects[i].duration < 0)
                                e.enchantEffects[i].active = false;
                            #endregion

                            break;
                        }
                    case EnchantEffect.ConditionType.DASH:
                        {
                            
                            #region Active Condition
                            if (((int)player.playerState == 1) && e.enchantEffects[i].duration < 0 && e.enchantEffects[i].effectDuration > 0)
                            {
                                e.enchantEffects[i].duration = currentEnchant.effectDuration;
                                e.enchantEffects[i].active = true;
                            }

                            if(e.enchantEffects[i].effectDuration > 0)
                                e.enchantEffects[i].duration -= Time.deltaTime;

                            if (e.enchantEffects[i].duration < 0)
                                e.enchantEffects[i].active = false;

                            
                            #endregion

                            break;
                        }
                    case EnchantEffect.ConditionType.HEALTH:
                        {
                            healthValue = player.maxHealth - player.health;
                            e.enchantEffects[i].active = (healthValue > 0);

                            break;
                        }
                    case EnchantEffect.ConditionType.MONEY:
                        {
                            e.enchantEffects[i].active = (inventory.money >= e.enchantEffects[i].conditionValues[1].value);
                            break;
                        }
                    case EnchantEffect.ConditionType.LIGHT_KILL:
                        {
                            #region Active Condition
                            if (attackEffect.kill_L && e.enchantEffects[i].duration < 0 && e.enchantEffects[i].effectDuration > 0)
                            {
                                e.enchantEffects[i].duration = currentEnchant.effectDuration;
                                e.enchantEffects[i].active = true;
                            }

                            if (e.enchantEffects[i].effectDuration > 0)
                                e.enchantEffects[i].duration -= Time.deltaTime;

                            if (e.enchantEffects[i].duration < 0)
                                e.enchantEffects[i].active = false;
                            #endregion

                            break;
                        }
                    case EnchantEffect.ConditionType.HEAVY_KILL:
                        {
                            #region Active Condition
                            if (attackEffect.kill_H && e.enchantEffects[i].duration < 0 && e.enchantEffects[i].effectDuration > 0)
                            {
                                e.enchantEffects[i].duration = currentEnchant.effectDuration;
                                e.enchantEffects[i].active = true;
                            }

                            if (e.enchantEffects[i].effectDuration > 0)
                                e.enchantEffects[i].duration -= Time.deltaTime;

                            if (e.enchantEffects[i].duration < 0)
                                e.enchantEffects[i].active = false;
                            #endregion

                            break;
                        }
                    case EnchantEffect.ConditionType.HIT:
                        {
                            #region Active Condition
                            if (player.isHurt && e.enchantEffects[i].duration < 0 && e.enchantEffects[i].effectDuration > 0)
                            {
                                e.enchantEffects[i].duration = currentEnchant.effectDuration;
                                e.enchantEffects[i].active = true;
                            }

                            if (e.enchantEffects[i].effectDuration > 0)
                                e.enchantEffects[i].duration -= Time.deltaTime;

                            if (e.enchantEffects[i].duration < 0)
                                e.enchantEffects[i].active = false;
                            #endregion
                            break;
                        }
                    case EnchantEffect.ConditionType.FULL_HEALTH:
                        {
                            e.enchantEffects[i].active = (player.maxHealth == player.health);
                            break;
                        }
                    case EnchantEffect.ConditionType.MIN_HEALTH:
                        {
                            e.enchantEffects[i].active = (player.health == 1);
                            break;
                        }

                    case EnchantEffect.ConditionType.NO_HIT:
                        {
                            #region Active Condition
                            if (player.isHurt && e.enchantEffects[i].duration < 0 && e.enchantEffects[i].effectDuration > 0)
                            {
                                e.enchantEffects[i].duration = currentEnchant.effectDuration;
                                e.enchantEffects[i].active = false;
                            }

                            if (e.enchantEffects[i].effectDuration > 0)
                                e.enchantEffects[i].duration -= Time.deltaTime;

                            if (e.enchantEffects[i].duration < 0)
                                e.enchantEffects[i].active = true;
                            #endregion
                            break;
                        }
                }

                // apply effect cond
                if (e.enchantEffects[i].active)
                {
                    switch (currentEnchant.effectType)
                    {
                        case EnchantEffect.EffectType.BOOST_LIGHT:
                            {
                                boostL *= currentEnchant.boostValue;
                                break;
                            }
                        case EnchantEffect.EffectType.BOOST_HEAVY:
                            {
                                boostH *= currentEnchant.boostValue;
                                break;
                            }
                        case EnchantEffect.EffectType.BOOST_ATTACK:
                            {
                                boostG *= currentEnchant.boostValue;
                                break;
                            }
                        case EnchantEffect.EffectType.BOOST_DROP:
                            {
                                boostDrop *= currentEnchant.boostValue;
                                break;
                            }
                        case EnchantEffect.EffectType.THEFT_HEALTH:
                            {
                                boostTheftHealth = currentEnchant.boostValue;
                                break;
                            }
                        case EnchantEffect.EffectType.PRICE:
                            {
                                reducMoney = currentEnchant.boostValue * (((int)currentEnchant.conditionType == 4)? healthValue : 1);
                                break;
                            }

                    }
                }
                
                //bool lightAttack = (player.lightAttack && (int)currentEnchant.conditionType == 0);
                //bool heavyAttack = (player.heavyAttack && (int)currentEnchant.conditionType == 1);
                bool shootAttack = (player.shootAttack && (int)currentEnchant.conditionType == 2);
                bool effectCond = (lightAttack || heavyAttack || shootAttack || dash);

                // apply duration 
                //if (effectCond && e.enchantEffects[i].duration < 0)
                    //e.enchantEffects[i].duration = currentEnchant.effectDuration;

                // decrease duration by time & apply boost effect
                /*if (e.enchantEffects[i].duration >= 0)
                {
                    if ((int)currentEnchant.effectType == 0)
                        boostL *= currentEnchant.boostValue;

                    if ((int)currentEnchant.effectType == 1)
                        boostH *= currentEnchant.boostValue;

                    if ((int)currentEnchant.effectType == 2)
                        boostS *= currentEnchant.boostValue;

                    e.enchantEffects[i].duration -= Time.deltaTime;
                }*/
            }
        }

        lightBoost = attackEffect.lightBoost = boostL;
        heavyBoost = attackEffect.heavyBoost = boostH;
        attackBoost = attackEffect.attackBoost = boostG;

        dropBoost = attackEffect.dropBoost = boostDrop;
        theftLifeProba = attackEffect.theftLifeProba = boostTheftHealth;

        moneyReduc = reducMoney;
    }
   
    
    public void AddEnchant(Enchant enchant)
    {
        if(enchant != null)
        {
            if (enchants.Count < maxEnchant)
                enchants.Add(enchant);
            else
            {
                enchants.RemoveAt(chooseIndex);
                enchants.Add(enchant);
            }
        }
    }
    public void ChooseEnchant()
    {
        if (enchants.Count < maxEnchant)
        {
            chooseIndex = enchants.Count;
            
        }
          

        else if (chooseDuration < 0)
        {
            chooseIndex = (chooseIndex + 1) % maxEnchant;
            chooseDuration = chooseDelay;
           
        }

        chooseDuration -= Time.deltaTime;
    }

}


