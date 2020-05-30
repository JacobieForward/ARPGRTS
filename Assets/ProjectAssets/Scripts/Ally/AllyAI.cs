using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Stats))]
public class AllyAI : MonoBehaviour {
    Combat combat;
    Movement movement;
    Stats stats;
    //TODO: Some sort of state machine in the combat script. Possibly in to the future to tie in to another script for a group of enemies to have intelligence and make tactical decisions.

    public float searchRadius;

    void Awake() {
        combat = GetComponent<Combat>();
        stats = GetComponent<Stats>();
    }

    void Update() {
        if (combat.currentTarget == null) {
            SearchForTarget();
        } else if (combat.approachingTarget == false) {
            combat.BasicAttack();
        } else {
            // Follow the player
            FollowPlayer();
        }
    }

    void SearchForTarget() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider collider in hitColliders) {
            if (collider.gameObject.tag.Equals("Enemy")) {
                combat.currentTarget = collider.gameObject;
                combat.BasicAttack();
            }
        }
    }

    void FollowPlayer() {
        // If outside of attack distance from player
        // Then move and turn to player
    }
}
