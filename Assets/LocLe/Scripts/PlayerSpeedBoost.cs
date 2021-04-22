using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Platformer.Mechanics;

public class PlayerSpeedBoost : MonoBehaviour
{
    public PlayerController player;
    private Image img = null;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (img != null && player != null)
            img.enabled = player.Hasten();
    }
}
