using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public bool allowSpawn;

    public Wave[] waves;
    public float timeToWave;

    int waveIndex = 0;
    public EnnemiBehaviour[] activeEnemy;
    private Transform roomtr;
    public GameObject room;
    public List<GameObject> doors;
    public bool tutorial;

    public GameObject poofVFX;
    // Start is called before the first frame update
    void Start()
    {
        waves = GetComponentsInChildren<Wave>();
        foreach( Wave w in waves)
        {
            w.poofVFX = poofVFX;
            w.gameObject.SetActive(false);
        }
        roomtr = transform.root;
        room = roomtr.gameObject;
        if (tutorial == false)
        {
            foreach (Transform child in roomtr)
            {
                if ((child.tag == "Door") && (child.gameObject.GetComponent<Door>().doorLink != null))
                {
                    doors.Add(child.gameObject);
                }
            }
        }
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update wave index 
        bool waveEnd = (activeEnemy.Length > 0);
        for (int i = 0; i < activeEnemy.Length; i++)
        {
            if (activeEnemy[i] != null)
                waveEnd = false;
        }
        if (waveEnd && waveIndex <= waves.Length - 1)
        {
            waveIndex++;
            activeEnemy = new EnnemiBehaviour[0];
        }

        //apply spawn
        if(waveIndex <= waves.Length - 1 && allowSpawn)
        {
            if (waves[waveIndex].gameObject.activeSelf == false)
            {
                waves[waveIndex].gameObject.SetActive(true);
                StartCoroutine(DelayToWave(waves[waveIndex], timeToWave));
            }
        }

        if(waveIndex >= waves.Length)
        {
            Debug.Log("all wave End");
            for (int i = 0; i < doors.Count; i++)
            {
                doors[i].GetComponent<BoxCollider2D>().isTrigger = true;
            }
            Destroy(gameObject);
        }
        

       
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            allowSpawn = true;
    }

    IEnumerator DelayToWave(Wave wave, float delay)
    {
        yield return new WaitForSeconds(delay);
        activeEnemy = wave.enemyArray;
        wave.Spawn();
    }
}
