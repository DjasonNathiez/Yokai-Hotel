using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class KF_MainMenu : MonoBehaviour
{
    public GameObject title, pressButton, background;
    public GameObject settingsMenu, creditsMenu, mainMenu;
    public GameObject settingsMenuButton, creditsMenuButton, playMenuButton, creditsButton, settingsButton;

    private bool pressedOnce;
    // Start is called before the first frame update
    void Start()
    {
        pressButton.SetActive(true);
        mainMenu.SetActive(false);
        title.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pressedOnce == false)
        {
            if ((Input.anyKeyDown) || Input.GetButtonDown("Interact") || Input.GetButtonDown("Dash"))
            {
                background.GetComponent<Animator>().SetTrigger("Change");
                pressButton.SetActive(false);
                title.SetActive(true);
                mainMenu.SetActive(true);
                pressedOnce = true;
                EventSystem.current.SetSelectedGameObject(playMenuButton);
                playMenuButton.GetComponent<Animator>().SetTrigger("Selected");
            }
        }

        if ((EventSystem.current.currentSelectedGameObject == null))
        {
            if ((Input.GetButtonDown("Horizontal")) || Input.GetButtonDown("Vertical"))
            {
                EventSystem.current.SetSelectedGameObject(playMenuButton);
            }
        }
            

    }

    public void CreditsMenu()
    {
        if (!creditsMenu.activeInHierarchy)
        {
            mainMenu.SetActive(false);
            creditsMenu.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(creditsButton);
        }
        else
        {
            creditsMenu.SetActive(false);
            mainMenu.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(creditsMenuButton);
        }
    }

    public void SettingsMenu()
    {
        if (!settingsMenu.activeInHierarchy)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingsButton);
        }
        else
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingsMenuButton);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
