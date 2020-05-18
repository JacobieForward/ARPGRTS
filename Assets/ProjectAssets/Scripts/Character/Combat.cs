using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(Movement))]
public class Combat : MonoBehaviour {
    // TODO: Better system for timers
    public float abilityOneTimer;
    public float abilityTwoTimer;

    Stats stats;
    Movement movement;

    float attackTimer;

    public GameObject currentTarget;

    public bool approachingTarget;

    AbilitiesLoader abilitiesLoader;

    void Awake() {
        stats = GetComponent<Stats>();
        movement = GetComponent<Movement>();

        attackTimer = stats.attackSpeed;
        approachingTarget = false;

        abilitiesLoader = FindObjectsOfType<AbilitiesLoader>()[0];
    }

    void Update() {
        CheckForDeath();
        attackTimer += Time.deltaTime;
        abilityOneTimer += Time.deltaTime;
        abilityTwoTimer += Time.deltaTime;
        if (approachingTarget) {
            movement.MoveToPosition(currentTarget.transform.position);
            movement.TurnToPosition(currentTarget.transform.position);
            if (WithinAttackRangeOfTarget(stats.attackRange) && FacingTarget()) {
                approachingTarget = false;
                movement.StopMovement();
                BasicAttack();
            }
        }
    }

    void CheckForDeath() {
        if (stats.currentHealth <= 0.0f) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }

    public void BasicAttack() {
        if (currentTarget == null) {
            Debug.Log("BasicAttack() called with no target selected.");
            return;
        }

        if (WithinAttackRangeOfTarget(stats.attackRange) && FacingTarget()) {
            approachingTarget = false;
            if (attackTimer >= stats.attackSpeed) {
                DealDamageToTarget(1.0f);
                attackTimer = 0.0f;
            }
        } else {
            approachingTarget = true;
        }
    }

    bool WithinAttackRangeOfTarget(float attackRange) {
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distance <= attackRange)
        {
            return true;
        }
        return false;
    }

    void DealDamageToTarget(float strength) {
        currentTarget.GetComponent<Stats>().currentHealth -= stats.attackPower * strength;
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

    // TODO: Check if the ability actually exists in the list of abilities stored in stats
    public void ActivateAbility(string abilityName) {
        Ability ability = abilitiesLoader.FetchAbilityByName(abilityName);
        if (currentTarget == null) {
            Debug.Log("ActivateAbility called with no target selected.");
            return;
        }
        // If Damaging Ability
        if (ability.type == 0) {
            // Check attack range
            if (WithinAttackRangeOfTarget(ability.range) && FacingTarget()) {
                if (attackTimer >= stats.attackSpeed && IsAbilityCooldownComplete(abilityName)) {
                    DealDamageToTarget(ability.power);
                    ResetAbilityCooldownForAbility(abilityName);
                    attackTimer = 0.0f;
                }
            }
        // Else if Healing Ability
        } else if (ability.type == 1) {

        }
    }

    bool IsAbilityCooldownComplete(string abilityName) {
        if (abilityName == stats.abilities[0]) {
            return abilityOneTimer >= abilitiesLoader.FetchAbilityByName(abilityName).cooldown;
        } else if (abilityName == stats.abilities[1]) {
            return abilityTwoTimer >= abilitiesLoader.FetchAbilityByName(abilityName).cooldown;
        }
        Debug.Log("No timer for ability " + abilityName);
        return false;
    }

    void ResetAbilityCooldownForAbility(string abilityName) {
        if (abilityName == stats.abilities[0]) {
            abilityOneTimer = 0.0f;
        } else if (abilityName == stats.abilities[1]) {
            abilityTwoTimer = 0.0f;
        }
        Debug.Log("No timer for ability " + abilityName);
    }
}
