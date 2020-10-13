using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    Camera cam;
    public float camWidth, camHeight;
    public Vector2 camBoundMin, camBoundMax;

    public DataRoom[] dataRooms;

    // Start is called before the first frame update
    void Start()
    {
        #region camera
        GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (camObj)
            cam = camObj.GetComponent<Camera>();

        camWidth = 1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);
        camHeight = 1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f);

        camBoundMin = new Vector2(-camWidth / 2, -camHeight / 2);
        camBoundMax = new Vector2(camWidth / 2, camHeight / 2);
        #endregion
        #region dataRoom
        GameObject[] roomObj = GameObject.FindGameObjectsWithTag("Room");// Get all room in scene
        dataRooms = new DataRoom[roomObj.Length]; // set Dataroom length

        for(int i = 0; i < roomObj.Length; i++)
        {
            dataRooms[i].room = roomObj[i];
            dataRooms[i].SetupPos(camWidth, camHeight);
            GameObject[] door = roomObj.
        }

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public struct DataRoom
    {
        public GameObject room;
        public Vector2 roomPos;

        public GameObject[] doors;
        public void SetupPos( float xSize, float ySize)
        {
            roomPos.x = Mathf.Floor(((room.transform.position.x / xSize)-0.5f));
            roomPos.y = Mathf.Floor(((room.transform.position.x / ySize) -0.5f));
        }
    }
}
