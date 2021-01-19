using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_MultipleInteract : MonoBehaviour
{
    public KF_InteractionManager intM;
    public List<KF_Interaction> interactions;
    public bool random;
    public KF_Interaction currentInteract;
    public int interactCount;
    public KF_LevelManager LevelM;
    private int listMax;
    public bool hubReturn;
    public bool endOfDialogue;


    void Start()
    {
        LevelM = FindObjectOfType<KF_LevelManager>();
        hubReturn = LevelM.gameObject.GetComponent<KF_Unlockables>().hubReturn;
        currentInteract = interactions[0];
        AddLines();
        if (random == true)
            currentInteract = interactions[Random.Range(0, interactions.Count)];
        intM = FindObjectOfType<KF_InteractionManager>();
        for (int i = 0; i < interactions.Count; i++)
        {
            if (interactions[i] == currentInteract)
            {
                interactions[i].gameObject.SetActive(true);
            }
            else
                interactions[i].gameObject.SetActive(false);
        }
        listMax = interactions.Count;    
    }

    void Update()
    {
        endOfDialogue = currentInteract.endOfDialogue;
        hubReturn = LevelM.hubReturn;
        if ((endOfDialogue == true) && (random == false))
        {
            currentInteract.endOfDialogue = false;
            currentInteract.gameObject.SetActive(false);
            interactCount++;
            if (interactCount == interactions.Count)
            {
                interactCount = 0;
            }
            currentInteract = interactions[interactCount];
            currentInteract.gameObject.SetActive(true);
            intM.interactDone = false;
        }
        if ((endOfDialogue == true) && (random == true))
        {
            currentInteract.endOfDialogue = false;
            currentInteract.gameObject.SetActive(false);
            interactCount++;
            if (interactCount == interactions.Count)
            {
                interactCount = 0;
            }
            currentInteract = interactions[Random.Range(0, interactions.Count)];
            currentInteract.gameObject.SetActive(true);
            intM.interactDone = false;
        }
        if ((LevelM.levelChanged == true) || (hubReturn == true))
        {
            AddLines();
        }
    }

    void AddLines()
    {
        foreach (Transform child in this.gameObject.GetComponent<Transform>())
        {
            if (child.gameObject.CompareTag("Interact"))
            {
                if (interactions.Contains(child.gameObject.GetComponent<KF_Interaction>()))
                    Debug.Log("Already in list");
                else
                {
                    if (child.gameObject.activeInHierarchy)
                    {
                        interactions.Add(child.gameObject.GetComponent<KF_Interaction>());
                        child.gameObject.SetActive(false);
                    }
                }

            }
        }
        foreach (KF_Interaction interact in interactions)
        {
            if (interact == interactions[0])
                interact.gameObject.SetActive(true);
        }
        listMax = interactions.Count;
    }
}
