using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KF_InteractionManager : MonoBehaviour
{
    public Text dialogueText; 
    public Queue<string> sentences; //sentence queue from the Dialogue script of the object of interaction
    [SerializeField]
    private float waitTime; //time between each characters
    public Animator dialogueAnim; //to make dialogue appear
    public List<GameObject> interactionInLevel = new List<GameObject>();
    public KF_LevelManager levelM;
    public Sprite objectBox;
    public Sprite dialogueBox;
    public Image dialogueBoxSP;
    public bool objectDialogue;
    public bool interactDone;
    public int[] objectDialogueColor = new int[3];

    void Start()
    {
        sentences = new Queue<string>();
        waitTime = 0;
        dialogueText = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<Text>();
        levelM = FindObjectOfType<KF_LevelManager>();
        dialogueBoxSP = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<Image>();
        dialogueAnim = GameObject.FindGameObjectWithTag("DialogueAnim").GetComponent<Animator>();
        foreach (GameObject interact in GameObject.FindGameObjectsWithTag("Interact"))
        {
            interactionInLevel.Add(interact);
        }
    }
    private void Update()
    {
        if ((levelM.levelChanged == true) || (levelM.hubReturn == true))
        {
            interactionInLevel.Clear();
            foreach (GameObject interact in GameObject.FindGameObjectsWithTag("Interact"))
            {
                interactionInLevel.Add(interact);
            }
            levelM.levelChanged = false;
        }
    }

    public void StartInteract(KF_Dialogue dialogue)
    {
        interactDone = false;
        dialogueAnim.SetBool("isOpen", true);
        sentences.Clear();
        if (objectDialogue == true)
        {
            ChangeToObject();
        }
        if (objectDialogue == false)
        {
            ChangeToDialogue();
        }
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void SecondStartInteract(KF_DialogueSecond dialogueSecond)
    {
        interactDone = false;
        sentences.Clear();
        if (objectDialogue == true)
        {
            ChangeToObject();
        }
        if (objectDialogue == false)
        {
            ChangeToDialogue();
        }
        dialogueAnim.SetBool("isOpen", true);
        foreach (string sentence2 in dialogueSecond.sentences2)
        {
            sentences.Enqueue(sentence2);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndInteraction();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndInteraction()
    {
        dialogueAnim.SetBool("isOpen", false);
        if (objectDialogue == true)
            objectDialogue = false;
        Debug.Log("Dialogue End");
        KF_ResetInteract.dialogueReset = 0;
        interactDone = true;
        dialogueText.text = "";
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(waitTime);
        }
    }

    void ChangeToObject()
    {
        dialogueBoxSP.sprite = objectBox;
        var tempColor = dialogueBoxSP.color;
        tempColor.a = 255f;
        dialogueBoxSP.color = tempColor;
    }

    void ChangeToDialogue()
    {
        var tempColor = dialogueBoxSP.color;
        tempColor.a = 230f;
        dialogueBoxSP.color = tempColor;
        dialogueBoxSP.sprite = dialogueBox;
    }
}
