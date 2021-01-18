using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_TutorialUnlock : MonoBehaviour
{
    public int keycount;
    public GameObject endDoor;
    public BoxCollider2D endDoorTrigger;
    public KF_TutorialKeyActivate tutorialGetKey;
    public GameObject interactb;
    private bool bActif;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {;
        endDoorTrigger = endDoor.GetComponent<BoxCollider2D>();
        endDoorTrigger.isTrigger = false;
        keycount = tutorialGetKey.keysinroom;
        interactb.SetActive(false);
        bActif = true;
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (keycount == 0)
        {
            endDoorTrigger.isTrigger = true;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
        {
            interactb.SetActive(true);
            if ((keycount >= 1) && (Input.GetButtonDown("Interact")))
            {
                keycount = keycount - 1;
                Debug.Log("Key Removed");
                anim.SetBool("keyRemoved", true);
                Destroy(interactb);
                bActif = false;
            }
            else
                if ((keycount == 0) && (Input.GetButtonDown("Interact")))
                {
                    Debug.Log("Not enough Keys");
                }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
            interactb.SetActive(false);
    }
}
