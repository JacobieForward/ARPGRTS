using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AnimationController))]
public class Movement : MonoBehaviour {
    Stats stats;
    NavMeshAgent navMeshAgent;
    AnimationController animationController;

    float minFollowDistance = 2.0f; // TODO: Put in Constants

    void Awake() {
        stats = GetComponent<Stats>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = stats.speed;
        animationController = GetComponent<AnimationController>();
    }

    void Update() {
        CheckForNoMovement();
    }

    void CheckForNoMovement() {
        if (!navMeshAgent.pathPending) {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                    animationController.GetAnimator().SetBool("moving", false);
                }
            }
        }
    }

    public void MoveToPosition(Vector3 positionToMoveTo) {
        if (positionToMoveTo == null) {
            Debug.Log("Tried to move to null target.");
            return;
        }
        navMeshAgent.destination = positionToMoveTo;
        navMeshAgent.speed = stats.speed * Mathf.Clamp01(1f);
        navMeshAgent.isStopped = false;
        animationController.GetAnimator().SetBool("moving", true);
    }

    public bool CanMoveTo(Vector3 destination) {
        NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
        if (!hasPath) return false;
        if (path.status != NavMeshPathStatus.PathComplete) return false;

        return true;
    }

    public void StopMovement() {
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.SetDestination(gameObject.transform.position); // Setting destination to current position stops movement immediately
        animationController.GetAnimator().SetBool("moving", false);
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

    public bool WithinFollowRangeOfTarget(float followRange, Vector3 followTargetPosition) {
        float distance = Vector3.Distance(transform.position, followTargetPosition);
        if (distance <= followRange) {
            return true;
        }
        return false;
    }
}
