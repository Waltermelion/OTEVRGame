using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public static Action OnSpawnEvent;

    public GameObject enemyRagdoll;
    public GameObject bloodPS;

    public LayerMask enemyLayerMask;

    private WaveManager waveManager;

    GameObject enemyRagdollClone;
    GameObject bloodPSClone;


    private void Start() {
        waveManager = FindObjectOfType<WaveManager>();
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, enemyLayerMask)) {
                hitInfo.collider.transform.parent.GetComponent<IHittable>()?.GetHit();
            }
        }
        if (Input.GetKey(KeyCode.F)) {
            waveManager.readyToCoutDown = true;
        }
    }

    public void HitEnemy(GameObject hitEnemy) {
        if (hitEnemy.GetComponent<EnemyController>()._healthSystem.IsHealth()) {
            foreach (Renderer variableName in hitEnemy.GetComponentsInChildren<Renderer>()) {
                variableName.material.color = Color.red;
            }

            hitEnemy.transform.GetChild(0).GetChild(0).GetComponentInChildren<Renderer>().material.color = Color.black;
            hitEnemy.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = Color.white;
            hitEnemy.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.color = Color.white;
        }
        if (hitEnemy.GetComponent<EnemyController>()._healthSystem.IsDead()) {
            bloodPSClone = Instantiate(bloodPS, new Vector3(hitEnemy.transform.position.x, 2, hitEnemy.transform.position.z), Quaternion.identity);
            Destroy(hitEnemy);
            waveManager.waves[waveManager.currentWaveIndex].enemiesLeft--;
            enemyRagdollClone = Instantiate(enemyRagdoll, hitEnemy.transform.position, Quaternion.identity);
            bloodPSClone.transform.SetParent(enemyRagdollClone.transform);
            enemyRagdollClone.transform.GetChild(3).GetComponent<Rigidbody>().AddForce(Vector3.forward * 2000, ForceMode.Force);
            enemyRagdollClone.GetComponent<EnemyRagdoll>().DestroyThisObject();

        } else {
            bloodPSClone = Instantiate(bloodPS, new Vector3(hitEnemy.transform.position.x, 2, hitEnemy.transform.position.z), Quaternion.identity);
            bloodPSClone.transform.SetParent(hitEnemy.transform.parent);
        }
    }
}

