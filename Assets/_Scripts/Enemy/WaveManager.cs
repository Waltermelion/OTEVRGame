using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    public GameObject[] enemiesSpawnPoint;
    public Transform enemiesParent;
    public float spawnTime;
    public int currentWaveIndex = 0;
    public bool readyToCoutDown = false;

    [NonReorderable]
    public Wave[] waves;


    private void Start() {
        for (int i = 0; i < waves.Length; i++) {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }

    private void Update() {
        if (currentWaveIndex >= waves.Length) {
            Debug.Log("WIN");
            return;
        }

        if (readyToCoutDown) {
            spawnTime -= Time.deltaTime;

        }

        if (spawnTime <= 0) {
            readyToCoutDown = false;

            spawnTime = waves[currentWaveIndex].timeToNextWave;
            StartCoroutine(WaveSpawner());
        }

        if (waves[currentWaveIndex].enemiesLeft == 0) {
            readyToCoutDown = true;
            currentWaveIndex++;
        }
    }

    private IEnumerator WaveSpawner() {
        if (currentWaveIndex <= waves.Length) {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++) {
                GameObject enemiesClone = Instantiate(waves[currentWaveIndex].enemies[i], enemiesSpawnPoint[Random.Range(0, enemiesSpawnPoint.Length)].transform.position, Quaternion.identity);

                enemiesClone.transform.SetParent(enemiesParent);

                yield return new WaitForSeconds(waves[currentWaveIndex].spawNextEnemy);
            }
        }
    }
}

[System.Serializable]
public class Wave {
    public GameObject[] enemies;
    public float spawNextEnemy;
    public float timeToNextWave;

    [HideInInspector]
    public int enemiesLeft;
}
