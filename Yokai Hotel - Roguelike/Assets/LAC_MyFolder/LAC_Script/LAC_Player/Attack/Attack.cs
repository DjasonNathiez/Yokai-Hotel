using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/AttackScriptableObject", order = 1)]
public class Attack : ScriptableObject
{
    public string clipName;
  
    public enum AttackType { LIGHT, HEAVY, SPECIAL}
    public AttackType attackType;

    public float damage;

    public float inertness,inertnessTime;

    [Header("KnockBack")]
    public float knockBackValue; 
    public float knockBackModifier;
    public float stunTime;

    [Header("Screen")]
    public float screenShakeAmp;
    public float screenShakeFreq; 
    public float screenShakeTime;



}
