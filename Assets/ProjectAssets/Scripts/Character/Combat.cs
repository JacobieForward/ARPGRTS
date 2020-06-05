using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(AnimationController))]
// TODO: Use inheritance for all the various scripts that use this
public class Combat : MonoBehaviour {
    Stats stats;
    Movement movement;
    AnimationController animationController;

    float attackTimer;

    public GameObject currentTarget;

    public bool approachingTarget;

    [SerializeField] List<Ability> abilityList;
    List<float> abilityCooldownTimers = new List<float>();

    void Awake() {
        stats = GetComponent<Stats>();
        movement = GetComponent<Movement>();
        animationController = GetComponent<AnimationController>();

        attackTimer = stats.attackSpeed;
        approachingTarget = false;

        InitializeAbilityLists();
    }

    void InitializeAbilityLists() {
        if (abilityList.Count == 0) {
            return;
        }
        float newCooldown = 0.0f;
        abilityCooldownTimers.Add(newCooldown);
    }

    void Update() {
        CheckForDeath();
        UpdateTimers();
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

    void UpdateTimers() {
        for (int i = 0; i < abilityCooldownTimers.Count; i++) {
            abilityCooldownTimers[i] += Time.deltaTime;
        }
        attackTimer += Time.deltaTime;
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
                attackTimer = 0.0f;
            }
        } else {
            approachingTarget = true;
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

    public void ActivateAbility(Ability ability) {
        print(ability.GetName());

        if (currentTarget == null) {
            Debug.Log("ActivateAbility called with no target selected.");
            return;
        }
        // If Damaging Ability
        if (ability.type == Ability.AbilityType.Damaging) {
            // Check attack range
            if (WithinAttackRangeOfTarget(ability.GetRange()) && FacingTarget()) {
                if (IsAbilityCooldownComplete(ability) && IsAttackCooldownComplete()) {
                    animationController.ChangeAbilityAnimation(ability);
                    animationController.GetAnimator().SetTrigger("usingAbility");
                    DealDamageToTarget(ability.GetPower());
                    attackTimer = 0.0f;
                    ResetAbilityCooldown(ability);
                }
            }
        // Else if Healing Ability
        } else if (ability.type == Ability.AbilityType.Healing) {

        }
    }

    bool IsAbilityCooldownComplete(Ability ability) {
        int abilityIndex = GetAbilityIndex(ability);

        print(abilityCooldownTimers[abilityIndex]);
        if (abilityCooldownTimers[abilityIndex] >= ability.GetCooldown()) {
            return true;
        }
        return false;
    }

    void ResetAbilityCooldown(Ability ability) {
        int abilityIndex = GetAbilityIndex(ability);

        abilityCooldownTimers[abilityIndex] = 0.0f;
    }

    int GetAbilityIndex(Ability ability) {
        int abilityIndex = 9999;
        for (int i = 0; i < abilityList.Count; i++) {
            if (abilityList[i].Equals(ability)) {
                abilityIndex = i;
            }
        }

        if (abilityIndex == 9999) {
            throw new System.ArgumentException("GetAbilityIndex called with ability not in abilityList.", "ability");
            return -1;
        }
        return abilityIndex;
    }

    bool IsAttackCooldownComplete() {
        return attackTimer >= stats.attackSpeed;
    }
}
