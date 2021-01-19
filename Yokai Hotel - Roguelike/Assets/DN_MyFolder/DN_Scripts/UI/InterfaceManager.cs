using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    InventoryManager inventory;
    PlayerController player;
    public ProceduralGenerator kActive;
    public KF_LevelManager levelM;
    public Slider fillSlider;

    [Header("Health")]
    public RawImage[] allTextureLantern;
    public Texture hpActivate;
    public Texture hpDown;
    public GameObject hpBar;
    public GameObject[] lightHP;
    /// Texture hpNotActive;

    float currentHealth;
    public float maxHealth;

    float shootValue;

    [Header("Money")]
    TextMeshProUGUI moneyText;
    float currentMoney;

    [Header("Keys")]
    public RawImage[] keysImage;
    public Texture keysMiss;
    public Texture keysUp;
    public GameObject keysObj;
    int currentKey;

    [Header("Enchant")]
    public RawImage[] enchantList;
    GameObject enchantIMG;
    EnchantManager enchantM;

    private void Awake()
    {
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        

        keysObj = GameObject.FindGameObjectWithTag("Keys");
        enchantIMG = GameObject.FindGameObjectWithTag("EnchantIMG");
        
        if(playerObj)
        inventory = playerObj.GetComponent<InventoryManager>(); //initialization du script InventoryManager
     
        moneyText = GameObject.FindGameObjectWithTag("MoneyText").GetComponent<TextMeshProUGUI>();

        if(playerObj)
        player = playerObj.GetComponent<PlayerController>();

        enchantM = playerObj.GetComponentInChildren<EnchantManager>();

        allTextureLantern = hpBar.GetComponentsInChildren<RawImage>();

        lightHP = GameObject.FindGameObjectsWithTag("LightUI");
        

        //initialization HealthBar
        

        for (int i = 0; i < enchantM.enchants.Count; i++)
        {
            enchantList[i].enabled = false;
        }

    }

    private void Update()
    {
        if(levelM)
        kActive = levelM.levels[levelM.levelCount].GetComponent<ProceduralGenerator>();

        UpdateHealth();
        UpdateMoney();
        UpdateShoot();
        UpdateKey();
        UpdateEnchant();
        
    }

    void UpdateHealth()
    {
        if (inventory)
        {
            currentHealth = inventory.currentHealth;
            maxHealth = inventory.maxHealth;
        }

        for(int i = 0; i < allTextureLantern.Length; i++)
        {

            //allTextureLantern[i].texture = (currentHealth > i) ? hpActivate : hpDown;

            if (i < maxHealth)
            {
                allTextureLantern[i].enabled = true;
            }

            if(i >= maxHealth)
            {
                allTextureLantern[i].enabled = false;
            }
        }

    }

    void UpdateMoney()
    {
        if (inventory)
            currentMoney = inventory.money;

        if (moneyText)
        {
            moneyText.text = currentMoney.ToString();
            
            if(currentMoney > 999)
            {
                moneyText.text = "999+";
            }
        }
    }

    void UpdateShoot()
    {
        if (player)
            fillSlider.value = player.shootGaugeState;
    }

    void UpdateKey()
    {
        //de 1 à 3 clé selon le niveau
        /// keys/keysinlvl

        if (kActive)
        currentKey = kActive.keyNumber;
       
        //initialization

        keysImage = keysObj.GetComponentsInChildren<RawImage>();

        for(int i = 0; i < currentKey; i++)
        {
            keysImage[i].enabled = true;

            if(inventory.keys != 0)
            {
                keysImage[inventory.keys -1].texture = keysUp;

            }
            else 
            {
                keysImage[i].texture = keysMiss;

            }
            
        }

    }

    void UpdateEnchant()
    {
        //initialize image

        enchantList = enchantIMG.GetComponentsInChildren<RawImage>();
        for(int i = 0; i < enchantM.enchants.Count; i++)
        {
            enchantList[i].enabled = true;
            enchantList[i].texture = enchantM.enchants[i].icon.texture;
            enchantList[i].color = Color.white;
           

            //if(enchantM.enchants[i].enchantEffects[i].active == true)

            if (enchantM.enchants[i].enchantEffects[0].active == true)
            {
               
                enchantList[i].color = Color.blue;
                
            }

            //chooseIndex
            if(enchantM.choosing == true)
            {
              enchantList[enchantM.chooseIndex].color = Color.red;
            }
        }
    }

   
}
