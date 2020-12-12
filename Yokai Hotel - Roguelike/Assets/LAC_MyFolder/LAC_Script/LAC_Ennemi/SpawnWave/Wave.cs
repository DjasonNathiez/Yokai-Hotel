using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{

    public EnnemiBehaviour[] enemyArray;
    public float delayPerMeter;
    // Start is called before the first frame update
    void Start()
    {
        enemyArray = GetComponentsInChildren<EnnemiBehaviour>();
        foreach( EnnemiBehaviour e in enemyArray)
        {
            e.gameObject.SetActive(false);
        }
 
       
    }

    private void OnEnable()
    {
        //Spawn();
    }

    public void Spawn()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        foreach (EnnemiBehaviour e in enemyArray)
        {
            float dist = Vector2.Distance(player.transform.position, e.transform.position);
            float delay = dist * delayPerMeter;
            StartCoroutine(DelayToSpawn(e.gameObject, delay));
        }
    }
    public IEnumerator DelayToSpawn(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
    }
    
}
