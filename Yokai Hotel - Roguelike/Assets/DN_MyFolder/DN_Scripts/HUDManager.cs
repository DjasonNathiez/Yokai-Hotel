using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HUDManager : MonoBehaviour
{
    [Serializable]
    private class HealthBarUI
    {
        public string name;
        public Image image;
        public int healthValue;
    }

    [SerializeField] HealthBarUI[] healthBarUI;

    PlayerValues playerValues;
    TextMeshProUGUI goldText;

    public float healthBarCount = 0;
    public float healthBarToHeal = 0;
    public float healthActualState = -1;
    public GameObject healthPVBar;
    public GameObject PVPrefab;
    public GameObject ParentPV;
    public float spaceValue;
    public Texture pvDeadTexture;
    public Texture pvLifeTexture;

    bool loadingPV = true;
    public bool healing;
    public bool healActive;

    private void Start()
    {
        goldText = GameObject.FindGameObjectWithTag("GoldUI").GetComponent<TextMeshProUGUI>();
        playerValues = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerValues>();

    }
    private void Update()
    {
        Health();
        Gold();

    }

    void Health() //update the health bar
    {



        if (healthBarCount < playerValues.maxHealth && loadingPV == true) //initialize pv bar
        {
            GameObject newPVBar = Instantiate(PVPrefab, new Vector3(healthPVBar.transform.position.x, healthPVBar.transform.position.y - (spaceValue * healthBarCount), 0), Quaternion.identity);
            newPVBar.transform.SetParent(ParentPV.transform);
            newPVBar.name = "PV Bar" + healthBarCount.ToString();
            newPVBar.tag = "healthBAR" + healthBarCount.ToString();
            newPVBar.transform.localScale = new Vector3(3, 3);

            playerValues.currentHealth += 1;

            healthBarCount += 1;
            healthActualState += 1;
            healthBarToHeal = healthBarCount;

            if(healthBarCount == playerValues.maxHealth)
            {
                loadingPV = false;
            }

            Debug.Log("Heal 1");
        }

        else if (healing == true && healthBarCount < playerValues.maxHealth)
        {
            GameObject newPVBar = Instantiate(PVPrefab, new Vector3(healthPVBar.transform.position.x, healthPVBar.transform.position.y - (spaceValue * healthBarCount), 0), Quaternion.identity);
            newPVBar.transform.SetParent(ParentPV.transform);
            newPVBar.name = "PV Bar" + healthBarCount.ToString();
            newPVBar.tag = "healthBAR" + healthBarCount.ToString();
            newPVBar.transform.localScale = new Vector3(3, 3);

            RawImage newPVBarTexture = newPVBar.GetComponent<RawImage>();
            newPVBarTexture.texture = pvDeadTexture;
            
            RawImage PVHolder = GameObject.FindGameObjectWithTag("healthBAR" + healthBarToHeal.ToString()).GetComponent<RawImage>();
            PVHolder.texture = pvLifeTexture;

            playerValues.currentHealth += 1;

            healthBarCount += 1;
            healthActualState += 1;
            healthBarToHeal += 1;

            healing = false;

            Debug.Log("Heal 2");
        }

        if (Input.GetKeyDown("space")) //loose a hp
        {
            RawImage PVHolder = GameObject.FindGameObjectWithTag("healthBAR" + healthActualState.ToString()).GetComponent<RawImage>();
            PVHolder.texture = pvDeadTexture;

            playerValues.currentHealth -= 1;
            healthActualState -= 1;
            healthBarToHeal -= 1;
        }

        if(healActive == true) //regen hp
        {
            RawImage PVHolder = GameObject.FindGameObjectWithTag("healthBAR" + healthBarToHeal.ToString()).GetComponent<RawImage>();
            PVHolder.texture = pvLifeTexture;

            playerValues.currentHealth += 1;

            healthActualState += 1;
            healthBarToHeal += 1;
            healActive = false;
        }
       
    }
    void Gold() //gold counter with limit
    {
        goldText.text = playerValues.currentGold.ToString(); //the text

        if(playerValues.currentGold > playerValues.maxGold) //creat a limit for the text
        {
            goldText.text = "+" + playerValues.maxGold.ToString();
        }
    }
}
