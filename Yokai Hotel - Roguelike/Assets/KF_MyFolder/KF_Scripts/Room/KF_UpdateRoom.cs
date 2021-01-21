using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_UpdateRoom : MonoBehaviour
{
// for props in rooms

    public int levelChange1;
    public int levelChange2;
    public KF_Unlockables unlockables;
    private int maxlevelReached;

    public List<GameObject> startObjects;
    public List<GameObject> middleObjects;
    public List<GameObject> endObjects;


    void Start()
    {
        unlockables = FindObjectOfType<KF_Unlockables>();
        maxlevelReached = unlockables.maxLevelReached;
        if (maxlevelReached < levelChange1)
        {
            foreach (GameObject objects in startObjects)
            {
                objects.SetActive(true);
            }
            foreach (GameObject objects in middleObjects)
                objects.SetActive(false);
            foreach (GameObject objects in endObjects)
                objects.SetActive(false);
        }
        if ((maxlevelReached < levelChange2) && (maxlevelReached >= levelChange1 ))
        {
            foreach (GameObject objects in startObjects)
                objects.SetActive(false);
            foreach (GameObject objects in middleObjects)
                objects.SetActive(true);
            foreach (GameObject objects in endObjects)
                objects.SetActive(false);
        }
        if (maxlevelReached >= levelChange2)
        {
            foreach (GameObject objects in startObjects)
                objects.SetActive(false);
            foreach (GameObject objects in middleObjects)
                objects.SetActive(true);
            foreach (GameObject objects in endObjects)
                objects.SetActive(true);
        }
    }


    void Update()
    {
        maxlevelReached = unlockables.maxLevelReached;
        if (maxlevelReached == levelChange1)
        {
            foreach (GameObject objects in startObjects)
            {
                objects.SetActive(false);
            }
            foreach (GameObject objects in middleObjects)
            {
                objects.SetActive(true);
            }
        }
        if (maxlevelReached == levelChange2)
        {
            foreach (GameObject objects in middleObjects)
            {
                objects.SetActive(false);
            }
            foreach (GameObject objects in endObjects)
            {
                objects.SetActive(true);
            }
        }
    }
}
