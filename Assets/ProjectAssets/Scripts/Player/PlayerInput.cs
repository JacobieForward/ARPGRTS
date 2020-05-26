using System.Collections;
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

    void Awake() {
        stats = GetComponent<Stats>();
        combat = GetComponent<Combat>();
        movement = GetComponent<Movement>();
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
                    combat.BasicAttack();
                }
                if (hitInfo.collider.gameObject.tag.Equals("Ally")) {
                    SelectTarget(hitInfo.collider.gameObject);
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
                    combat.approachingTarget = false;
                    movement.MoveToPosition(hitInfo.point);
                }
            }
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
