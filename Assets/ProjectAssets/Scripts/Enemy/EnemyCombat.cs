using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(Outliner))] // Required for PlayerInput to outline this object if it is selected as a target
public class EnemyCombat : MonoBehaviour {
    Combat combat;
    Stats stats;
    //TODO: Some sort of state machine in a separate script. Possibly in to the future to tie in to another script for a group of enemies to have intelligence and make tactical decisions.

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
        }
    }

    void SearchForTarget() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider collider in hitColliders) {
            if (collider.gameObject.tag.Equals("Player")) {
                combat.currentTarget = collider.gameObject;
                combat.BasicAttack();
            }
        }
    }
}
