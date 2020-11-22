using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_EnemySwap : MonoBehaviour
{
    public List<GameObject> enemySets;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemySets.Count; i++)
        {
            enemySets[i].SetActive(false);
        }

        GameObject chosenSet;
        chosenSet = enemySets[Random.Range(0, enemySets.Count)];
        chosenSet.SetActive(true);

    }
}
