using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public int playSceneNumber;
    public int hubScene;
    public Canvas pauseMenu;
    public bool paused;

    PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

        if (pauseMenu.enabled == true)
        {
            paused = true;
        } else if (pauseMenu.enabled == false)
        {
            paused = false;
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

        playerController.enabled = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseMenu.enabled = false;
        paused = false;
        playerController.enabled = true;
        Time.timeScale = 1;
    }

}
