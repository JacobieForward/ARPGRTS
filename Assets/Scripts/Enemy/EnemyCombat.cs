using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCombat : MonoBehaviour {
    CharacterActions characterActions;
    CharacterStats characterStats;
    NavMeshAgent navMeshAgent;

    [SerializeField]
    GameObject currentTarget; // Serialized for debugging

    public float searchRadius;

    void Awake() {
        characterActions = GetComponent<CharacterActions>();
        characterStats = GetComponent<CharacterStats>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        // TODO: Very messy conditionals, clean up later
        if (currentTarget == null) {
            SearchForTarget();
        } else {
            if (WithinAttackRangeOfTarget()) {
                AttackTarget();
            } else {
                ApproachTarget();
            }
        }
    }

    void SearchForTarget() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider collider in hitColliders) {
            if (collider.gameObject.tag.Equals("Player")) {
                currentTarget = collider.gameObject;
            }
        }
    }

    bool WithinAttackRangeOfTarget() {
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distance <= characterStats.attackRange) {
            return true;
        }
        return false;
    }

    void AttackTarget() {
        characterActions.Attack(currentTarget);
    }

    void ApproachTarget() {
        navMeshAgent.SetDestination(currentTarget.transform.position);
    }
}
