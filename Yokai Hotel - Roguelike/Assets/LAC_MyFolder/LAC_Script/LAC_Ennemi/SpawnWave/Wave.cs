using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{

    public EnnemiBehaviour[] enemyArray;
    public float delayPerMeter;
    public GameObject poofVFX;
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
            float dist = (Vector2.Distance(player.transform.position, e.transform.position));
            float delay = dist * delayPerMeter;
            StartCoroutine(DelayToSpawn(e.gameObject, delay));
        }
    }
    public IEnumerator DelayToSpawn(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if(poofVFX != null)
        {
            GameObject particle = Instantiate(poofVFX, obj.transform.position, obj.transform.rotation);
            Destroy(particle, 1);
        }

        //yield return new WaitForSeconds(0.2f);
        obj.SetActive(true);
    }
    
}
