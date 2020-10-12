using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_Changeroom : MonoBehaviour
{
    public Transform targetRoom;

    [SerializeField]
    private GameObject playerGameObject;
    [SerializeField]
    private Transform arrivalAreaPosition;

    public bool eastDoor;
    public bool northDoor;

    public Transform resetTrans;


    // Update is called once per frame


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            if (eastDoor == true)
            {
                foreach (Transform child in targetRoom) if (child.CompareTag("WestArrival"))
                    {
                        arrivalAreaPosition = child;
                        StartCoroutine("UpdateLocation");
                    }

            }
            if (northDoor == true)
            {
                foreach (Transform child in targetRoom) if (child.CompareTag("SouthArrival"))
                    {
                        arrivalAreaPosition = child;
                        StartCoroutine("UpdateLocation");
                    }

            }
        }
    }

    IEnumerator UpdateLocation()
    {
        playerGameObject.GetComponent<Transform>().position = arrivalAreaPosition.position;
        yield return new WaitForSeconds(1);
        arrivalAreaPosition = resetTrans;
    }
}

