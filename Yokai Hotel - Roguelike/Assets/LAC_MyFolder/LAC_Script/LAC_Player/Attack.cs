using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/AttackScriptableObject", order = 1)]
public class Attack : ScriptableObject
{
    public string name;

    public enum AttackType { LIGHT, HEAVY, SPECIAL}
    public AttackType attackType;

    public int damage;
    public bool activeDamage;

    public float inertness;
    public int inertnessFrame;

    public bool armorable;
    public int armorTime;

}
