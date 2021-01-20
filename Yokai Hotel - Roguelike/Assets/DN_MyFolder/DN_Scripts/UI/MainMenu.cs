using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int sceneIndex;
    public int transitionFade;

    public GameObject splashscreen;
    public Animator fadeAnim;

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

    public void DestroyThis()
    {
        GameObject.Destroy(splashscreen);
    }
}
