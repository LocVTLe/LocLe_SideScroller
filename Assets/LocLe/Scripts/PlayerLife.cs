using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Platformer.Mechanics;

public class PlayerLife : MonoBehaviour
{
    public PlayerController player;
    private TextMeshProUGUI tmp = null;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tmp != null && player != null)
            tmp.SetText("x " + player.health.currentHP);
    }
}
