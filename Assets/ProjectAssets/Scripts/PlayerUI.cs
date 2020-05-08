using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    CharacterStats playerStats;
    CharacterCombat playerCombat;

    Slider playerHealthBar;
    Slider targetHealthBar;


    void Awake() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterCombat>();

        playerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<Slider>();
        targetHealthBar = GameObject.Find("TargetHealthBar").GetComponent<Slider>();
    }

    void Update() {
        playerHealthBar.maxValue = playerStats.maxHealth;
        playerHealthBar.value = playerStats.currentHealth;
        if (playerCombat.currentTarget != null) {
            targetHealthBar.maxValue = playerCombat.currentTarget.GetComponent<CharacterStats>().maxHealth;
            targetHealthBar.value = playerCombat.currentTarget.GetComponent<CharacterStats>().currentHealth;
        }
    }
}
