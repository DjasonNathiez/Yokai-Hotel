using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetRenderer : MonoBehaviour
{
    public SpriteRenderer m_spriteRenderer;
    public Color fadeColor;
    public float fadeTime, fadeDuartion, alphaRef;

    // Update is called once per frame
    void Update()
    {
        fadeTime -= Time.deltaTime;
        fadeColor.a = (fadeTime / fadeDuartion) * alphaRef;
        m_spriteRenderer.color = fadeColor;
    }
}
