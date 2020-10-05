using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/AttackScriptableObject", order = 1)]
public class Attack : ScriptableObject
{
    public string clipName;
  
    public enum AttackType { LIGHT, HEAVY, SPECIAL}
    public AttackType attackType;

    public int damage;

    public float inertness, acceleration;
    
    public float knockBackvalue;


}
