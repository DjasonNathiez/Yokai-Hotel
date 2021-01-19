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
    private bool inRange;

    AudioManager audioM;

    // Start is called before the first frame update
    void Awake()
    {
        audioM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
        keysinroom = 1;
        this.gameObject.SetActive(true);
        interactb.SetActive(false);
        bActif = true;
        anim = this.gameObject.GetComponent<Animator>();

    }

    private void Update()
    {
        if (inRange == true)
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


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
        {
            interactb.SetActive(true);
            inRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bActif == true))
        {
            interactb.SetActive(false);
            inRange = false;
        }
            
    }

    public void KeysSound()
    {
        if (audioM)
            audioM.PlaySound("Keys", 0);
    }
}
