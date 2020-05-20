using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour {
    Stats stats;
    NavMeshAgent navMeshAgent;
    Animator animator;

    void Awake() {
        stats = GetComponent<Stats>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = stats.speed;
        animator = GetComponent<Animator>();
    }

    void Update() {
        CheckForNoMovement();
    }

    void CheckForNoMovement() {
        if (!navMeshAgent.pathPending) {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                    animator.SetBool("moving", false);
                }
            }
        }
    }

    public void MoveToPosition(Vector3 positionToMoveTo) {
        if (positionToMoveTo == null) {
            Debug.Log("Tried to move to null target.");
            return;
        }
        navMeshAgent.SetDestination(positionToMoveTo);
        animator.SetBool("moving", true);
    }

    public void StopMovement() {
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.SetDestination(gameObject.transform.position); // Setting destination to current position stops movement immediately
        animator.SetBool("moving", false);
    }

    public void TurnToPosition(Vector3 positionToTurnTo) {
        if (positionToTurnTo == null) {
            Debug.Log("Tried to turn to null position.");
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(positionToTurnTo - gameObject.transform.position);
        float str = Mathf.Min(stats.speed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, str);
    }
}
