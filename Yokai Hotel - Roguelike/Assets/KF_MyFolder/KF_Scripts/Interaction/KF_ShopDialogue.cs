using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KF_ShopDialogue
{
    [TextArea(3, 6)]
    public string[] nameObj;
    [TextArea(3, 6)]
    public string[] description;
    [TextArea(3, 6)]
    public string[] price;
}
