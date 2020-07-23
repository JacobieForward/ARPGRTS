using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
#pragma warning disable CS0649
public class AbilityButton : MonoBehaviour {
    Button button;
    AbilitySystem playerAbilitySystem;

    [SerializeField] Ability ability;

    void Awake() {
        button = GetComponent<Button>();
        
        button.onClick.AddListener(CallButtonAbility);

        playerAbilitySystem = GameObject.Find("Player").GetComponent<AbilitySystem>();
        AddAbility();
    }

    void CallButtonAbility() {
        playerAbilitySystem.ActivateAbility(ability);
    }

    void AddAbility() {
        if (ability != null) {
            Image image = GetComponent<Image>();
            image.enabled = true;
            image.sprite = ability.GetIcon();
        }
    }
}
