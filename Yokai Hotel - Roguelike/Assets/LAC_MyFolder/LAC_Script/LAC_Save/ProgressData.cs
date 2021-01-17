using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressData 
{
    public int maxLevelReached;

    public ProgressData(KF_Unlockables unlockables)
    {
        maxLevelReached = unlockables.maxLevelReached;
        
    }
}
