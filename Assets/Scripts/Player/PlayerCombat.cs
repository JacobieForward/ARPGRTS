using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterActions))]
public class PlayerCombat : MonoBehaviour {
    CharacterActions characterActions;

    void Awake() {
        characterActions = GetComponent<CharacterActions>();
    }

    void Update() {
        CheckForMouseInput();
    }

    void CheckForMouseInput() {
        if (Input.GetMouseButtonDown(1)) {
            // Draw Ray from camera to mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            // Use the ray to raycast and set the destination to the mouse point
            if (Physics.Raycast(ray, out hitInfo, 100)) {
                if (hitInfo.collider.gameObject.tag.Equals("Enemy")) {
                    characterActions.Attack(hitInfo.collider.gameObject);
                }
            }
        }
    }
}
