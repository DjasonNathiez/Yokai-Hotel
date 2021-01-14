using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Enchant", menuName = "ScriptableObjects/EnchantScriptableObject", order = 2)]
public class Enchant : ScriptableObject
{
    public Sprite icon;
    public EnchantEffect[] enchantEffects;

    public string GenerateDescription()
    {
        // initialize
        string sentence = "";

        string[] actions = { "light attack", "heavy attack", "shoot", "dash" };
        string[] boostChange = { "increase", "decrease" };

        foreach(EnchantEffect e in enchantEffects)
        {
            float boostState = Mathf.Abs(e.boostValue - 1) * 100;
            if (sentence != "") // line return
                sentence += "\n and ";

            sentence += "After "+actions[(int)e.conditionType]+": " +((e.boostValue < 1)?boostChange[1] : boostChange[0])+ " " +actions[(int)e.effectType] +" damage by " +boostState+" % " +
                        " during :" + e.effectDuration + " secondes";
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

}

[Serializable]
public struct BoostValue
{
    public string name;
    public float value;
}


