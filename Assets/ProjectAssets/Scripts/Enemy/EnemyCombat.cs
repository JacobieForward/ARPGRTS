using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterActions))]
public class EnemyCombat : MonoBehaviour {
    CharacterActions characterActions;
    CharacterStats characterStats;

    public float searchRadius;

    void Awake() {
        characterActions = GetComponent<CharacterActions>();
        characterStats = GetComponent<CharacterStats>();
    }

    void Update() {
        // TODO: Very messy conditionals, clean up later
        if (characterActions.currentTarget == null) {
            SearchForTarget();
        } else {
            characterActions.Attack();
        }
    }

    void SearchForTarget() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider collider in hitColliders) {
            if (collider.gameObject.tag.Equals("Player")) {
                characterActions.currentTarget = collider.gameObject;
            }
        }
    }
}
