using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    CharacterStats playerStats;
    CharacterActions playerActions;

    Slider playerHealthBar;
    Slider targetHealthBar;


    void Awake() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterActions>();

        playerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<Slider>();
        targetHealthBar = GameObject.Find("TargetHealthBar").GetComponent<Slider>();
    }

    void Update() {
        playerHealthBar.maxValue = playerStats.maxHealth;
        playerHealthBar.value = playerStats.currentHealth;
        if (playerActions.currentTarget != null) {
            targetHealthBar.maxValue = playerActions.currentTarget.GetComponent<CharacterStats>().maxHealth;
            targetHealthBar.value = playerActions.currentTarget.GetComponent<CharacterStats>().currentHealth;
        }
    }
}
