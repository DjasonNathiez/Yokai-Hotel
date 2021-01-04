using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(Enchantement))]
public class customInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Enchantement enchant = (Enchantement)target;
        if(GUILayout.Button("Generate Enchantement"))
        {
            enchant.name = enchant.enchantementName;
            PrefabUtility.SaveAsPrefabAsset(enchant.gameObject, "Assets/Resources/Enchants/" + enchant.name + ".prefab");
            enchant.name = "EnchantementGenerator";
        }
    }
}

public class Enchantement : MonoBehaviour
{
    public PlayerController playerC;
    public AttackManager attackM;

    [Header("Information")]
    public string enchantementName;
    public string description;
    public Image icon;

    [Header("Caracteristics")]
    public float shopDropRate;
    public float itemCost;
    public enum ValueToChange { DropRate, Damage, Speed, DistanceGaugeValue, Health, ItemCost };
    public ValueToChange valueToChange;
    public enum SecondaryValueToChange { None, DropRate, Damage, Speed, DistGaugeValue, Health, ItemCost };
    SecondaryValueToChange secondaryValueToChange;
    public enum Affected { Player, AllEnnemy, EnnemyA, EnnemyB, EnnemyC, Shop, LightAttack, HeavyAttack, DistanceAttack }
    public Affected affected;
    public int value;
    public enum Condition1 { None, LightAttack, HeavyAttack, DistanceAttack, Dash, Health };
    public Condition1 condition1;
    public float condition1Value;
    public bool condition1State;
    public enum Conditon2 { None, LightAttack, HeavyAttack, DistanceAttack, Dash, Health, State };
    Conditon2 condition2;
    float condition2Value;
    bool condition2State;

    [Header("Cooldown Effect")]
    public bool cooldown;
    public float CDTime;

    public float valueBackup;
    public bool isTrue;
    public bool isActivated = false;

    Enchantement enchant;

    private void Start()
    {
        enchant = GetComponent<Enchantement>();
        playerC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        attackM = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackManager>();

        if (isActivated == true)
        {
            CheckCondition();

            if (isTrue == true)
            {
                StartCoroutine(ChangeTheValue());
            }
        }

    }

    void CheckCondition() ///the action to do for use the enchant
    {


        if(playerC || attackM)
        {
            //if None
            ///the enchant don't need any condition -> passive effect

            if (enchant.condition1 == Enchantement.Condition1.None)
            {
                isTrue = true;
            }


            //if LightAttack
            ///when LightAttack hit something -> bool

            if (enchant.condition1 == Enchantement.Condition1.LightAttack)
            {
                if (enchant.playerC.attackChoose != -1 && enchant.playerC.attackChoose > 3)
                {
                    isTrue = true;
                }
            }

            //if HeavyAttack
            ///when HeavyAttack hit something -> bool

            if (enchant.condition1 == Enchantement.Condition1.HeavyAttack)
            {
                if (enchant.playerC.attackChoose == 3)
                {
                    isTrue = true;
                }
            }

            //if DistanceAttack
            ///when DistanceAttack hit something -> bool

            if (enchant.condition1 == Enchantement.Condition1.DistanceAttack)
            {
                if (enchant.playerC.attackChoose == 4)
                {
                    isTrue = true;
                }
            }

            //if Dash
            ///when dash is used -> bool

            if (enchant.condition1 == Enchantement.Condition1.Dash)
            {
                if (enchant.playerC.playerState == PlayerController.PlayerState.DASH)
                {
                    isTrue = true;
                }
            }

            //if Health
            ///when health is at an special amount -> value

            if (enchant.condition1 == Enchantement.Condition1.Health)
            {
                if (enchant.playerC.health == enchant.value)
                {
                    isTrue = true;
                }
            }
        }
       

    }

