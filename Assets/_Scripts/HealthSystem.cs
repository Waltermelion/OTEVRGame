using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem {
    private int health;


    public HealthSystem(int health) {
        this.health = health;
    }

    public int GetHealth() { return health; }

    public void TakeDamage(int damage) {
        health -= damage;
    }

    public bool IsHealth() { 
        return health <= 1; 
    }

    public bool IsDead() {
        if (health <= 0) return true;
        return false;
    }
}
