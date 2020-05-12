using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterCombat))]
public class PlayerHUD : MonoBehaviour {
    CharacterStats playerStats;
    CharacterCombat playerCombat;

    Slider playerHealthBar;
    Slider targetHealthBar;

    
    void Awake() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterCombat>();

        playerHealthBar = GameObject.Find(Constants.PLAYER_HEALTH_BAR_GAMEOBJECT_NAME).GetComponent<Slider>();
        targetHealthBar = GameObject.Find(Constants.TARGET_HEALTH_BAR_GAMEOBJECT_NAME).GetComponent<Slider>();
    }

    void Update() {
        try {
            playerHealthBar.maxValue = playerStats.maxHealth;
            playerHealthBar.value = playerStats.currentHealth;
            if (playerCombat.currentTarget != null) {
                targetHealthBar.maxValue = playerCombat.currentTarget.GetComponent<CharacterStats>().maxHealth;
                targetHealthBar.value = playerCombat.currentTarget.GetComponent<CharacterStats>().currentHealth;
            }
        // TODO: Get rid of all this dirty try catching and just attach a script to the health bars to set their gameObject names automatically?
        // Then again how would we know the script was always attached?
        } catch (NullReferenceException e) {
            Debug.Log("One of the healthbar gameObject names has been changed, this makes the PlayerHUD script unable to use them." +
                "Or an unknown issue is causing a NullReference exception in PlayerHUD. The constants file may also have been affected.");
        }
    }
}
