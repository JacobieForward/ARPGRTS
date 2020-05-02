using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    CharacterStats playerStats;

    public Slider healthBar;

    void Awake() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        healthBar.maxValue = playerStats.health;
    }

    void Update() {
        healthBar.value = playerStats.health;
    }
}
