using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChangeTest : MonoBehaviour
{
    [SerializeField]
    private GameObject playerGameObject;
    [SerializeField]
    private GameObject arrivalArea;

    [SerializeField]
    private Vector3 arrivalAreaPosition;

    // Update is called once per frame
    void Update()
    {
        arrivalAreaPosition = arrivalArea.GetComponent<Transform>().position;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerGameObject.GetComponent<Transform>().position = arrivalAreaPosition;
        }
    }
}

