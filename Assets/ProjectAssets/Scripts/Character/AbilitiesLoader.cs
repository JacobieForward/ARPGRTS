using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesLoader : MonoBehaviour {
    public const string path = "abilities";
    AbilityContainer abilityContainer;

    void Start() {
        abilityContainer = AbilityContainer.Load(path);
    }

    public Ability FetchAbilityByName(string abilityName) {
        foreach (Ability ability in abilityContainer.abilities) {
            if (abilityName.Equals(ability.name)) {
                return ability;
            }
        }
        Debug.Log("FetchAbilityByName called with string abilityName which was not found.");
        return null;
    }
}
