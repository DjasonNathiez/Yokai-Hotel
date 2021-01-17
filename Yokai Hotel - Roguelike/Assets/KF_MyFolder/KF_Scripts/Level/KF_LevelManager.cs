using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_LevelManager : MonoBehaviour
{
    public List<GameObject> levels;
    public GameObject currentLevel;
    public GameObject nextLevel;
    public int levelCount;
    public Transform playerPosition;
    private PlayerController player;

    public KF_CheckTriggers checkTriggers;
    public KF_LevelExit endTrigger;
    public GameObject fadecanvas;
    public bool levelChanged;

    public GameObject postProcess1;
    public GameObject postProcess2;
    public int postProcessChangeLevel;
    public bool hubReturn;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        for (int i = 1; i < levels.Count; i++)
        {
            levels[i].SetActive(false);
        }
        currentLevel = levels[0];
        levelCount = 0;
        Transform levelpos = currentLevel.transform;
        foreach (Transform child in levelpos) if (child.CompareTag("StartPoint"))
                playerPosition.position = child.position;
        checkTriggers = currentLevel.GetComponent<KF_CheckTriggers>();
        fadecanvas.SetActive(true);

        postProcess1.SetActive(true);
        postProcess2.SetActive(false);
        levelChanged = false;
    }

    public void Update()
    {
        if (player.isDead == true)
            hubReturn = true;

        if (currentLevel.CompareTag("Hub"))
        {
            endTrigger = GameObject.FindGameObjectWithTag("HubTrigger").GetComponent<KF_LevelExit>();
            if (endTrigger.exitTrigger == true)
            {
                levelChanged = true;
                StartCoroutine("MoveLevelHub");
            }
        }
        else
        {
            checkTriggers = currentLevel.GetComponent<KF_CheckTriggers>();
            if (checkTriggers.exitcheck == true)
            {
                levelChanged = true;
                StartCoroutine("MoveLevel");
            }
            /*if (checkTriggers.goingDownCheck == true)
            {
                levelCount = levelCount - 1;
                nextLevel = levels[levelCount];
                currentLevel = nextLevel;
                Transform levelpos1 = currentLevel.transform;
                foreach (Transform child in levelpos1) if (child.CompareTag("StartPoint"))
                        playerPosition.position = child.position;
            }*/
        }
        if (levelCount == postProcessChangeLevel)
        {
            postProcess1.SetActive(false);
            postProcess2.SetActive(true);
        }
        if (hubReturn == true)
        {
            levelChanged = true;
            hubReturn = false;
        }
    }
    private IEnumerator MoveLevelHub()
    {
        levelCount = levelCount + 1;
        nextLevel = levels[levelCount];
        currentLevel = nextLevel;
        GameObject[] endtriggers = GameObject.FindGameObjectsWithTag("EndTrigger");
        foreach (GameObject trigger in endtriggers)
            GameObject.Destroy(trigger);
        GameObject[] endtriggers2 = GameObject.FindGameObjectsWithTag("EndTrigger2");
        foreach (GameObject trigger in endtriggers2)
            GameObject.Destroy(trigger);
        levels[levelCount].SetActive(true);
        Transform levelpos1 = currentLevel.transform;
        foreach (Transform child in levelpos1) if (child.CompareTag("StartPoint"))
                playerPosition.position = child.position;
        yield return new WaitForSeconds(5f);
    }

    private IEnumerator MoveLevel()
    {
        levelCount = levelCount + 1;
        nextLevel = levels[levelCount];
        GameObject previouslevel = currentLevel;
        currentLevel = nextLevel;
        GameObject[] currentrooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject room in currentrooms)
            GameObject.Destroy(room);
        GameObject[] prepareEndRooms = GameObject.FindGameObjectsWithTag("PrepareEndRoom");
        foreach (GameObject proom in prepareEndRooms)
            GameObject.Destroy(proom);
        previouslevel.SetActive(false);
        levels[levelCount].SetActive(true);
        Transform levelpos1 = currentLevel.transform;
        foreach (Transform child in levelpos1) if (child.CompareTag("StartPoint"))
                playerPosition.position = child.position;
        yield return new WaitForSeconds(5f);
    }

}
