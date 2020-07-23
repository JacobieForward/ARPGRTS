using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour {
    // TODO: Eventually have different animation sets for certain characters/weapons
    Animator animator;
    AnimatorOverrideController animatorOverrideController;

    void Awake() {
        animator = GetComponent<Animator>();


        animatorOverrideController = new AnimatorOverrideController();
        animatorOverrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
    }

    void AbilityActivated(Ability ability) {
        ChangeAbilityAnimation(ability.GetAnimationClip());
        animator.SetTrigger("usingAbility");
    }

    public Animator GetAnimator() {
        return animator;
    }

    void ChangeAbilityAnimation(AnimationClip abilityAnimationClip) {
        animatorOverrideController["ShieldWarrior@BlockHit01"] = abilityAnimationClip;
        animator.runtimeAnimatorController = animatorOverrideController;
    }
}
