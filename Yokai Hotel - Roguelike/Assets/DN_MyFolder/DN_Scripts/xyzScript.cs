using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class xyzScript : MonoBehaviour
{
    public TextMeshProUGUI xyzText;
    public Transform room;
    public Transform gameObjectFind;
    public Transform camera;

    private void Update()
    {
        xyzText.text = room.transform.position.ToString() + "\n" + gameObjectFind.transform.position.ToString() + "\n" + camera.transform.position.ToString();
    }
}
