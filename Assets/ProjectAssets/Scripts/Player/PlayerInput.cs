using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterCombat))]
[RequireComponent(typeof(CharacterMovement))]
public class PlayerInput : MonoBehaviour {
    CharacterStats characterStats;
    CharacterCombat characterCombat;
    CharacterMovement characterMovement;

    public Material outlineMaterial;
    public LayerMask whatCanBeClickedOn;

    void Awake() {
        characterStats = GetComponent<CharacterStats>();
        characterCombat = GetComponent<CharacterCombat>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    void Update() {
        CheckForMouseInput();
        MouseInputForMovement();
    }

    void CheckForMouseInput() {
        if (Input.GetMouseButtonDown(1)) {
            // Draw Ray from camera to mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            // Use the ray to raycast and set the destination to the mouse point
            if (Physics.Raycast(ray, out hitInfo, 100)) {
                if (hitInfo.collider.gameObject.tag.Equals("Enemy")) {
                    SelectTarget(hitInfo.collider.gameObject);
                    characterCombat.Attack();
                }
            }
        }
    }

    void MouseInputForMovement() {
        // 0 value passed to Input.GetMouseButtonDown is right mouse button
        if (Input.GetMouseButtonDown(0)) {
            // Draw Ray from camera to mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            // Use the ray to raycast and set the destination to the mouse point
            if (Physics.Raycast(ray, out hitInfo, 100, whatCanBeClickedOn)) {
                // Check if mouse is over a UI object, if so ignore input commands
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    characterCombat.approachingTarget = false;
                    characterMovement.MoveToPosition(hitInfo.point);
                }
            }
        }
    }

    void SelectTarget(GameObject selectedObject) {
        selectedObject.GetComponent<Outliner>().AddOutline();
        characterCombat.currentTarget = selectedObject;
    }

    void DeSelectTarget() {
        characterCombat.currentTarget.GetComponent<Outliner>().RemoveOutline();
        characterCombat.currentTarget = null;
    }
}
