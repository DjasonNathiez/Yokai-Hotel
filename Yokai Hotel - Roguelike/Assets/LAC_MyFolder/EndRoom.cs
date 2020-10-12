using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoom : MonoBehaviour
{
    GameObject postPro1,postPro2;

    Camera cam;
    public Vector2 boundMin, boundMax;
    public LayerMask detectMask;

    public bool roomClean;
    // Start is called before the first frame update
    void Start()
    {
        postPro1 = GameObject.Find("Global Volume jour");
        postPro2 = GameObject.Find("Global Volume nuit");

        cam = GetComponent<Camera>();

    }

    private void Update()
    {
        if (cam)
        {
            float height = cam.orthographicSize * 2;
            float width = cam.aspect * height;

            boundMin = new Vector2(transform.position.x - height * 0.85f, transform.position.y - width*0.25f );
            boundMax = new Vector2(transform.position.x + height *0.85f, transform.position.y + width * 0.25f);

            Collider2D hit = Physics2D.OverlapArea(boundMin, boundMax,detectMask);
            roomClean = !hit;
        }

        if ( postPro1 && postPro2)
        {
            postPro1.SetActive(roomClean);
            postPro2.SetActive(!roomClean);
        }
            
    }

}
