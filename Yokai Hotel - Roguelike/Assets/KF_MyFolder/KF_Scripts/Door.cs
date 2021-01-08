using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType { NORMAL, SPECIAL, FINAL }
    public DoorType type;

    public Vector2 dir;
    public bool open;


    public Door doorLink;
    public GameObject playerObj;
    public BoxCollider2D doorTriggerZone;
    public GameObject temporaryShow;
   

    public SpriteRenderer sprite;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        doorTriggerZone = this.gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (doorLink == null)
        {
            doorTriggerZone.isTrigger = false;
            temporaryShow.SetActive(false);
        }
        if (doorTriggerZone.isTrigger == false)
        {
            temporaryShow.SetActive(false);
        }
        if (doorTriggerZone.isTrigger == true)
        {
            temporaryShow.SetActive(true);
        }
    }             



    public void UpdateDoor(int doorType, bool isOpen)
    {
        type = (Door.DoorType)doorType;
        open = isOpen;

        // show color difine by type and open
        Color doorColor = Color.gray;
        switch (type)
        {
            case DoorType.NORMAL:
                {
                    doorColor = Color.green;
                    break;
                }
            case DoorType.SPECIAL:
                {
                    doorColor = Color.blue;
                    break;
                }

            case DoorType.FINAL:
                {
                    doorColor = Color.red;
                    break;
                }
        }
        doorColor.a = (open) ? 1 : 0;
        sprite.color = doorColor;
    } 

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Teleport();

        }
    }

    private void Teleport()
    {
        playerObj.transform.position = doorLink.GetComponent<Transform>().position;
    }
}
