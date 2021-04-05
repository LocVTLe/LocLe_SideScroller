using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update

    public Collider2D player = null;
    public GameObject spawnPoint = null;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player != null && other == player)
        {
            if (spawnPoint != null) spawnPoint.transform.position = this.transform.position;
        }
    }

}
