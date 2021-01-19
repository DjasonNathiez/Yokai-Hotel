using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KF_Interaction : MonoBehaviour
{
    public KF_Dialogue dialogue;
    public GameObject interactionIcon;
    public GameObject exclamation;
    public UnityEvent interactAction;
    public UnityEvent interactContinue;
    public KF_InteractionManager intM;
    public GameObject thisInteract;
    public bool inRange;
    private bool inDialogue;

    [Header("==== OPTIONS ====")]
    public bool isInMultiple;
    public bool givesSomething;
    public bool exclamationPoint;
    public bool objectDialogue;
    public bool objectEffect;
    public bool effectIsRemove;
    public GameObject effectTarget;
    public bool effectReversable;
    public bool reverse;
    private bool hastalked;
    public bool secondDialogue;
    public KF_DialogueSecond dialogue2;
    public UnityEvent interactOtherStart;
    private bool secondDialogueStart;
    private bool onlyOnce;

    [Header("==== DO NOT TOUCH ====")]
    public bool isGiven;
    public int counter;
    public int counting;
    public bool endOfDialogue;


    // Start is called before the first frame update
    void Start()
    {
        intM = FindObjectOfType<KF_InteractionManager>();
        foreach (GameObject interact in intM.interactionInLevel)
        {
            if (interact.Equals(this.gameObject))
                thisInteract = interact;
        }
        interactionIcon.SetActive(false);

        if (exclamationPoint == true)
            exclamation.SetActive(true);
        else
            exclamation.SetActive(false);

        if (secondDialogue == false)
        {
            dialogue2.sentences2 = dialogue.sentences;
        }
        if ((reverse == true) && (effectIsRemove == true))
        {
            effectTarget.SetActive(false);
        }
        if ((reverse == true) && (effectIsRemove == false))
        {
            effectTarget.SetActive(true);
        }
        counter = dialogue.sentences.Length;
    }

    private void Update()
    {
        if (inRange == true)
        {
            if (Input.GetButtonDown("Interact") && (KF_ResetInteract.dialogueReset != 1))
            {
                onlyOnce = true;
                Debug.Log("Dialogue Start");
                KF_ResetInteract.dialogueReset = 1;
                if (effectReversable == false)
                {
                    hastalked = false;
                    reverse = false;
                }
                if (givesSomething == true)
                {
                    isGiven = true;
                }
                if (objectDialogue == true)
                {
                    intM.objectDialogue = true;
                    if ((objectEffect == true) && (reverse == false))
                    {
                        if (effectIsRemove == true)
                            effectTarget.SetActive(false);
                        if (effectIsRemove == false)
                            effectTarget.SetActive(true);
                        secondDialogueStart = false;
                    }
                    if ((effectReversable == true) && (objectEffect == true) && (reverse == true))
                    {
                        if (effectIsRemove == true)
                            effectTarget.SetActive(true);
                        if (effectIsRemove == false)
                            effectTarget.SetActive(false);
                        hastalked = false;
                        secondDialogueStart = true;
                    }
                }
                if ((hastalked == false) && (secondDialogueStart == false))
                {
                    reverse = true;
                    hastalked = true;
                    interactAction.Invoke();
                }     
                if ((secondDialogue == true) && (secondDialogueStart == true))
                {
                    reverse = false;
                    hastalked = false;
                    interactOtherStart.Invoke();
                }
                inDialogue = true;
                return;
            }
            else if (Input.GetButtonDown("Interact") && (KF_ResetInteract.dialogueReset == 1))
            {
                counting++;
                endOfDialogue = false;
                if (counting == counter)
                {
                    counting = 0;
                    endOfDialogue = true;
                }
                Debug.Log("Dialogue Next Line");
                interactContinue.Invoke();
                return;
            }
        }
    }

    public void TriggerInteract()
    {
        intM.StartInteract(dialogue);
    }

    public void TriggerContinue()
    {
        intM.DisplayNextSentence();
    }

    public void TriggerOtherInteract()
    {
        intM.SecondStartInteract(dialogue2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
            interactionIcon.SetActive(true);
            foreach (GameObject interact in intM.interactionInLevel)
            {
                if ((interact.Equals(this.gameObject)) || (interact.GetComponent<KF_Interaction>().isInMultiple == true))
                    continue;
                interact.SetActive(false);
            }

            if ((exclamationPoint == true) && (onlyOnce == false))
                exclamation.SetActive(false);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            interactionIcon.SetActive(false);
            foreach (GameObject interact in intM.interactionInLevel)
            {
                if ((interact.GetComponent<KF_Interaction>().isInMultiple == true) || (interact == thisInteract))
                    continue;
                interact.SetActive(true);
            }
            if (inDialogue == true)
            {
                inDialogue = false;
                endOfDialogue = true;
                intM.EndInteraction();
            }
            if ((exclamationPoint == true) && (onlyOnce == false))
                exclamation.SetActive(true);
        }
    }
}
