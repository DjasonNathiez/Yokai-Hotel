using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public bool isShaking;

    public float shakeDurationInitialisation;
    public float shakeDuration;
    public float shakePower;

    Vector3 localTransform;

    void Start()
    {
       localTransform = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (isShaking == true)
        {
            shakeDuration = shakeDurationInitialisation;
        }

        if (shakeDuration > 0)
        {
            shakeDuration -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;
            transform.position += new Vector3(xAmount, yAmount, 0f);

            isShaking = false;
        }

        if (shakeDuration == 0)
        {
            transform.position = localTransform;
        }

        if (shakeDuration < 0)
        {
            shakeDuration = 0;
        }
    }


}
