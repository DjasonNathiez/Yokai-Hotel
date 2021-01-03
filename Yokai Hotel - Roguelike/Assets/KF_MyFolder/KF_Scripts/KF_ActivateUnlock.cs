using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_ActivateUnlock : MonoBehaviour
{
    public GameObject totem1, totem2, totem3, totem4;
    public ProceduralGenerator lvlM;
    private int keysinlvl;
    public int keycount;
    public GameObject endDoor;
    public BoxCollider2D endDoorTrigger;


    // Start is called before the first frame update
    void Start()
    {
        totem1.SetActive(false);
        totem2.SetActive(false);
        totem3.SetActive(false);
        totem4.SetActive(false);
        lvlM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<ProceduralGenerator>();
        keysinlvl = lvlM.keyNumber;
        endDoor = GameObject.FindGameObjectWithTag("EndDoor");
        endDoorTrigger = endDoor.GetComponent<BoxCollider2D>();
        endDoorTrigger.isTrigger = false;
        keycount = keysinlvl;
    }

    // Update is called once per frame
    void Update()
    {
        if (keysinlvl == 1)
        {
            totem1.SetActive(true);
        }
        if (keysinlvl == 2)
        {
            totem1.SetActive(true);
            totem2.SetActive(true);
        }
        if (keysinlvl == 3)
        {
            totem1.SetActive(true);
            totem2.SetActive(true);
            totem3.SetActive(true);
        }
        if (keysinlvl == 4)
        {
            totem1.SetActive(true);
            totem2.SetActive(true);
            totem3.SetActive(true);
            totem4.SetActive(true);
        }
        if (keycount == 0)
        {
            endDoorTrigger.isTrigger = true;
        }
    }
}
