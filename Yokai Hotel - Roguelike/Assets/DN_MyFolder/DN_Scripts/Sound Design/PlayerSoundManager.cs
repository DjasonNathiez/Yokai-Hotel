using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    AudioManager audioM;

    // Start is called before the first frame update
    void Start()
    {
        audioM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightAttackSound()
    {
        audioM.PlaySound("Player fast attack", 0); 
    }

    public void HeavyAttackSound()
    {
       audioM.PlaySound("Player heavy attack", 0);
    }
}
