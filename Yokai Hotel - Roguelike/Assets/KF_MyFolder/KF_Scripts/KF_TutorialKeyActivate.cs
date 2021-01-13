using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_TutorialKeyActivate : MonoBehaviour
{
    public Room thisRoom;
    public int keysinroom;
    public GameObject interactb;
    private bool bActif;
    private Animator anim;


    // Start is called before the first frame update
    void Awake()
    {

        keysinroom = 1;
        this.gameObject.SetActive(true);
        interactb.SetActive(false);
        bActif = true;
        anim = this.gameObject.GetComponent<Animator>();

    }



    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
        {
            if (Input.GetButtonDown("Interact"))
            {
                Debug.Log("Key Added");
                anim.SetBool("gotKey", true);
                Destroy(interactb);
                bActif = false;  
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
            interactb.SetActive(false);
    }
}
