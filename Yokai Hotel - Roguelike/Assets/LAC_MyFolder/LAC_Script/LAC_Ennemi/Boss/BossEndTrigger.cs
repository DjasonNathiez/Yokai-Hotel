using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossEndTrigger : MonoBehaviour
{
    PlayerController player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.GetComponent<PlayerController>();

            player.manageDir = Vector2.down;
            player.manageSpeed = 0;
            player.playerState = PlayerController.PlayerState.MANAGE;

            StartCoroutine(EndGame(3));
            Debug.Log("EndBoss battle");
        }
    }

    public IEnumerator EndGame(float delay)
    {
        yield return new WaitForSeconds(delay);

        int restartIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(restartIndex);

    }
}
