using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

    public void SetVolumeSFX(float volume)
    {
        audioMixer.SetFloat("sfx", volume);
    }

    public void SetVolumeMUSIC(float volume)
    {
        audioMixer.SetFloat("music", volume);
    }
}
