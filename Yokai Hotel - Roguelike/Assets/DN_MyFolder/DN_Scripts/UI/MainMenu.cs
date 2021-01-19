using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int sceneIndex;

    public Image black;
    public Animator fadeAnim;

    public void Play()
    {
        StartCoroutine(Fading());
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator Fading()
    {
        fadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(sceneIndex);
    }
}
