using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public int playSceneNumber;
    public int hubScene;
    public bool paused;

    public GameObject pauseMenu;
    public Canvas HUD;

    public GameObject enchantInformation;
    public bool isOpen;

    PlayerController playerController;
    public AudioManager audioM;

    private void Awake()
    {
        enchantInformation = GameObject.FindGameObjectWithTag("EnchantInfo");

        GameObject hud = GameObject.FindGameObjectWithTag("HUD");
        if (hud)
            HUD = hud.GetComponent<Canvas>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
            playerController = player.GetComponent<PlayerController>();



    }
    public void Start()
    {
        paused = false;
        isOpen = false;
        enchantInformation.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetButtonDown("Cancel") && paused == false)
        {
            Pause();

            if (audioM)
                audioM.PlaySound("UI open menu", 0);

        }
        else if (Input.GetButtonDown("Cancel") && paused == true)
        {
            Resume();

            if (audioM)
                audioM.PlaySound("UI close menu", 0);
        }


        if (Input.GetButtonDown("Information") && isOpen == false)
        {

            enchantInformation.SetActive(true);
            isOpen = true;

            if (audioM)
                audioM.PlaySound("UI open menu", 0);
        }
        else if (Input.GetButtonDown("Information") && isOpen == true)
        {
            enchantInformation.SetActive(false);
            isOpen = false;

            if (audioM)
                audioM.PlaySound("UI close menu", 0);
        }


        if (pauseMenu)
        {

            if (HUD && paused == true)
            {
                HUD.enabled = false;
            }
            else if (HUD && paused == false)
            {
                HUD.enabled = true;
            }
        }

    }
    public void Play()
    {
        SceneManager.LoadScene(playSceneNumber);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToHub()
    {
        SceneManager.LoadScene(hubScene);
        Time.timeScale = 1;
    }

    public void Pause()
    {

        pauseMenu.SetActive(true);
        paused = true;

        if (playerController)
        {
            playerController.enabled = false;
            Time.timeScale = 0;
        }

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        paused = false;

        if (playerController)
        {
            playerController.enabled = true;
            Time.timeScale = 1;
        }
    }

    public void ClickSound()
    {
        if(audioM)
        audioM.PlaySound("UI click button", 0);
    }

    public void OpenSound()
    {
        if (audioM)
            audioM.PlaySound("UI open menu", 0);
    }

    public void CloseSound()
    {
        if (audioM)
            audioM.PlaySound("UI close menu", 0);
    }
}
