using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType {NULL, START, COMMUN, RARE, RISK, SHOP, KEY, PREPAREND, END };
    public RoomType type;

    public Vector2 roomPos;
    public Door[] doors;
    public SpriteRenderer sprite;

    public bool keyToSpawn;
    private void Awake()
    {
        doors = GetComponentsInChildren<Door>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void UpdateRoom(int roomType)
    {
        type = (RoomType)roomType;
        Color roomColor = Color.black;
        switch (type)
        {
            case RoomType.START:
                {
                    //roomColor = Color.green;
                    roomColor = Color.white;
                    break;
                }
            case RoomType.COMMUN:
                {
                    roomColor = Color.white;
                    break;
                }

            case RoomType.RARE:
                {
                    roomColor = Color.grey;
                    break;
                }
            case RoomType.RISK:
                {
                    roomColor = new Color(1,0,1);
                    break;
                }

            case RoomType.SHOP:
                {
                    //roomColor = new Color(0, 0.5f, 0);
                    roomColor = Color.white;
                    break;
                }
            case RoomType.KEY:
                {
                    //roomColor = Color.cyan;
                    roomColor = Color.white;
                    break;
                }
            case RoomType.PREPAREND:
                {
                    //roomColor = new Color(0.5f, 0, 0.5f);
                    roomColor = Color.white;
                    break;
                }
            case RoomType.END:
                {
                    //roomColor = new Color(1, 0, 0);
                    roomColor = Color.white;
                    break;
                }   
        }
        sprite.color = roomColor;
    }
    public void UpdateDoors(int doorNumber, int doorType, bool isOpen)
    {
        doors[doorNumber].UpdateDoor(doorType, isOpen);
    }
    
}
