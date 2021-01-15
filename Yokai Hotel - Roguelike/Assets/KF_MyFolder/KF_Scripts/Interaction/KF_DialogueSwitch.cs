using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_DialogueSwitch : MonoBehaviour
{

    public Sprite objectBox;
    public Sprite dialogueBox;
    public KF_InteractionManager intM;
    public SpriteRenderer dialogueText;
    // Start is called before the first frame update
    void Start()
    {
        intM = FindObjectOfType<KF_InteractionManager>();
        dialogueText = intM.dialogueText.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
