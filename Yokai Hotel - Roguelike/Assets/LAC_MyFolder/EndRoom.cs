using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoom : MonoBehaviour
{
    GameObject postPro1,postPro2;

    Camera cam;
    public float camWidth, camHeight;
    public Vector2 camBoundMin, camBoundMax;
    public LayerMask detectMask;

    public bool roomClean;
    // Start is called before the first frame update
    void Start()
    {
        postPro1 = GameObject.Find("Global Volume jour");
        postPro2 = GameObject.Find("Global Volume nuit");

        cam = GetComponent<Camera>();

        camWidth = 1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);
        camHeight = 1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f);

        camBoundMin = new Vector2(-camWidth / 2, -camHeight / 2);
        camBoundMax = new Vector2(camWidth / 2, camHeight / 2);

    }

    private void Update()
    {
        if (cam)
        {
            float height = cam.orthographicSize * 2;
            float width = cam.aspect * height;

            camBoundMin = new Vector2(transform.position.x - height * 0.85f, transform.position.y - width*0.25f );
            camBoundMax = new Vector2(transform.position.x + height *0.85f, transform.position.y + width * 0.25f);

            Collider2D hit = Physics2D.OverlapArea(camBoundMin, camBoundMax,detectMask);
            roomClean = !hit;
        }

        if ( postPro1 && postPro2)
        {
            postPro1.SetActive(roomClean);
            postPro2.SetActive(!roomClean);
        }
            
    }

}
