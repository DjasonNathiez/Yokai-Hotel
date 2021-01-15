using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_UnlockablesIndividual : MonoBehaviour
{
    public bool secretUnlocked;
    public int secretInList;
    public KF_Interaction linkedDialogue;

    private void Update()
    {
        if (linkedDialogue.isGiven == true)
        {
            secretUnlocked = true;
        }
    }
}
