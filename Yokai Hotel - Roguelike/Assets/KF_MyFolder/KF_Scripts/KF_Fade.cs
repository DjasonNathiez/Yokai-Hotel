using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_Fade : MonoBehaviour
{
    public Animator transition;
    private bool animStart;

    private void Start()
    {
        transition = GameObject.FindGameObjectWithTag("FadeAnim").GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (animStart == true)
        {
            transition.SetTrigger("Fade");
            animStart = false;
            
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animStart = true;
        }
    }
}
