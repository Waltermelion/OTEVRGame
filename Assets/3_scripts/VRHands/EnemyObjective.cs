using System;
using UnityEngine;

public class EnemyObjective : MonoBehaviour
{
    public HealthSystem _healthSystem;

    private void Start() {
        _healthSystem = new HealthSystem(20);
    }

    private void Update() {
        if (_healthSystem != null) {
            if (_healthSystem.IsDead()) {
                Destroy(gameObject);
            }
        }
    }
}
