using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilities : MonoBehaviour {
    public List<Ability> abilities;

    public void ActivateAbility(string abilityName) {
        Debug.Log("Ability Activated");
    }
}
