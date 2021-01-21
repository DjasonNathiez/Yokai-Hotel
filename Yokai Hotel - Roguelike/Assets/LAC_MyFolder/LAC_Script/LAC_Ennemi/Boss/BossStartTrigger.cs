using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BossStartTrigger : MonoBehaviour
{
    public bool playerReady;
    PlayerController player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.GetComponent<PlayerController>();
            playerReady = true;

            player.manageDir = Vector2.up;
            player.manageSpeed = 0;
            player.playerState = PlayerController.PlayerState.MANAGE;

            StartCoroutine(EndIntro(6.5f));
            Debug.Log("StartBoss battle");
        }
    }

    public IEnumerator EndIntro(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.playerState = PlayerController.PlayerState.FREE;
        gameObject.SetActive(false);

    }
}
