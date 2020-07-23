using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#pragma warning disable CS0649
public class AbilitySystem : MonoBehaviour {
    [SerializeField] List<Ability> abilityList;
    List<float> abilityCooldownTimers = new List<float>();

    Combat combat;
    AnimationController animationController;

    void Awake() {
        InitializeAbilityLists();
        combat = GetComponent<Combat>();
        animationController = GetComponent<AnimationController>();
    }

    void Update() {
        UpdateAbilityTimers();
    }

    void InitializeAbilityLists() {
        if (abilityList.Count == 0) {
            return;
        }

        for (int i = 0; i < abilityList.Count; i++) {
            float newCooldown = 0.0f;
            abilityCooldownTimers.Add(newCooldown);
        }
    }

    void UpdateAbilityTimers() {
        for (int i = 0; i < abilityCooldownTimers.Count; i++) {
            abilityCooldownTimers[i] += Time.deltaTime;
        }
    }

    bool IsAbilityCooldownComplete(Ability ability) {
        int abilityIndex = GetAbilityIndex(ability);

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
        }
        return abilityIndex;
    }

    public void ActivateAbility(Ability ability) {
        if (combat.currentTarget == null) {
            Debug.Log("ActivateAbility called with no target selected.");
            return;
        }

        if (IsAbilityCooldownComplete(ability) && combat.IsAttackCooldownComplete()) {
            BroadcastMessage("AbilityActivated", ability);
            ResetAbilityCooldown(ability);
        }
    }

    public void ActivateAbilityByAbilityNumber(int abilityNumber) {
        try {
            ActivateAbility(abilityList[abilityNumber]);
        } catch (ArgumentOutOfRangeException e) {
            Debug.Log("Used Ability with slot not in Ability List of: " + gameObject.name);
            Debug.Log(e.ToString());
            return;
        }
    }
}
