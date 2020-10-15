using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public string volumeName;
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(volumeName, Mathf.Log10(sliderValue*2) * 20);
    }
}
