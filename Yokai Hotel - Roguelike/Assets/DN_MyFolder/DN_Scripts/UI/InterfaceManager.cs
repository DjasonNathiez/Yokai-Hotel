﻿using System.Collections;
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
    AudioManager audioM;
    public ProceduralGenerator kActive;
    public KF_LevelManager levelM;
    public Slider fillSlider;

    [Header("Health")]
    public RawImage[] allTextureLantern;
    public Texture hpActivate;
    public Texture hpDown;
    public GameObject hpBar;
    public GameObject lightHP;
    public RawImage[] lightIMG;
    public Material lightUP;
    public Material LightDOWN;

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

    public bool oscilate;
    public float oscilMag, oscilFreq;
    float randomOscil;

    [Header("Enchant")]
    public RawImage[] enchantList;
    public Image[] enchantEffect;
    public GameObject enchantEffectIMG;
    GameObject enchantIMG;
    EnchantManager enchantM;

    public GameObject enchantI;
    TextMeshProUGUI[] information;
    RawImage[] informationIcon;

    [Header("Boss HP")]
    public Slider healthBossSlider;
    public BossManager bossM;
    public GameObject healthBarBossObj;
    int bossCurrentHP;
    public int bossLevelIndex;
    bool triggerPhaseTwo, endTrigger;
    public Image fillHp;


    private void Awake()
    {
        randomOscil = Random.value - 0.5f;
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        audioM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();

        keysObj = GameObject.FindGameObjectWithTag("Keys");
        enchantIMG = GameObject.FindGameObjectWithTag("EnchantIMG");

        if (playerObj)
        inventory = playerObj.GetComponent<InventoryManager>(); //initialization du script InventoryManager

        moneyText = GameObject.FindGameObjectWithTag("MoneyText").GetComponent<TextMeshProUGUI>();

        if (playerObj)
        player = playerObj.GetComponent<PlayerController>();

        enchantM = playerObj.GetComponentInChildren<EnchantManager>();

        //alltexturelantern
        allTextureLantern = hpBar.GetComponentsInChildren<RawImage>();

        lightIMG = lightHP.GetComponentsInChildren<RawImage>();

        //enchant
        enchantEffect = enchantEffectIMG.GetComponentsInChildren<Image>();
        information = enchantI.GetComponentsInChildren<TextMeshProUGUI>();
        informationIcon = enchantI.GetComponentsInChildren<RawImage>();

        //bossHP

        healthBarBossObj.SetActive(false);

        //initialization HealthBar

        for (int i = 0; i < enchantM.enchants.Count; i++)
        {
            enchantList[i].enabled = false;
            lightIMG[i].enabled = false;
        }

    }

    private void Update()
    {
        if (levelM)
            kActive = levelM.levels[levelM.levelCount].GetComponent<ProceduralGenerator>();

        UpdateHealth();
        UpdateMoney();
        UpdateShoot();
        UpdateKey();
        UpdateEnchant();
        FillHPBoss();
    }

    void UpdateHealth()
    {
        if (inventory)
        {
            currentHealth = inventory.currentHealth;
            maxHealth = inventory.maxHealth;
        }

        for (int i = 0; i < allTextureLantern.Length; i++)
        {

            lightIMG[i].material = (currentHealth > i) ? lightUP : LightDOWN;



            if (i < maxHealth)
            {
                allTextureLantern[i].enabled = true;
                lightIMG[i].enabled = true;

            }

            if (i >= maxHealth)
            {
                allTextureLantern[i].enabled = false;
                lightIMG[i].enabled = false;
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

            if (currentMoney > 999)
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

        for (int i = 0; i < currentKey; i++)
        {
            keysImage[i].enabled = true;

            if (inventory.keys > i)
            {
                keysImage[inventory.keys - 1].texture = keysUp;

                float oscill = Mathf.Sin(Time.time * oscilFreq + randomOscil) * oscilMag;
                keysImage[i].transform.position += Vector3.up * oscill * Time.deltaTime;

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
        for (int i = 0; i < enchantM.enchants.Count; i++)
        {
            enchantList[i].enabled = true;
            enchantList[i].texture = enchantM.enchants[i].icon.texture;
            enchantList[i].color = Color.white;


            if (enchantM.enchants[i].enchantEffects[0].active == true)
            {
                enchantEffect[i].enabled = true;

            }
            else
            {
                enchantEffect[i].enabled = false;
            }

            //chooseIndex
            if (enchantM.choosing == true)
            {
                Color col = Color.white;
                col.a = 0;
                enchantList[enchantM.chooseIndex].color = col;
            }
            else
            {
                enchantList[i].color = Color.white;
            }

            //visual menu information
            information[i].enabled = true;
            informationIcon[i].enabled = true;

            information[i].text = enchantM.enchants[i].GenerateDescription();
            informationIcon[i].texture = enchantM.enchants[i].icon.texture;
            
        }
    }

    void FillHPBoss()
    {
        healthBossSlider.minValue = 0;

        if(levelM.levelCount == bossLevelIndex)
        {
            healthBarBossObj.SetActive(true);
        }

        if (bossM)
        {
            if (bossM.phaseTwo == false)
            {
                healthBossSlider.maxValue = bossM.pTwoHp;
                bossCurrentHP = bossM.currentBossHp - bossM.pTwoHp;
            }
            else if( ! triggerPhaseTwo)
            {
                bossCurrentHP = 0;
                StartCoroutine(FillHPBossLate(10));
                triggerPhaseTwo = true;
            }
            if (bossM.phaseTwo && endTrigger)
            {
                bossCurrentHP = bossM.currentBossHp;
            }
        }

        healthBossSlider.value = bossCurrentHP;
        if (fillHp != null)
        {
            Color col = Color.white;
            col.a = 0;
            fillHp.color = (healthBossSlider.value <= 0) ?col : Color.white;
        }
        

    }

    IEnumerator FillHPBossLate(float delay)
    {
        yield return new WaitForSeconds(delay);
        healthBossSlider.maxValue = bossM.globalBossHp;
        bossCurrentHP = bossM.currentBossHp;
        endTrigger = true;
    }

}
