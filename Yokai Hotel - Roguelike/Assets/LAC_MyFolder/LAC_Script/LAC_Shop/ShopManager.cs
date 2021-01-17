using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public PlayerController playerC;
    public EnchantManager enchantM;
    public InventoryManager inventoryM;

    [Header("Floor")]
    public int floorLevel;
    public float priceMultiplier;

    [Header("Enchant")]
    public List<Enchant> enchantList;
    List<Enchant> exceptEnchants;
    public Enchant[] shopEnchants;
    

    [Header("Health")]
    public int healValue;
    int maxHealValue;
    public float healPrice;
    public float maxHealPrice;

    public Sprite[] healSprite;

    [Header("Buy")]
    public BuyDetector[] buyDetectors;
    public int[] price = new int[3];

    [Header("Show")]
    public SpriteRenderer[] shopItemS;
    public KF_ShopInteract[] shopInteracts;

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

        UpdatePrice();

        // get detector
        buyDetectors = GetComponentsInChildren<BuyDetector>();
        shopInteracts = GetComponentsInChildren<KF_ShopInteract>();

        // update shop inte
    }

    // Update is called once per frame
    void Update()
    {
        BuyProcess(buyDetectors);
        UpdateVisual();
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
                    if (i < 2)
                    {
                        enchantM.ChooseEnchant();
                        Debug.Log("Choose Enchant");
                    }
                    if (shopInteracts[i] != null)
                        shopInteracts[i].showState = true;

                    trigger = true;
                }
                else if (shopInteracts[i] != null)
                    shopInteracts[i].showState = false ;

                if (buyDetectors[i].Buy )
                {
                    if (inventoryM.money >= price[i])
                    {
                        Buy(i);
                        buyDetectors[i].Buy = false;
                        buyDetectors[i].enabled = false;

                        if (shopInteracts[i] != null)
                            shopInteracts[i].showState = false;
                    }
                    else
                        Debug.Log("need more money");
                }
            }
        }
    }
    void Buy(int buyIndex)
    {
        if(buyIndex < 2)
        {
            // buy enchant
            enchantM.AddEnchant(shopEnchants[buyIndex]);
            shopEnchants[buyIndex] = null;

            inventoryM.money -= price[buyIndex];
            Debug.Log("Buy Enchant");
        }
        else
        {
            playerC.ChangeHealth(healValue);
            healValue = 0;

            Debug.Log("Buy Heal");
        }
    } 
    void UpdatePrice()
    {
        for(int i = 0; i < 2; i++)
        {
            if (shopEnchants[i]!= null)
            {
                price[i] = (int)(shopEnchants[i].price * Mathf.Pow(priceMultiplier, floorLevel) * enchantM.moneyReduc);
            }    
        }
        price[2] = (int)((healPrice * healValue) *Mathf.Pow(priceMultiplier, floorLevel) *enchantM.moneyReduc);
    }
    void UpdateVisual()
    {
        // show enchants
        shopItemS[0].sprite = (shopEnchants[0] != null)? shopEnchants[0].icon : null;
        shopItemS[1].sprite = (shopEnchants[1] != null) ? shopEnchants[1].icon : null;

        // show heal
        shopItemS[2].sprite = (healValue > 0) ? healSprite[healValue - 1] : null;

        UpdateShopInteractVisual();
    }

    void UpdateShopInteractVisual()
    {
        for(int i = 0; i < shopInteracts.Length; i++)
        {
            if(i < 2) // detect enchant
            {
                if(shopEnchants[i] != null)
                {
                    shopInteracts[i].objectToBuy.nameObj = shopEnchants[i].name.Split('?');
                    shopInteracts[i].objectToBuy.description = shopEnchants[i].GenerateDescription().Split('?');
                    shopInteracts[i].objectToBuy.price = price[i].ToString().Split('?');
                }
                

            }
            else
            {
                string healName = "Heal stock of : " + healValue;
                string healDesc = " Heal " + healValue + " points no more, no less";
                shopInteracts[i].objectToBuy.nameObj = healName.Split('?');
                shopInteracts[i].objectToBuy.description = healDesc.Split('?');
                shopInteracts[i].objectToBuy.price = price[i].ToString().Split('?');
            }

            
        }
    }
}
