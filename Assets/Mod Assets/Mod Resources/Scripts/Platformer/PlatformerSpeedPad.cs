using System.Collections;
using UnityEngine;
using Platformer.Mechanics;

public class PlatformerSpeedPad : MonoBehaviour
{
    public float additionalSpeed;

    [Range (0, 5)]
    public float duration = 1f;
    private bool isActivated = false;

    void OnTriggerEnter2D(Collider2D other){
        var rb = other.attachedRigidbody;
        if (rb == null) return;
        var player = rb.GetComponent<PlayerController>();
        if (player == null) return;

        if (isActivated == false)
        {
            isActivated = true;

            if (player.trailPrefab != null) player.trailPrefab.SetActive(isActivated);

            player.StartCoroutine(PlayerModifier(player, duration));
        }
    }

    IEnumerator PlayerModifier(PlayerController player, float lifetime)
    {
        float trailTime = 0.0f;

        if (player.trailPrefab != null)
        {
            player.trailPrefab.SetActive(true);
            trailTime = player.trailPrefab.GetComponent<TrailRenderer>().time;
        }

        player.maxSpeed += additionalSpeed;
        yield return new WaitForSeconds(duration * 0.8f);

        while (player.maxSpeed > player.GetOriginalSpeed())
        {
            player.maxSpeed -= additionalSpeed / 30;
            player.trailPrefab.GetComponent<TrailRenderer>().time -= trailTime / 30;
            yield return new WaitForSeconds(0.05f);
        }

        player.maxSpeed = player.GetOriginalSpeed();

        isActivated = false;
        if (player.trailPrefab != null)
        {
            player.trailPrefab.SetActive(false);
            player.trailPrefab.GetComponent<TrailRenderer>().time = trailTime;
        }

    }

}
