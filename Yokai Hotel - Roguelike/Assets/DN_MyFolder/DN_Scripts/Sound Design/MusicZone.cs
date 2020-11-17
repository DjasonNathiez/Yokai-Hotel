using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    [SerializeField]
    string[] musicAllow = { "MainTheme", "Theme1.0", "Theme2.0", "Theme4.0" };
    public string musicChose;

    Collider2D collider;
    AudioManager audioM;
    public LayerMask layerMask;
    bool trigger = false;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        audioM = GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapArea(collider.bounds.min, collider.bounds.max, layerMask) && !trigger && !audioM.IsPlayingSound(musicChose))
        {
            // set current music volume
            
            audioM.SetSoundVolume(musicChose, 0, 0);
            audioM.PlaySound(musicChose, 0);
            StartCoroutine(MusicTransition(musicChose, true, 2f, 8f));

            //mute other music
            foreach (string m in musicAllow)
            {
                if (m != musicChose)
                {
                    StartCoroutine(MusicTransition(m, false, 1f, 8f));
                    audioM.StopSound(m, 1.1f);
                }    
            }
            trigger = true;
        }


        if (!audioM.IsPlayingSound(musicChose))
            trigger = false;
    }
    public IEnumerator MusicTransition(string music, bool play, float duration, float smooth )
    {
        int i = 0;
        while(i < smooth)
        {
            yield return new WaitForSeconds(duration / smooth);
            audioM.SetSoundVolume(music, (play) ? ( 0.1f*i/smooth) : (0.1f - (0.1f * i / smooth)), 0);
            i++;
        }
        
    }
}
