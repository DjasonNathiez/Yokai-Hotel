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
    private Animator dialogueAnim;
    public GameObject thisInteract;
    public bool inRange;

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
        dialogueAnim = GameObject.FindGameObjectWithTag("DialogueAnim").GetComponent<Animator>();
    }

    private void Update()
    {
        if (inRange == true)
        {
            if (Input.GetButtonDown("Interact") && (KF_ResetInteract.dialogueReset != 1))
            {
                Debug.Log("Dialogue Start");
                KF_ResetInteract.dialogueReset = 1;
                interactAction.Invoke();
                return;
            }
            else if (Input.GetButtonDown("Interact") && (KF_ResetInteract.dialogueReset == 1))
            {
                if (intM.sentences.Count == 0)
                {
                    KF_ResetInteract.dialogueReset = 0;
                    Debug.Log("Dialogue End");
                    dialogueAnim.SetBool("isOpen", false);
                    return;
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
        }
    }
}
