using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Enchant", menuName = "ScriptableObjects/EnchantScriptableObject", order = 2)]
public class Enchant : ScriptableObject
{
    public Sprite icon;
    public EnchantEffect[] enchantEffects;
    public float price;
    //public string description;
    public string GenerateDescription()
    {
        // initialize
        string sentence = "";

        string condition = "";
        string effect = "";

        string[] boostChanges = { "increase ", "decrease " };

        foreach(EnchantEffect e in enchantEffects)
        {
            float boostState = Mathf.Abs(e.boostValue - 1) * 100;
            if (sentence != "") // line return
                sentence += "\n and ";
            string boostChange = (e.boostValue < 1) ? boostChanges[1] : boostChanges[0];
            switch (e.conditionType)
            {
                case EnchantEffect.ConditionType.LIGHT_ATTACK:
                    {
                        condition += " After light attack ";
                        break;
                    }
                case EnchantEffect.ConditionType.HEAVY_ATTACK:
                    {
                        condition += " After heavy attack ";
                        break;
                    }
                case EnchantEffect.ConditionType.DASH:
                    {
                        condition += " After dash ";
                        break;
                    }
                case EnchantEffect.ConditionType.HEALTH:
                    {
                        condition += " for each missing health points ";
                        break;
                    }
                case EnchantEffect.ConditionType.MONEY:
                    {
                        condition += " If money stock is greater than ";
                        break;
                    }
                case EnchantEffect.ConditionType.LIGHT_KILL:
                    {
                        condition += " After a kill by light attack ";

                        break;
                    }
                case EnchantEffect.ConditionType.HEAVY_KILL:
                    {
                        condition += " After a kill by heavy attack ";
                        break;
                    }
                case EnchantEffect.ConditionType.HIT:
                    {
                        condition += " After taking damage ";
                        break;
                    }
                case EnchantEffect.ConditionType.FULL_HEALTH:
                    {
                        condition += " If you are full health ";
                        break;
                    }
                case EnchantEffect.ConditionType.MIN_HEALTH:
                    {
                        condition += " If one health point left ";
                        break;
                    }
            }
            switch (e.effectType)
            {
                case EnchantEffect.EffectType.BOOST_LIGHT:
                    {
                        effect += boostChange + "light attack damage by : " + boostState +" %";
                        break;
                    }
                case EnchantEffect.EffectType.BOOST_HEAVY:
                    {
                        effect += boostChange + "heavy attack damage by : " + boostState + " %";
                        break;
                    }
                case EnchantEffect.EffectType.BOOST_ATTACK:
                    {
                        effect += boostChange + "attack damage by : " + boostState + " %";
                        break;
                    }
                case EnchantEffect.EffectType.BOOST_DROP:
                    {
                        effect += boostChange + "item drop rate by  : " + boostState + " %";
                        break;
                    }
                case EnchantEffect.EffectType.THEFT_HEALTH:
                    {
                       effect += " gain : " + e.boostValue * 100 + " % of chance to heal one health point";
                        break;
                    }
                case EnchantEffect.EffectType.PRICE:
                    {
                        effect += boostChange + " shop price by : " + boostState + " %";
                        break;
                    }

            }

            sentence = condition + effect;
        }
        

        return sentence;
    }

}

[Serializable]
public struct EnchantEffect
{
    public int level;
    public enum ConditionType { LIGHT_ATTACK, HEAVY_ATTACK, SHOOT_ATTACK, DASH, HEALTH, MONEY,LIGHT_KILL,HEAVY_KILL, HIT, FULL_HEALTH , MIN_HEALTH};
    public ConditionType conditionType;
    public BoostValue[] conditionValues;

    public enum EffectType { BOOST_LIGHT, BOOST_HEAVY, BOOST_SHOOT, BOOST_ATTACK, BOOST_DROP, THEFT_HEALTH, PRICE};
    public EffectType effectType;

    public float boostValue;
    public float effectDuration;
    [HideInInspector]
    public float duration;

    public bool active;
}

[Serializable]
public struct BoostValue
{
    public string name;
    public float value;
}


