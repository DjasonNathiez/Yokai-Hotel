using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_CheckTriggers : MonoBehaviour
{
    public KF_LevelExit exitTrigger;
    public KF_LevelExit exitTrigger2;
    public bool hub;
    //public KF_LevelExit goingDownTrigger;
    //public KF_LevelExit goingDownTrigger2;
    public bool exitcheck;
    //public ProceduralGenerator levelGen;
    public bool goingDownCheck;


    // Start is called before the first frame update
    void Start()
    {
        //levelGen = gameObject.GetComponent<ProceduralGenerator>();
        //exitTrigger = levelGen.endPrefab.GetComponentInChildren<KF_LevelExit>();
        //goingDownTrigger = levelGen.startPrefab.GetComponentInChildren<KF_LevelExit>();
        //goingDownTrigger =  GameObject.FindGameObjectWithTag("BackTrigger").GetComponent<KF_LevelExit>();
        //goingDownTrigger2 = GameObject.FindGameObjectWithTag("BackTrigger2").GetComponent<KF_LevelExit>();
        
    }

    // Update is called once per frame
    void Update()
    {
        exitTrigger = GameObject.FindGameObjectWithTag("EndTrigger").GetComponent<KF_LevelExit>();
        exitTrigger2 = GameObject.FindGameObjectWithTag("EndTrigger2").GetComponent<KF_LevelExit>();
        if (hub == true)
        {
            exitTrigger = GameObject.FindGameObjectWithTag("EndTrigger").GetComponent<KF_LevelExit>();
            exitTrigger2 = GameObject.FindGameObjectWithTag("EndTrigger2").GetComponent<KF_LevelExit>();
        }
        else
        {
            if ((exitTrigger.exitTrigger == true) || (exitTrigger2.exitTrigger == true))
            {
                exitcheck = true;
            }
            else
                exitcheck = false;
        }
        
        
        

        /*if ((goingDownTrigger.goingDownTrigger == true) || (goingDownTrigger2.goingDownTrigger == true))
        {
            goingDownCheck = true;
        }         
        else
            goingDownCheck = false;*/
    }
}
