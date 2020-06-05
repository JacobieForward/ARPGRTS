using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animation))]
public class AnimationController : MonoBehaviour {
    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>> {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name] {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }
    Animation animation;
    Animator animator;
    AnimatorOverrideController animatorOverrideController;

    void Awake() {
        animation = GetComponent<Animation>();
        animator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
    }

    public Animator GetAnimator() {
        return animator;
    }

    public Animation GetAnimation() {
        return animation;
    }


    public void PlayAbilityAnimationClip(Ability ability) {
        animation.AddClip(ability.GetAnimationClip(), "Ability");
        animation.Play("Ability");
        //animation.RemoveClip(ability.GetAnimationClip());
    }

    // TODO: Maybe get this working later
    public void ChangeAbilityAnimation(Ability ability) {
        /*AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        List<KeyValuePair<AnimationClip, AnimationClip>> newAnimationClips = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        //newAnimationClips["usingAbility"] = ability.GetAnimationClip();
        for (int i = 0; i < animatorOverrideController.animationClips; i++)
        foreach(AnimationClip anim in animatorOverrideController.animationClips) {
            newAnimationClips.Add(new KeyValuePair<AnimationClip, AnimationClip>(anim, ));
        }
        animatorOverrideController.ApplyOverrides(newAnimationClips);*/


        /*AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        AnimationClipOverrides animationClipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(animationClipOverrides);

        animationClipOverrides["Ability"] = ability.GetAnimationClip();

        animatorOverrideController.ApplyOverrides(animationClipOverrides);*/
        animatorOverrideController["Ability"] = ability.GetAnimationClip();
    }
}
