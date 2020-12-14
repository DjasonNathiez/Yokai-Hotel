using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_Fade : MonoBehaviour
{
    public Animator transition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transition.SetBool("Fade", true);
            StartCoroutine("FadeWait");
        }
    }
    private IEnumerator FadeWait()
    {
        yield return new WaitForSeconds(1f);
        transition.SetBool("Fade", false);

    }
}
