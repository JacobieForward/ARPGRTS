using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterMovement))]
public class CharacterCombat : MonoBehaviour {
    CharacterStats characterStats;
    CharacterMovement characterMovement;

    float attackTimer;

    public GameObject currentTarget;

    public bool approachingTarget;

    void Awake() {
        characterStats = GetComponent<CharacterStats>();
        characterMovement = GetComponent<CharacterMovement>();

        attackTimer = characterStats.attackSpeed;
        approachingTarget = false;
    }

    void Update() {
        CheckForDeath();
        attackTimer += Time.deltaTime;
        if (approachingTarget) {
            characterMovement.MoveToPosition(currentTarget.transform.position);
            characterMovement.TurnToPosition(currentTarget.transform.position);
            if (WithinAttackRangeOfTarget() && FacingTarget()) {
                approachingTarget = false;
                characterMovement.StopMovement();
                Attack();
            }
        }
    }

    void CheckForDeath() {
        if (characterStats.currentHealth <= 0.0f) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }

    public void Attack() {
        if (currentTarget == null) {
            Debug.Log("Attack() called with no target selected.");
            return;
        }

        if (WithinAttackRangeOfTarget() && FacingTarget()) {
            approachingTarget = false;
            if (attackTimer >= characterStats.attackSpeed) {
                DealDamageToTarget();
            }
        } else {
            approachingTarget = true;
        }
    }

    public void Attack(GameObject newTarget) {
        currentTarget = newTarget;
        if (currentTarget == null) {
            Debug.Log("Attack(GameObject) called with null target passed.");
            return;
        }

        if (WithinAttackRangeOfTarget() && FacingTarget()) {
            approachingTarget = false;
            if (attackTimer >= characterStats.attackSpeed) {
                DealDamageToTarget();
            }
        } else {
            approachingTarget = true;
        }
    }

    bool WithinAttackRangeOfTarget() {
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distance <= characterStats.attackRange)
        {
            return true;
        }
        return false;
    }

    void DealDamageToTarget() {
        currentTarget.GetComponent<CharacterStats>().currentHealth -= characterStats.attackPower;
        attackTimer = 0.0f;
    }

    bool FacingTarget() {
        if (currentTarget == null) {
            return false;
        }
        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
        float DotProd = Vector3.Dot(direction, transform.forward);
        if (DotProd > 0.9) {
            return true;
        }
        return false;
    }
}
