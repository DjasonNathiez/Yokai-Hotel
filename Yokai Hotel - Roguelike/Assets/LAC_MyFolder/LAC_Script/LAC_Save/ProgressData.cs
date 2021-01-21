using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData 
{
    public bool hubReturn;

    public int maxLevelReached;
    public bool unlock2;
    public bool unlock4;
    public bool unlock6;
    public bool unlock8;
    public bool unlockBoss;
    public bool firstTime;
    public bool keyboard;

    public List<int> unlocked = new List<int>();
    

    public ProgressData(KF_Unlockables unlockables)
    {
        hubReturn = unlockables.hubReturn;

        maxLevelReached = unlockables.maxLevelReached;
        unlock2 = unlockables.unlock2;
        unlock4 = unlockables.unlock4;
        unlock6 = unlockables.unlock6;
        unlock8 = unlockables.unlock8;
        unlockBoss = unlockables.unlockBoss;
        firstTime = unlockables.firstTime;
        keyboard = unlockables.keyboard;

        for (int i = 0; i < unlockables.unlocked.Count; i++)
            unlocked.Add(unlockables.unlocked[i]);
    }
}
