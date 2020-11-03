using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundActivation : MonoBehaviour
{
    AudioManager audioManager;
    PlayerController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        audioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
    }

    void Update()
    {

    }

    void PlayerSound()
    {
        if (Input.GetButtonDown("Attack1"))
        {
            audioManager.PlaySound("Player fast attack", 0);
        }

        if (Input.GetButtonDown("Attack2"))
        {
            audioManager.PlaySound("Player heavy attack", 0);
        }

        if (Input.GetButtonDown("Dash"))
        {
            audioManager.PlaySound("Player dash", 0);
        }
    }
}
