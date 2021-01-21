using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KF_KeyboardSupport : MonoBehaviour
{
    private bool keyboard; 
    public bool ui;
    public Sprite keyboardButton; //Don't forget to assign this!
    private Sprite controllerButton;
    private KF_Unlockables unlockables;


    void Start()
    {
        if (ui == false)
            controllerButton = this.gameObject.GetComponent<SpriteRenderer>().sprite;

        if (ui == true)
            controllerButton = this.gameObject.GetComponent<Image>().sprite;

        keyboard = FindObjectOfType<KF_Unlockables>().keyboard;
        if (keyboard == true)
        {
            if (ui == false)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = keyboardButton;
            }
            else
            {
                this.gameObject.GetComponent<Image>().sprite = keyboardButton;
            }
        }
        if (keyboard == false)
        {
            if (ui == false)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = controllerButton;
            }
            else
            {
                this.gameObject.GetComponent<Image>().sprite = controllerButton;
            }
        }
        unlockables = FindObjectOfType<KF_Unlockables>();
    }

    private void Update()
    {
        if ((unlockables.keyboard == true) && (unlockables.runOnce == true))
        {
            if (ui == false)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = keyboardButton;
            }
            else
            {
                this.gameObject.GetComponent<Image>().sprite = keyboardButton;
            }
            unlockables.runOnce = false;
        }
        if ((unlockables.keyboard == false) && (unlockables.runOnce == true))
        {
            if (ui == false)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = controllerButton;
            }
            else
            {
                this.gameObject.GetComponent<Image>().sprite = controllerButton;
            }
            unlockables.runOnce = false;
        }
    }

}
