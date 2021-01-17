using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData 
{
    public bool hubReturn;

    public int maxLevelReached;
    

    public ProgressData(KF_Unlockables unlockables, KF_LevelManager lvlM)
    {
        hubReturn = lvlM.hubReturn;

        maxLevelReached = unlockables.maxLevelReached;
        
    }
}
