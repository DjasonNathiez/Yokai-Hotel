using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KF_Unlockables : MonoBehaviour
{
    public List<GameObject> level2Unlock;
    public List<GameObject> level4Unlock;
    public List<GameObject> level6Unlock;
    public List<GameObject> level8Unlock;
    public List<GameObject> bossUnlock;
    public int maxLevelReached;
    public KF_LevelManager lvlM;
    public bool hubReturn;

    public int[] unlockLevels = new int[5];
    private bool updateUnlocks;
    public int nextUnlock;
    private bool onePerLevel;

    [Header("==== Secrets ====")]
    public List<GameObject> secrets;
    public bool secretActivated;
    public List<KF_UnlockablesIndividual> secretTriggers;
    private int secretToUnlock;
    public List<int> unlocked = new List<int>();

    [Header("==== Do Not Touch ====")]
    public bool unlock2;
    public bool unlock4;
    public bool unlock6;
    public bool unlock8;
    public bool unlockBoss;
    private int count;
    public bool runOnce;

    [Header("==== Save Settings ====")]
    private bool changeKeyCon;
    public bool keyboard;
    public bool firstTime;
    public bool deleteSave;
    



    void Awake()
    {
        Debug.Log("Save exist ? " + SaveSystem.SaveExist());
        lvlM = FindObjectOfType<KF_LevelManager>();

        if (SaveSystem.SaveExist() == true)
        {
            Debug.Log("LoadSave");
            firstTime = false;
            ProgressData data = SaveSystem.LoadProgress();
            maxLevelReached = data.maxLevelReached;
            unlock2 = data.unlock2;
            unlock4 = data.unlock4;
            unlock6 = data.unlock6;
            unlock8 = data.unlock8;
            unlockBoss = data.unlockBoss;
            hubReturn = data.hubReturn;
            firstTime = data.firstTime;
            keyboard = data.keyboard;

            lvlM.hubReturn = data.hubReturn;
            for (int i = 0; i < data.unlocked.Count; i++)
                unlocked.Add(data.unlocked[i]);

            KeepUnlocks();
        }
            //SaveSystem.LoadProgress().firstTime;
        /*if (firstTime == true)
        {
            SaveSystem.SaveProgress(this);
            SaveSystem.DeleteSave();
            Debug.Log("DeletedSave");

            
        }*/

        if (SaveSystem.SaveExist() == false)// detect first time      
        {
            SaveSystem.SaveProgress(this);
            SaveSystem.DeleteSave();
            Debug.Log("NewSave");

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
        }


        nextUnlock = unlockLevels[count];
        
        //foreach (GameObject secret in secrets)
         //   secret.SetActive(false);
        foreach (GameObject secrets in GameObject.FindGameObjectsWithTag("Secret"))
        {
            secretTriggers.Add(secrets.GetComponent<KF_UnlockablesIndividual>());
        }

    }


    void Update()
    {
        if (deleteSave == true)
        {
            DeleteSave();
        }

        hubReturn = lvlM.hubReturn;

        if ((lvlM.levelChanged == true) || (hubReturn == true))
        {
            firstTime = false;
            SaveSystem.SaveProgress(this);
            onePerLevel = false;
            StartCoroutine(AddSecret());
        }
        if (lvlM.levelCount >= maxLevelReached)
        {
            maxLevelReached = lvlM.levelCount;
        }

        if ((lvlM.levelCount == nextUnlock) && (onePerLevel == false))
        {
            Debug.Log("NEXT UNLOCK");
            updateUnlocks = true;
            nextUnlock = unlockLevels[count];
            count++;
            onePerLevel = true;
        }
        if (updateUnlocks == true)
            LevelUnlock();


        foreach (KF_UnlockablesIndividual secretTrigger in secretTriggers)
        {
            if (secretTrigger.secretUnlocked == true)
            {  
                secretToUnlock = secretTrigger.secretInList;
                secretTrigger.secretUnlocked = false;
                secretActivated = true;
            }
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
        else
        {
            foreach (GameObject go in level2Unlock)
                go.SetActive(false);
        }

        if (unlock4 == true)
        {
            foreach (GameObject go in level4Unlock)
            {
                go.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject go in level4Unlock)
                go.SetActive(false);
        }

        if (unlock6 == true)
        {
            foreach (GameObject go in level6Unlock)
            {
                go.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject go in level6Unlock)
                go.SetActive(false);
        }

        if (unlock8 == true)
        {
            foreach (GameObject go in level8Unlock)
            {
                go.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject go in level8Unlock)
                go.SetActive(false);
        }

        if (unlockBoss == true)
        {
            foreach (GameObject go in bossUnlock)
            {
                go.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject go in bossUnlock)
                go.SetActive(false);
        }

        for (int i = 0; i < unlocked.Count; i++)
        {
            foreach (int id in unlocked)
            {
                if (i == id)
                {
                    secrets[id].SetActive(true);
                }
                else
                    secrets[i].SetActive(false);
            }
        }
        /*foreach (int id in unlocked)
        {
            secrets[id].SetActive(true); //problem?
        }*/



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


    public void DeleteSave() //if problem at start
    {
        SaveSystem.SaveProgress(this);
        SaveSystem.DeleteSave();
        deleteSave = false;
    }

    public void Keyboard()
    {
        Debug.Log("Change1");
        keyboard = true;
        runOnce = true;
    }

    public void Manette()
    {
        keyboard = false;
        runOnce = true;
    }

}
