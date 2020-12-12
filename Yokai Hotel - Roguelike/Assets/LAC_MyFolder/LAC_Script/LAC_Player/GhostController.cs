using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public GameObject GhostPrefab;

    public SpriteRenderer spriteToGhost;
    public Color colorFilter;

    public float spawnDelay, destroyTime;
    float delta;

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime; 
        if (delta >= spawnDelay){ CreateGhost(); delta = 0; }
    }
    void CreateGhost()
    {
        GameObject ghostObj = Instantiate(GhostPrefab, transform.position, transform.rotation);
        SpriteRenderer ghostSprite = ghostObj.AddComponent<SpriteRenderer>();

        ghostSprite.sprite = spriteToGhost.sprite;
        ghostSprite.material = spriteToGhost.material;
        ghostSprite.color = colorFilter;

        Destroy(ghostObj, destroyTime);
    }
}
