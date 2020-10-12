using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class BasiqueEnnemiDist : MonoBehaviour
{
    Rigidbody2D rb2D;
    public enum EnnemyState { IDLE, AGGRO, ATTACK, WAIT, HURT, DIE }
    public EnnemyState ennemyState;

    public int healthPoints;

    public float speed;
    Vector2 velocity;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
