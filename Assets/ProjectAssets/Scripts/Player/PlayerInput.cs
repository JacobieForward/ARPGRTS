﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour {
    Stats stats;
    Combat combat;
    Movement movement;

    public Material outlineMaterial;
    public LayerMask whatCanBeClickedOn;

    bool isMoving;

    void Awake() {
        stats = GetComponent<Stats>();
        combat = GetComponent<Combat>();
        movement = GetComponent<Movement>();
        isMoving = false;
    }

    void Update() {
        CheckForMouseInput();
        MouseInputForMovement();
        CheckForAbilityInput();
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
                    combat.BasicAttack();
                }
                if (hitInfo.collider.gameObject.tag.Equals("Ally")) {
                    SelectTarget(hitInfo.collider.gameObject);
                }
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            isMoving = false;
        }
    }

    void MouseInputForMovement() {
        Vector3 mousehitPosition;

        if (IsRaycastToNavmeshSuccessful(out mousehitPosition)) {
            if (Input.GetMouseButtonDown(0)) {
                combat.approachingTarget = false;
                isMoving = true;
            }

            if (Input.GetMouseButton(0) && isMoving) {
                combat.approachingTarget = false;
                movement.MoveToPosition(mousehitPosition);
            }
        }
    }

    bool IsRaycastToNavmeshSuccessful(out Vector3 raycastPosition) {
        raycastPosition = new Vector3();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool isCastSuccessful = Physics.Raycast(ray, out hitInfo, 100, whatCanBeClickedOn);
        raycastPosition = hitInfo.point;
        return isCastSuccessful && !EventSystem.current.IsPointerOverGameObject();
    }

    void CheckForAbilityInput() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            combat.ActivateAbilityByAbilityNumber(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            combat.ActivateAbilityByAbilityNumber(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            combat.ActivateAbilityByAbilityNumber(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            combat.ActivateAbilityByAbilityNumber(3);
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            combat.ActivateAbilityByAbilityNumber(4);
        }
    }

    void SelectTarget(GameObject selectedObject) {
        if (combat.currentTarget != null) {
            DeSelectTarget();
        }
        selectedObject.GetComponent<Outliner>().AddOutline();
        combat.currentTarget = selectedObject;
    }
    
    void DeSelectTarget() {
        combat.currentTarget.GetComponent<Outliner>().RemoveOutline();
        combat.currentTarget = null;
    }
}
