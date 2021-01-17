using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_TutorialDoorSupport : MonoBehaviour
{
    public BoxCollider2D referenceDoor;
    private BoxCollider2D thisDoor;


    // Start is called before the first frame update
    void Start()
    {
        thisDoor = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (referenceDoor.isTrigger == true)
            thisDoor.isTrigger = true;
        if (referenceDoor.isTrigger == true)
            thisDoor.isTrigger = true;
    }
}
