using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenFade : MonoBehaviour
{
    Animator fadeAnim;
    // Start is called before the first frame update
    void Start()
    {
        fadeAnim = GetComponent<Animator>();

        fadeAnim.SetBool("isFading", true);

        
    }

    public void EnableControls()
    {
        
    }
}
