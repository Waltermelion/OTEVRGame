using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarWaveOnHit : MonoBehaviour, IHittable {

    private WaveManager waveManager;

    private void Start() {
        waveManager = FindObjectOfType<WaveManager>();
    }
    public void GetHit() {
        waveManager.readyToCoutDown = true;
    }
}
