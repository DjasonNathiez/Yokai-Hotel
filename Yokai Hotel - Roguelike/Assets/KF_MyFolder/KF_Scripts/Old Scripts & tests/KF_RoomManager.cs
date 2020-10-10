using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_RoomManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> rooms = new List<GameObject>();
    private Camera roomCameras;
    private GameObject roomCamerasGameObject;
    [SerializeField]
    List<Camera> cameras = new List<Camera>();

    void Start()
    {
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
        {
            if (room.Equals(this.gameObject))
            {
                continue;

            }
            rooms.Add(room);
            foreach (GameObject kid in rooms) //I want to get the camera GO from the parent in the list. The parent wont have the cam tag
                {
                    roomCamerasGameObject = kid;
                    Camera cam = roomCamerasGameObject.GetComponentInChildren<Camera>();
                    cameras.Add(cam);
                }
        }

        foreach (Camera child in cameras)
            {
            roomCameras = child;
            roomCameras.gameObject.SetActive(false);
        }
    }
}
