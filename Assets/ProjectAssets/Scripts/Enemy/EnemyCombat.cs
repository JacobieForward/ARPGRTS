using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterCombat))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(Outliner))] // Required for PlayerInput to outline this object if it is selected as a target
public class EnemyCombat : MonoBehaviour {
    CharacterCombat characterCombat;
    CharacterStats characterStats;
    //TODO: Some sort of state machine in a separate script. Possibly in to the future to tie in to another script for a group of enemies to have intelligence and make tactical decisions.

    public float searchRadius;

    void Awake() {
        characterCombat = GetComponent<CharacterCombat>();
        characterStats = GetComponent<CharacterStats>();
    }

    void Update() {
        if (characterCombat.currentTarget == null) {
            SearchForTarget();
        } else if (characterCombat.approachingTarget == false) {
            characterCombat.Attack();
        }
    }

    void SearchForTarget() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider collider in hitColliders) {
            if (collider.gameObject.tag.Equals("Player")) {
                characterCombat.Attack(collider.gameObject);
            }
        }
    }
}
