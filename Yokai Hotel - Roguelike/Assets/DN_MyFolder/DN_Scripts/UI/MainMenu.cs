using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int sceneIndex;
    public int transitionFade;
    bool titleIsUp;

    public GameObject splashscreen;
    public GameObject paperPlease;
    public GameObject titleYoo;
    public Animator fadeAnim;

    private void Update()
    {
        if(Input.anyKeyDown && titleIsUp == true)
        {
            fadeAnim.SetBool("MenuAppear", true);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public IEnumerator Fading()
    {
        yield return new WaitForSeconds(transitionFade);
        fadeAnim.SetBool("Fade", true);
    }

    public void TitleAppearNow()
    {
        titleIsUp = true;
        fadeAnim.SetBool("TitleAppear", true);
    }

    public void TitleWaiting()
    {
        fadeAnim.SetBool("TitleWaiting", true);
    }

    public void GoToIdleNow()
    {
        fadeAnim.SetBool("GoToIdle", true);
    }

    public void DestroyThis()
    {
        GameObject.Destroy(splashscreen);
    }
}
