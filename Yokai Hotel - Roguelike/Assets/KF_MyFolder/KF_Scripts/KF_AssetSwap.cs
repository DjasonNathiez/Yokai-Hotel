using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_AssetSwap : MonoBehaviour
{
    public List<GameObject> assetSets;

    // Start is called before the first frame update
    void Start()
    {
        for (int i =0; i < assetSets.Count; i++)
        {
            assetSets[i].SetActive(false);
        }

        GameObject chosenSet;
        chosenSet = assetSets[Random.Range(0, assetSets.Count)];
        chosenSet.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
