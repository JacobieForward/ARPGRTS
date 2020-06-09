using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
#pragma warning disable CS0649
public class AbilityButton : MonoBehaviour {
    Button button;
    Combat playerCombat;

    [SerializeField] Ability ability;

    void Awake() {
        button = GetComponent<Button>();
        
        button.onClick.AddListener(CallButtonAbility);

        playerCombat = GameObject.Find("Player").GetComponent<Combat>();
        AddAbility();
    }

    void CallButtonAbility() {
        playerCombat.ActivateAbility(ability);
    }

    void AddAbility() {
        if (ability != null) {
            Image image = GetComponent<Image>();
            image.enabled = true;
            image.sprite = ability.GetIcon();
        }
    }
}
