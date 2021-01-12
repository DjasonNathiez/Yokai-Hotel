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
         
    void Start()
    {
        sentences = new Queue<string>();
        waitTime = 0;
        dialogueText = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<Text>();
        levelM = FindObjectOfType<KF_LevelManager>();
    }
    private void Update()
    {
        if (levelM.levelChanged == true)
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
        dialogueAnim.SetBool("isOpen", true);
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
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
        Debug.Log("Dialogue End");
        KF_ResetInteract.dialogueReset = 0;
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
}
