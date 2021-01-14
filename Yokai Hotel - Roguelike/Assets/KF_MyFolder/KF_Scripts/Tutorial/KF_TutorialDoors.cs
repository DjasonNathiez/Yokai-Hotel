using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_TutorialDoors : MonoBehaviour
{
    public GameObject doorLink;
    public GameObject playerObj;
    public BoxCollider2D doorTriggerZone;
    public GameObject temporaryShow;
    public bool TutorialFinish;
    public bool TutorialStart;
    private GameObject tutorial;
    public GameObject tutorialDoor;
    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        doorTriggerZone = this.gameObject.GetComponent<BoxCollider2D>();
        tutorial = GameObject.FindGameObjectWithTag("Tutorial");

    }


    private void Update()
    {
        if (doorLink == null)
        {
            if (TutorialFinish == true)
            {
                doorTriggerZone.isTrigger = true;
                temporaryShow.SetActive(true);
                
            }
            if (TutorialStart == true)
            {
                doorTriggerZone.isTrigger = true;
                temporaryShow.SetActive(false);
            }
            else
            {
                doorTriggerZone.isTrigger = false;
                temporaryShow.SetActive(false);
            }
        }
        if (doorTriggerZone.isTrigger == false)
        {
            temporaryShow.SetActive(false);
        }
        if (doorTriggerZone.isTrigger == true)
        {
            temporaryShow.SetActive(true);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (TutorialFinish == true)
                Destroy(tutorialDoor);
            Teleport();              
        }
    }

    private void Teleport()
    {
        playerObj.transform.position = doorLink.GetComponent<Transform>().position;
        if (TutorialFinish == true)
            StartCoroutine(DestroyTuto());
    }

    IEnumerator DestroyTuto()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(tutorial);
    }
}
