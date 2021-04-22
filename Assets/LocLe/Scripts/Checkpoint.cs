using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerController player = null;
    public GameObject spawnPoint = null;
    public GameObject dialog = null;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player != null && other == player.collider2d)
        {
            if (spawnPoint != null) spawnPoint.transform.position = this.transform.position;
            if (dialog != null) dialog.SetActive(true);
            if (player.audioSource && player.interactAudio)
                player.audioSource.PlayOneShot(player.interactAudio);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (player != null && other == player.collider2d)
        {
            //if (spawnPoint != null) spawnPoint.transform.position = this.transform.position;
            if (dialog != null) dialog.SetActive(false);
        }
    }

}
