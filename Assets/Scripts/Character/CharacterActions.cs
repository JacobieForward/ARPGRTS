using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterActions : MonoBehaviour {
    CharacterStats characterStats;

    float attackTimer;

    void Awake() {
        characterStats = GetComponent<CharacterStats>();
    }

    public void Attack(GameObject attackTarget) {
        // For now assumes target is of type Enemy and containing all scripts required of an enemy
        if (attackTimer >= characterStats.attackSpeed) {
            attackTarget.GetComponent<CharacterStats>().health -= characterStats.attackPower;
            attackTimer = 0.0f;
        }
    }

    void FollowTarget(GameObject targetToFollow) {

    }

    void Update() {
        CheckForDeath();
        attackTimer += Time.deltaTime;
    }

    void CheckForDeath() {
        if (characterStats.health <= 0.0f) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}
