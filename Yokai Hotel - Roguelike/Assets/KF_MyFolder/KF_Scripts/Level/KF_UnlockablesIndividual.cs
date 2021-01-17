using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_UnlockablesIndividual : MonoBehaviour
{
    public bool secretUnlocked;
    public int secretInList;
    public KF_Interaction linkedDialogue;
    private bool runOnce;

    private void Update()
    {
        if ((linkedDialogue.isGiven == true) && (runOnce == false))
        {
            if (FindObjectOfType<KF_Unlockables>().unlocked.Contains(secretInList))
                Debug.Log("Secret already unlocked");
            else
            {
                secretUnlocked = true;
            }
            runOnce = true;
        }
    }
}
