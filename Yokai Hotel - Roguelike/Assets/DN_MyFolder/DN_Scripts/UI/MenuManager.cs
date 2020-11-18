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

    public Canvas pauseMenu;
    public Canvas HUD;

    PlayerController playerController;

    private void Awake()
    {
        GameObject menu = GameObject.FindGameObjectWithTag("PauseMenu");
        if( menu)
        pauseMenu = menu.GetComponent<Canvas>();

        GameObject hud = GameObject.FindGameObjectWithTag("HUD");
        if(hud)
            HUD = hud.GetComponent<Canvas>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player)
            playerController = player.GetComponent<PlayerController>();
        
        
    }
    public void Start()
    {
    }
    public void Update()
    {
        if (Input.GetButtonDown("Cancel") && paused == false)
        {
            Pause();
        }
        else if (Input.GetButtonDown("Cancel") && paused == true)
        {
            Resume();
        }

        if (pauseMenu)
        {
            if (pauseMenu.enabled == true)
            {
                paused = true;
            }
            else if (pauseMenu.enabled == false)
            {
                paused = false;
            }

            if(HUD && paused == true)
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
        
        pauseMenu.enabled = true;
        paused = true;

        if (playerController)
        {
            playerController.enabled = false;
            Time.timeScale = 0;
        }
        
    }

    public void Resume()
    {
        pauseMenu.enabled = false;
        paused = false;

        if (playerController)
        {
            playerController.enabled = true;
            Time.timeScale = 1;
        }
    }

}
