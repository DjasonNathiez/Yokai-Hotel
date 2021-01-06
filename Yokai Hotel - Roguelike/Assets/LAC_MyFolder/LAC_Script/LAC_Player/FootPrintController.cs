using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintController : MonoBehaviour
{
    public GameObject player;
    Vector2 lastPlayerPos, playerPos, deltaPos;

    public GameObject feetPrefab;
    
    public float footFreq, footDuration, footDist;
    float delta, rdFeet, rdFeet2;
    bool rightfeet;


    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if (delta >= footFreq + rdFeet) 
        {
            // update pose;
            lastPlayerPos = playerPos;
            playerPos = player.transform.position;

            deltaPos = playerPos - lastPlayerPos;
            if(deltaPos.magnitude >= 0.2f)
            {
                printFeet();

                rightfeet = !rightfeet;
                rdFeet = (Random.value - 0.5f) * (footFreq * 0.5f);
                rdFeet2 = (Random.value - 0.5f) * (footDist * 0.5f);
            }
            
            delta = 0; 
        }
    }
    public void printFeet()
    {
        GameObject feetPrint = Instantiate(feetPrefab, transform.position, transform.rotation);
        FeetRenderer m_FeetRenderer = feetPrint.GetComponent<FeetRenderer>();

        // apply rotation
        var rotationVector = feetPrint.transform.rotation.eulerAngles;
        rotationVector.z = (Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg) % 360;
        feetPrint.transform.rotation = Quaternion.Euler(rotationVector);

        // rigth or left feet
        float perpdAngle = (rotationVector.z + ((rightfeet) ? -90 : 90))%360 * Mathf.Deg2Rad;
        Vector2 feetOfset = new Vector2(Mathf.Cos(perpdAngle), Mathf.Sin(perpdAngle)).normalized * (footDist+rdFeet2);
        feetPrint.transform.position += (Vector3)feetOfset;

        m_FeetRenderer.fadeTime = m_FeetRenderer.fadeDuartion =  footDuration;
        m_FeetRenderer.alphaRef = m_FeetRenderer.fadeColor.a;

        Destroy(feetPrint, footDuration);
    }
}
