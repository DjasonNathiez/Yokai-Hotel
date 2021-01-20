using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    [SerializeField]
    string[] musicAllow;
    public string musicChose;

    public int hubIndex;
    public int smoothLevelIndex;
    public int darkLevelIndex;
    public int bossIndex;

    KF_LevelManager levelM;
    Collider2D collider;
    AudioManager audioM;
    public LayerMask layerMask;
    bool trigger = false;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        audioM = GetComponent<AudioManager>();

        levelM = GameObject.FindGameObjectWithTag("HotelManager").GetComponent<KF_LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeRoomMusic();

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

    void ChangeRoomMusic()
    {
        if(levelM.levelCount == hubIndex)
        {
            musicChose = "Main Theme";
        }

        if (levelM.levelCount == smoothLevelIndex)
        {
            musicChose = "OST 1";
        }

        if (levelM.levelCount == darkLevelIndex)
        {
            musicChose = "OST 2";
        }

        if (levelM.levelCount == bossIndex)
        {
            musicChose = "Boss Theme";
        }
    }
}
