using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_RoomCam : MonoBehaviour
{
    public GameObject virtulCam;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if(virtulCam)
                virtulCam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (virtulCam)
                virtulCam.SetActive(false);
        }
    }
}
