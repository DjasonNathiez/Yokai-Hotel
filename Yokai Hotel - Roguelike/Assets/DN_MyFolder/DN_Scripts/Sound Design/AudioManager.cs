 using Unity.Audio;
using UnityEngine.Audio;
using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public MenuManager pause;
    public AudioMixerGroup mixerGroupPause;
    MusicZone musicZone;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }


        DontDestroyOnLoad(gameObject);
        musicZone = GetComponent<MusicZone>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = s.mixerGroup;
        }
    }

    private void Start()
    {
        PlaySound(musicZone.musicChose, 0);

        StopSound("Theme1.0", 0);
        StopSound("Theme2.0", 0);
        StopSound("Theme4.0", 0);
    }

 

    public void PlaySoundMultiple(string name, float time)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        if ((!pause.paused) || (pause.paused && s.mixerGroup == mixerGroupPause))
            StartCoroutine(PlaySoundTimer(s, time));
        else if (pause.paused)
            StartCoroutine(StopSoundTimer(s, 0));
    }
    public void PlaySound (string name, float time)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        if ((!s.source.isPlaying && !pause.paused) || (pause.paused && s.mixerGroup == mixerGroupPause))
            StartCoroutine(PlaySoundTimer(s, time));
        else if (pause.paused)
            StartCoroutine(StopSoundTimer(s, 0));
          
    }
    public void StopSound(string name, float time)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        StartCoroutine(StopSoundTimer(s, time));
    }
    public void SetSoundVolume(string name, float volume, float delay)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        StartCoroutine(SetVolumeTimer(s, volume, delay));
    }
    IEnumerator PlaySoundTimer(Sound s, float time)
    {
        yield return new WaitForSeconds(time);
            s.source.Play();
    }

    IEnumerator SetVolumeTimer(Sound s, float volume, float time)
    {
        yield return new WaitForSeconds(time);
        s.source.volume = volume;
    }
    IEnumerator StopSoundTimer(Sound s, float time)
    {
        yield return new WaitForSeconds(time);
        s.source.Stop();
    }
    public float GetVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            return s.source.volume;
        else
            return 0;
    }
    public bool IsPlayingSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            return s.source.isPlaying;
        else
            return false;
    }
}
