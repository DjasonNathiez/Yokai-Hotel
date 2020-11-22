using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingAid : MonoBehaviour
{
    public GameObject bullet;
    public float speed;
    private void Update()
    {
        bullet = GameObject.FindGameObjectWithTag("BulletAlly");

        if(bullet)
        transform.position = bullet.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit Something" + collision.gameObject.name);

        if (collision.gameObject.tag == "Ennemi")
        {
            if(bullet)
            bullet.transform.position = Vector3.MoveTowards(
                new Vector3(bullet.transform.position.x, bullet.transform.position.y, 0), 
                new Vector3(collision.transform.position.x, collision.transform.position.y, 0), 
                speed * Time.deltaTime);

            Debug.Log("EnnemiFind");
        }
    }

}
