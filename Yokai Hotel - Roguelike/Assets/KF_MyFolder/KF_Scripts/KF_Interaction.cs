using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KF_Interaction : MonoBehaviour
{
    public KF_Dialogue dialogue;
    public GameObject interactionIcon;
    public UnityEvent interactAction;
    public UnityEvent interactContinue;
    public KF_InteractionManager intM;
    public GameObject thisInteract;
    public bool inRange;
    private bool inDialogue;

    [Header("==== OPTIONS ====")]
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
    }

    private void Update()
    {
        if (inRange == true)
        {
            if (Input.GetButtonDown("Interact") && (KF_ResetInteract.dialogueReset != 1))
            {
                Debug.Log("Dialogue Start");
                KF_ResetInteract.dialogueReset = 1;
                if (effectReversable == false)
                {
                    hastalked = false;
                    reverse = false;
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
                        Debug.Log("ObjectNotReverse");
                        secondDialogueStart = false;
                    }
                    if ((effectReversable == true) && (objectEffect == true) && (reverse == true))
                    {
                        Debug.Log("ObjectReverse");
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
                    Debug.Log("1");
                    reverse = true;
                    hastalked = true;
                    interactAction.Invoke();
                }     
                if ((secondDialogue == true) && (secondDialogueStart == true))
                {
                    Debug.Log("2");
                    reverse = false;
                    hastalked = false;
                    interactOtherStart.Invoke();
                }
                inDialogue = true;
                return;
            }
            else if (Input.GetButtonDown("Interact") && (KF_ResetInteract.dialogueReset == 1))
            {
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
                if (interact.Equals(this.gameObject))
                    continue;
                interact.SetActive(false);
            }
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
                interact.SetActive(true);
            }
            if (inDialogue == true)
            {
                inDialogue = false;
                intM.EndInteraction();
            }

        }
    }
}
