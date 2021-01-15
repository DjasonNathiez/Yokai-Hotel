using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_LevelExit : MonoBehaviour
{
    public bool exitTrigger;
    //public bool goingDown;
    //public bool goingDownTrigger;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
                exitTrigger = true;
                
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine("TransitionLevelTime");
        }
    }

    private IEnumerator TransitionLevelTime()
    {
        exitTrigger = false;
        yield return new WaitForSeconds(5f);
    }


    /*public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (goingDown == false)
                exitTrigger = true;
            else
                if (goingDown == true)
                goingDownTrigger = true;

        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (goingDown == false)
                exitTrigger = false;
            else
                if (goingDown == true)
                goingDownTrigger = false;
        }
    }*/
}
