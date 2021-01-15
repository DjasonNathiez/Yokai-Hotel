using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public PlayerController playerC;
    public EnchantManager enchantM;
    public InventoryManager inventoryM;

    [Header("Floor")]
    public int floor;
    public float priceMultiplier;

    [Header("Enchant")]
    public List<Enchant> enchantList;
    List<Enchant> exceptEnchants;
    public Enchant[] shopEnchants;
    public float[] enchantPrice;

    [Header("Health")]
    public int healValue;
    int maxHealValue;
    public float healPrice;
    public float maxHealPrice;

    [Header("Buy")]
    public BuyDetector[] buyDetectors;

    // Start is called before the first frame update
    void Start()
    {
        // get player setting
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
        {
            playerC = playerObj.GetComponent<PlayerController>();
            enchantM = playerObj.GetComponentInChildren<EnchantManager>();
            exceptEnchants = enchantM.enchants;
            inventoryM = playerObj.GetComponent<InventoryManager>();
        }

        // Generate shop value
        SelectShopEnchant(enchantList, exceptEnchants, ref shopEnchants);
        SelectHealValues(ref healValue);

        // get detector
        buyDetectors = GetComponentsInChildren<BuyDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        BuyProcess(buyDetectors);
    }

    void SelectShopEnchant(List<Enchant> baseEnchant, List<Enchant> removeEnchant, ref Enchant[] shopEnchant)
    {
        // remove enchant
        List<Enchant> selectList = baseEnchant;
        foreach(Enchant re in removeEnchant)
        {
            selectList.Remove(re);
        }

        // choose enchant from the select list
        for(int i = 0; i < shopEnchant.Length; i++)
        {
            if(selectList.Count > 0)
            {
                int chooseIndex = Random.Range(0, selectList.Count);
                shopEnchant[i] = selectList[chooseIndex];
                selectList.Remove(selectList[chooseIndex]);
            }
        }
    }
    void SelectHealValues(ref int healValue)
    {
        // defin heal value
        int chooseHealth = Random.Range(0,7);
        if (chooseHealth > 5)
            healValue = 3;
        else if (chooseHealth > 3)
            healValue = 2;
        else
            healValue = 1;
    }

    void BuyProcess( BuyDetector[] buyDetectors)
    {
        bool trigger = false;
        for(int i = 0; i < buyDetectors.Length; i++)
        {
            if(buyDetectors[i] != null)
            {
                if (buyDetectors[i].onTrigger && !trigger)
                {
                    // display something
                    if (i < 3)
                    {
                        enchantM.ChooseEnchant();
                        Debug.Log("Choose Enchant");
                    }
                    trigger = true;
                }
                if (buyDetectors[i].Buy)
                {
                    Buy(i);
                    buyDetectors[i].Buy = false;
                    buyDetectors[i].enabled = false;
                }
            }
        }
    }
    void Buy(int buyIndex)
    {
        if(buyIndex < 3)
        {
            // buy enchant
            enchantM.AddEnchant(shopEnchants[buyIndex]);
            shopEnchants[buyIndex] = null;

            Debug.Log("Buy Enchant");
        }
        else
        {
            // buy else
        }
    }
}
