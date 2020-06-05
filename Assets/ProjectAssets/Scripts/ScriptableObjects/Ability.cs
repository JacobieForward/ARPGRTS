#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("CustomARPGRTSObjects/Ability"))]
public class Ability : ScriptableObject {
    [SerializeField] string name;
    [SerializeField] float cooldown;
    [SerializeField] float power; // Power applies for damage/healing/any other effects (i.e. shielding, buffs)
    [SerializeField] float range;
    [SerializeField] Sprite icon;
    [SerializeField] AnimationClip animationClip;

    public AbilityType type;

    public enum AbilityType {
        Damaging,
        Healing,
        Shielding
    }

    public string GetName() {
        return name;
    }

    public float GetCooldown() {
        return cooldown;
    }

    public float GetPower() {
        return power;
    }

    public float GetRange() {
        return range;
    }

    public Sprite GetIcon() {
        return icon;
    }

    public AnimationClip GetAnimationClip() {
        return animationClip;
    }
}
