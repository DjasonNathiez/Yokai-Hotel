using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetRenderer : MonoBehaviour
{
    public SpriteRenderer m_spriteRenderer;
    public Color fadeColor;
    public Color waterColor;
    public float fadeTime, fadeDuartion, alphaRef;

    public LayerMask waterMask;
    public ParticleSystem dust;
    public ParticleSystem splash;
    // Update is called once per frame
    private void Start()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.2f, waterMask);
        if (hit)
        {
            splash.Play();
            //fadeColor = waterColor;
            
        }
            
        else
            dust.Play();
    }
    void Update()
    {
        

        fadeTime -= Time.deltaTime;
        fadeColor.a = (fadeTime / fadeDuartion) * alphaRef;
        m_spriteRenderer.color = fadeColor;

    }
}
