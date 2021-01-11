using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Enchant", menuName = "ScriptableObjects/EnchantScriptableObject", order = 2)]
public class Enchant : ScriptableObject
{
    public Sprite icon;
    public EnchantEffect[] enchantEffects;
}

[Serializable]
public struct EnchantEffect
{
    
    public enum ActionType { LIGHT_ATTACK, HEAVY_ATTACK, SHOOT_ATTACK, DASH};
    public ActionType actionType;

    public enum EffectType { BOOST_LIGHT, BOOST_HEAVY, BOOST_SHOOT};
    public EffectType effectType;

    public float boostValue;
    public float effectDuration;

    public float duration;


}
