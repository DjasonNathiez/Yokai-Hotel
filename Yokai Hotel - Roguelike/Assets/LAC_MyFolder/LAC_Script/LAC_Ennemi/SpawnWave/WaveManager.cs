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
    
    // Start is called before the first frame update
    void Start()
    {
        waves = GetComponentsInChildren<Wave>();
        foreach( Wave w in waves)
        {
            w.gameObject.SetActive(false);
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
