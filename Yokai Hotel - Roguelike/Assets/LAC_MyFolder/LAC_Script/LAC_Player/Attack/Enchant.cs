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

            sentence += "After "+actions[(int)e.actionType]+": " +((e.boostValue < 1)?boostChange[1] : boostChange[0])+ " " +actions[(int)e.effectType] +" damage by " +boostState+" % " +
                        " during :" + e.effectDuration + " secondes";
        }

        return sentence;
    }

}

[Serializable]
public struct EnchantEffect
{
    
    public enum ConditionType { LIGHT_ATTACK, HEAVY_ATTACK, SHOOT_ATTACK, DASH, PERMANENT, HEALTH, };
    public ConditionType actionType;
    
    public enum EffectType { BOOST_LIGHT, BOOST_HEAVY, BOOST_SHOOT};
    public EffectType effectType;

    public float boostValue;
    public float effectDuration;

    public float duration;

}


