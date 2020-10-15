using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    public GameObject virtualCam;
    private void Start()
    {
        Cinemachine.CinemachineVirtualCamera vCam = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();

        if(vCam)
            virtualCam = vCam.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && virtualCam)
        {
            virtualCam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && virtualCam)
        {
            virtualCam.SetActive(false);
        }
    }
}
