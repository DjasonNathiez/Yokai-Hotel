using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_DetectEnemies : MonoBehaviour
{
    public bool ennemiesInRoom;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ennemi"))
        {
            Debug.Log("Ennemies in room");
            ennemiesInRoom = true;
        }
        else
            Debug.Log("Room Cleared");
            ennemiesInRoom = false;
    }
}
