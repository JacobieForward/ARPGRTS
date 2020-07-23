using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {
    public float maxHealth;
    public float currentHealth;
    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public float speed;

    void Awake() {
        currentHealth = maxHealth;
    }
}