    IEnumerator ChangeTheValue() ///the value which will be change by the enchant
    {

            //if ValueToChange = DropRate
            ///Select the ennemy -> value

            if (enchant.valueToChange == Enchantement.ValueToChange.DropRate)
            {
                if (enchant.affected == Enchantement.Affected.Shop)
                {
                    enchant.valueBackup = enchant.shopDropRate;
                    yield return new WaitForEndOfFrame();
                    enchant.shopDropRate += enchant.value;

                    if (enchant.cooldown == true)
                    {
                        yield return new WaitForSeconds(enchant.CDTime);
                        enchant.shopDropRate = enchant.valueBackup;
                    }
                }
            }

            //if ValueToChange = Damage
            ///Select the good attack -> value

            if (enchant.valueToChange == Enchantement.ValueToChange.Damage)
            {
                if (enchant.affected == Enchantement.Affected.LightAttack)
                {
                    enchant.valueBackup = enchant.attackM.attack[1].damage;
                    yield return new WaitForEndOfFrame();
                    enchant.attackM.attack[1].damage += enchant.value;

                    if (enchant.cooldown == true)
                    {
                        yield return new WaitForSeconds(enchant.CDTime);
                        enchant.attackM.attack[1].damage = enchant.valueBackup;
                    }
                }

                if (enchant.affected == Enchantement.Affected.HeavyAttack)
                {
                    enchant.valueBackup = enchant.attackM.attack[3].damage;
                    yield return new WaitForEndOfFrame();
                    enchant.attackM.attack[3].damage += enchant.value;

                    if (enchant.cooldown == true)
                    {
                        yield return new WaitForSeconds(enchant.CDTime);
                        enchant.attackM.attack[3].damage = enchant.valueBackup;
                    }
                }

                if (enchant.affected == Enchantement.Affected.DistanceAttack)
                {
                    enchant.valueBackup = enchant.attackM.attack[4].damage;
                    yield return new WaitForEndOfFrame();
                    enchant.attackM.attack[4].damage += enchant.value;

                    if (enchant.cooldown == true)
                    {
                        yield return new WaitForSeconds(enchant.CDTime);
                        enchant.attackM.attack[4].damage = enchant.valueBackup;
                    }
                }
            }

            //if ValueToChange = Speed
            ///Change into character script -> value
            if (enchant.valueToChange == Enchantement.ValueToChange.Speed)
            {
                if (enchant.affected == Enchantement.Affected.Player)
                {
                    enchant.valueBackup = enchant.playerC.speed;
                    yield return new WaitForEndOfFrame();
                    enchant.playerC.speed += enchant.value;

                    if (enchant.cooldown == true)
                    {
                        yield return new WaitForSeconds(enchant.CDTime);
                        enchant.playerC.speed = enchant.valueBackup;
                    }

                }

                if (enchant.affected == Enchantement.Affected.AllEnnemy)
                {
                    //wait for it
                }

                if (enchant.affected == Enchantement.Affected.EnnemyA)
                {
                    //wait for it
                }

                if (enchant.affected == Enchantement.Affected.EnnemyB)
                {
                    //wait for it
                }

                if (enchant.affected == Enchantement.Affected.EnnemyC)
                {
                    //wait for it
                }
            }

            //if ValueToChange = DistanceGaugeValue
            ///Change into character script -> value

            if (enchant.valueToChange == Enchantement.ValueToChange.DistanceGaugeValue)
            {
                enchant.valueBackup = enchant.playerC.shootGaugeMax;
                yield return new WaitForEndOfFrame();
                enchant.playerC.shootGaugeMax += enchant.value;

                if (enchant.cooldown == true)
                {
                    yield return new WaitForSeconds(enchant.CDTime);
                    enchant.playerC.shootGaugeMax += enchant.valueBackup;
                }
            }

            //if ValueToChange = Health
            ///Change into character script -> value

            if (valueToChange == ValueToChange.Health)
            {
                if (affected == Affected.Player)
                {
                    valueBackup = playerC.health;
                    yield return new WaitForEndOfFrame();
                    playerC.health += value;

                Debug.Log("Add Health Enchant");
                    if (cooldown == true)
                    {
                        yield return new WaitForSeconds(CDTime);
                        playerC.health = enchant.valueBackup;

                    Debug.Log("BackToNormal");
                    }

                }

                if (enchant.affected == Enchantement.Affected.AllEnnemy)
                {
                    //wait for it
                }

                if (enchant.affected == Enchantement.Affected.EnnemyA)
                {
                    //wait for it
                }

                if (enchant.affected == Enchantement.Affected.EnnemyB)
                {
                    //wait for it
                }

                if (enchant.affected == Enchantement.Affected.EnnemyC)
                {
                    //wait for it
                }

           

        }

        //if ValueToChange = ItemCost
        ///Change into self script -> value

        if (enchant.valueToChange == Enchantement.ValueToChange.ItemCost)
            {
                if (enchant.affected == Enchantement.Affected.Shop)
                {
                    enchant.valueBackup = enchant.itemCost;
                    yield return new WaitForEndOfFrame();
                    enchant.itemCost += enchant.value;

                    if (enchant.cooldown == true)
                    {
                        yield return new WaitForSeconds(enchant.CDTime);
                        enchant.itemCost = enchant.valueBackup;
                    }

                }
            }
        }

    }
