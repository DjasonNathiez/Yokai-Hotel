using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_FirstTime : MonoBehaviour
{
    public List<GameObject> firstTimeObjects;
    public List<GameObject> normalObjects;
    public List<GameObject> spawnObjects;
    public bool firstTime;
    public KF_Interaction tutorialActivator;
    public bool hubReturn; //when player dies = true. Should be reference of the gamemanager 

    void Awake()
    {
        if (firstTime == true)
        {
            foreach (GameObject objects in normalObjects)
            {
                objects.SetActive(false);
            }
            foreach (GameObject objects in spawnObjects)
            {
                objects.SetActive(true);
            }
            tutorialActivator.reverse = false;
            foreach (GameObject objects in firstTimeObjects)
            {
                objects.SetActive(true);
            }
        }
        if (firstTime == false)
        {
            foreach (GameObject objects in spawnObjects)
            {
                objects.SetActive(true);
            }
            foreach (GameObject objects in normalObjects)
            {
                objects.SetActive(false);
            }
            tutorialActivator.reverse = true;
            foreach (GameObject objects in firstTimeObjects)
            {
                objects.SetActive(false);
            }
        }
        hubReturn = FindObjectOfType<KF_LevelManager>().hubReturn;
    }

    private void Update()
    {
        if (hubReturn == true)
        {
            foreach (GameObject objects in spawnObjects)
            {
                objects.SetActive(false);
            }
            foreach (GameObject objects in normalObjects)
            {
                objects.SetActive(true);
            }
            if (firstTime == true)
            {
                foreach (GameObject objects in firstTimeObjects)
                {
                    objects.SetActive(false);
                }
            }
        }
    }
}
