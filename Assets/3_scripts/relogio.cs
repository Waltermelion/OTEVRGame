using UnityEngine;
using System;
using TMPro; // Make sure to include the TextMeshPro namespace

public class relogio : MonoBehaviour
{
    public TextMeshProUGUI clockText; // Assign your TextMeshProUGUI in the inspector
    private TimeSpan gameTime;
    private float realTimeCounter = 0f;

    private void Start()
    {
        gameTime = new TimeSpan(10, 0, 0); // Start at 00:00 in-game time
    }

    private void Update()
    {
        realTimeCounter += Time.deltaTime;

        // Every 5 real-life seconds, add a minute to the game time
        if (realTimeCounter >= 5f) // 5 seconds
        {
            gameTime = gameTime.Add(TimeSpan.FromMinutes(1));
            realTimeCounter -= 5f; // Subtract the interval to avoid drift
        }

        // Update the TextMeshPro text to show the game time
        clockText.text = gameTime.ToString(@"hh\:mm");
    }
}

