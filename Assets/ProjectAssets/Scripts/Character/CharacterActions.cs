using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterStats))]
public class CharacterActions : MonoBehaviour {
    CharacterStats characterStats;
    NavMeshAgent navMeshAgent;

    float attackTimer;

    public GameObject currentTarget;

    bool approachingTarget = false;

    void Awake() {
        characterStats = GetComponent<CharacterStats>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        attackTimer = characterStats.attackSpeed;
    }

    void Update() {
        CheckForDeath();
        attackTimer += Time.deltaTime;

        if (approachingTarget) {
            if (!WithinAttackRangeOfTarget()) {
                //ApproachTarget();
            } else if (!FacingTarget()){
                TurnToTarget();
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
        if (!WithinAttackRangeOfTarget()) {
            ApproachTarget();
            approachingTarget = true;
        } else if (attackTimer >= characterStats.attackSpeed) {
            if (FacingTarget()) {
                DealDamageToTarget();
                approachingTarget = false;
            } else {
                TurnToTarget();
                approachingTarget = true;
            }
        }
    }

    void ApproachTarget() {
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
            Debug.Log("Face Target ran with no Target");
            return false;
        }
        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
        float DotProd = Vector3.Dot(direction, transform.forward);
        if (DotProd > 0.9){
            return true;
        }
        return false;
    }

    void TurnToTarget() {
        if (currentTarget == null) {
            Debug.Log("Turn To Target ran with no Target");
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
        Debug.Log(targetRotation.ToString());
        float str = Mathf.Min(characterStats.speed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
    }
}
