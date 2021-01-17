using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_Unlockables : MonoBehaviour
{
    public List<GameObject> level2Unlock;
    public List<GameObject> level4Unlock;
    public List<GameObject> level6Unlock;
    public List<GameObject> level8Unlock;
    public List<GameObject> bossUnlock;
    public int maxLevelReached;
    public KF_LevelManager lvlM;
    private bool hubReturn;

    private int[] unlockLevels = { 2, 4, 6, 8, 10 };
    private bool updateUnlocks;
    public bool unlock2;
    private bool unlock4;
    private bool unlock6;
    private bool unlock8;
    private bool unlockBoss;
    private int nextUnlock;
    private bool onePerLevel;

    [Header("==== Secrets ====")]
    public List<GameObject> secrets;
    public bool secretActivated;
    public List<KF_UnlockablesIndividual> secretTriggers;
    private int secretToUnlock;
    public List<int> unlocked = new List<int>();




    void Start()
    {
        Debug.Log("Save exist ? " + SaveSystem.SaveExist());
        ProgressData data = SaveSystem.LoadProgress();


        nextUnlock = 2;
        hubReturn = lvlM.hubReturn;
        lvlM = FindObjectOfType<KF_LevelManager>();
        foreach (GameObject go in level2Unlock)
            go.SetActive(false);
        foreach (GameObject go in level4Unlock)
            go.SetActive(false);
        foreach (GameObject go in level6Unlock)
            go.SetActive(false);
        foreach (GameObject go in level8Unlock)
            go.SetActive(false);
        foreach (GameObject go in bossUnlock)
            go.SetActive(false);
        foreach (GameObject secret in secrets)
            secret.SetActive(false);
        foreach (GameObject secrets in GameObject.FindGameObjectsWithTag("Secret"))
        {
            secretTriggers.Add(secrets.GetComponent<KF_UnlockablesIndividual>());
        }
        if (hubReturn == true)
            KeepUnlocks();
    }


    void Update()
    {
        hubReturn = lvlM.hubReturn;
        if (lvlM.levelCount >= maxLevelReached)
        {
            maxLevelReached = lvlM.levelCount;
        }

        if ((maxLevelReached == nextUnlock) && (onePerLevel == false))
        {
            updateUnlocks = true;
        }
        if (updateUnlocks == true)
            LevelUnlock();

        //

        foreach (KF_UnlockablesIndividual secretTrigger in secretTriggers)
        {
            if (secretTrigger.secretUnlocked == true)
            {  
                secretToUnlock = secretTrigger.secretInList;
                secretTrigger.secretUnlocked = false;
                secretActivated = true;
            }
        }
        if ((lvlM.levelChanged == true) || (hubReturn == true))
        {
            StartCoroutine(AddSecret());
        }
        if (secretActivated == true)
            SecretUnlock();

    }

    void SecretUnlock()
    {
        for (int i = 0; i < secrets.Count; i++)
        {
            if (i == secretToUnlock)
            {
                secrets[i].SetActive(true);
                unlocked.Add(i);
            }
        }
        secretActivated = false;
    }

    void LevelUnlock()
    {
        if ((maxLevelReached == 2) && (unlock2 == false))
        {
            foreach (GameObject go in level2Unlock)
            {
                go.SetActive(true);
            }
            unlock2 = true;
        }
        if ((maxLevelReached == 4) && (unlock4 == false))
        {
            foreach (GameObject go in level4Unlock)
            {
                go.SetActive(true);
            }
            unlock4 = true;
        }
        if ((maxLevelReached == 6) && (unlock6 == false))
        {
            foreach (GameObject go in level6Unlock)
            {
                go.SetActive(true);
            }
            unlock6 = true;
        }
        if ((maxLevelReached == 8) && (unlock8 == false))
        {
            foreach (GameObject go in level8Unlock)
            {
                go.SetActive(true);
            }
            unlock8 = true;
        }
        if ((maxLevelReached == 10) && (unlockBoss == false))
        {
            foreach (GameObject go in bossUnlock)
            {
                go.SetActive(true);
            }
            unlockBoss = true;
        }
        updateUnlocks = false;
        onePerLevel = true;
    }


    void KeepUnlocks()
    {
        if (unlock2 == true)
        {
            foreach (GameObject go in level2Unlock)
            {
                go.SetActive(true);
            }
        }
        if (unlock4 == true)
        {
            foreach (GameObject go in level4Unlock)
            {
                go.SetActive(true);
            }
        }
        if (unlock6 == true)
        {
            foreach (GameObject go in level6Unlock)
            {
                go.SetActive(true);
            }
        }
        if (unlock8 == true)
        {
            foreach (GameObject go in level8Unlock)
            {
                go.SetActive(true);
            }
        }
        if (unlockBoss == true)
        {
            foreach (GameObject go in bossUnlock)
            {
                go.SetActive(true);
            }
        }
        foreach (int id in unlocked)
        {
            secrets[id].SetActive(true); //problem?
        }


    }

    private IEnumerator AddSecret()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject secrets in GameObject.FindGameObjectsWithTag("Secret"))
        {
            if (secretTriggers.Contains(secrets.GetComponent<KF_UnlockablesIndividual>()))
                Debug.Log("Secret already added");
            else
                secretTriggers.Add(secrets.GetComponent<KF_UnlockablesIndividual>());
        }
        onePerLevel = false;
    }
}
