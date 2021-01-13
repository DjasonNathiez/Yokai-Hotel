using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_TutorialWave : MonoBehaviour
{
    public List<GameObject> ennemies;
    private bool doorsOpen;
    public KF_TutorialDoors door1;
    public KF_TutorialDoors door2;
    public bool haveSpawned;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject ennemi in ennemies)
            ennemi.SetActive(false);
        door1.doorTriggerZone.isTrigger = false;
        door2.doorTriggerZone.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (haveSpawned == true)
        {
            foreach (GameObject ennemi in ennemies)
            {
                if (ennemi == null)
                {
                    ennemies.Remove(ennemi);
                }
            }
        }           
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (GameObject ennemi in ennemies)
            {
                ennemi.SetActive(true);
                haveSpawned = true;
            }
        }         
    }

    void ClearList()
    {
        door1.doorTriggerZone.isTrigger = true;
        door2.doorTriggerZone.isTrigger = true;
    }
}
