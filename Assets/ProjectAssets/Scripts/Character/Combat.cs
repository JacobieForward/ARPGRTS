using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(AbilitySystem))]
// TODO: Use inheritance for all the various scripts that use this
public class Combat : MonoBehaviour {
    Stats stats;
    Movement movement;
    AnimationController animationController;
    AbilitySystem abilitySystem;

    float attackTimer;

    public GameObject currentTarget;

    public bool approachingTarget;

    void Awake() {
        stats = GetComponent<Stats>();
        movement = GetComponent<Movement>();
        animationController = GetComponent<AnimationController>();
        abilitySystem = GetComponent<AbilitySystem>();

        attackTimer = stats.attackSpeed;
        approachingTarget = false;
    }

    void Update() {
        CheckForDeath();
        attackTimer += Time.deltaTime;
        if (approachingTarget) {
            // TODO: Combine moving and turning into a single method?
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
            if (IsAttackCooldownComplete()) {
                animationController.GetAnimator().SetTrigger("attacking");
                DealDamageToTarget(1.0f);
                ResetAttackCooldown();
            }
        } else {
            approachingTarget = true;
        }
    }
    
    void AbilityActivated(Ability ability) {
        // If Damaging Ability
        if (ability.type == Ability.AbilityType.Damaging) {
            // Check attack range
            if (WithinAttackRangeOfTarget(ability.GetRange()) && FacingTarget()) {
                DealDamageToTarget(ability.GetPower());
                ResetAttackCooldown();
            }
            // Else if Healing Ability
        } else if (ability.type == Ability.AbilityType.Healing) {

        }
    }

    bool WithinAttackRangeOfTarget(float attackRange) {
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distance <= attackRange) {
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

    public bool IsAttackCooldownComplete() {
        return attackTimer >= stats.attackSpeed;
    }

    public void ResetAttackCooldown() {
        attackTimer = 0.0f;
    }
}
