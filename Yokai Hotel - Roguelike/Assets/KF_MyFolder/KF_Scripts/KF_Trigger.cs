using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_Trigger : MonoBehaviour
{
    public GameObject triggerTarget;

    [Header("==== Effects ====")]
    public bool destroyObject;
    public bool onEnter;
    public bool onExit;
    public bool setActif;
    public bool setInactif;

    // Start is called before the first frame update
    void Start()
    {
        if (setActif == true)
            triggerTarget.SetActive(false);
        if (setInactif == false)
            triggerTarget.SetActive(true);
    }

    private void Update()
    {
        if (triggerTarget == null)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((destroyObject == true) && (onEnter == true))
        {
            Destroy(triggerTarget);
            destroyObject = false;
        }
        if ((setActif == true) && (onEnter == true))
        {
            triggerTarget.SetActive(true);
        }
        if ((setInactif == true) && (onEnter == true))
        {
            triggerTarget.SetActive(false);
        }
        if ((setActif == true) && (onExit == true))
        {
            triggerTarget.SetActive(false);
        }
        if ((setInactif == true) && (onExit == true))
        {
            triggerTarget.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((destroyObject == true) && (onExit == true))
        {
            Destroy(triggerTarget);
            destroyObject = false;
        }
        if ((setActif == true) && (onExit == true))
        {
            triggerTarget.SetActive(true);
        }
        if ((setInactif == true) && (onExit == true))
        {
            triggerTarget.SetActive(false);
        }
        if ((setActif == true) && (onEnter == true))
        {
            triggerTarget.SetActive(false);
        }
        if ((setInactif == true) && (onEnter == true))
        {
            triggerTarget.SetActive(true);
        }
    }
}

