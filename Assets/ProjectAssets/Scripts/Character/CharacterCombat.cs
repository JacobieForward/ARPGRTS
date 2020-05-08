using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterCombat : MonoBehaviour {
    CharacterStats characterStats;
    NavMeshAgent navMeshAgent;

    float attackTimer;

    public GameObject currentTarget;

    public bool approachingTarget;

    void Awake() {
        characterStats = GetComponent<CharacterStats>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        attackTimer = characterStats.attackSpeed;
        approachingTarget = false;
    }

    void Update() {
        CheckForDeath();
        attackTimer += Time.deltaTime;
        if (approachingTarget) {
            navMeshAgent.destination = currentTarget.transform.position;
            TurnToTarget();
            if (WithinAttackRangeOfTarget() && FacingTarget()) {
                approachingTarget = false;
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.SetDestination(gameObject.transform.position); // Setting destination to current position stops movement immediately
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

    void MoveToTarget() {
        navMeshAgent.SetDestination(currentTarget.transform.position);
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

    void TurnToTarget() {
        if (currentTarget == null) {
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
        float str = Mathf.Min(characterStats.speed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
    }
}
