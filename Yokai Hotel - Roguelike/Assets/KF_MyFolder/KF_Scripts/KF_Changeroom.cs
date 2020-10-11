using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_Changeroom : MonoBehaviour
{
    public Transform targetRoom;
    private Transform currentRoom;

    [SerializeField]
    private GameObject playerGameObject;
    [SerializeField]
    private Transform arrivalAreaPosition;
    private Transform roomCamera;
    private Transform arrivalRoomCamera;

    public bool eastDoor;
    public bool northDoor;
    public bool southDoor;
    public bool westDoor;

    public Transform resetTrans;


    // Update is called once per frame
    private void Start()
    {
        currentRoom = this.transform.parent;
        foreach (Transform child in currentRoom) if (child.CompareTag("MainCamera"))
            {
                roomCamera = child;
                roomCamera.gameObject.SetActive(true);
            }
    }

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
            if (southDoor == true)
            {
                foreach (Transform child in targetRoom) if (child.CompareTag("NorthArrival"))
                    {
                        arrivalAreaPosition = child;
                        StartCoroutine("UpdateLocation");
                    }

            }
            if (westDoor == true)
            {
                foreach (Transform child in targetRoom) if (child.CompareTag("EastArrival"))
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

