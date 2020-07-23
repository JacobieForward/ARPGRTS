using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(AbilitySystem))]
public class PlayerInput : MonoBehaviour {
    Stats stats;
    Combat combat;
    Movement movement;
    AbilitySystem abilitySystem;

    public Material outlineMaterial;
    public LayerMask whatCanBeClickedOn;

    bool isMoving;

    void Awake() {
        stats = GetComponent<Stats>();
        combat = GetComponent<Combat>();
        movement = GetComponent<Movement>();
        abilitySystem = GetComponent<AbilitySystem>();
        isMoving = false;
    }

    void Update() {
        // TODO: Both mouse input methods and the raycast methods associated with each of them seriously need refactoring
        if (!CheckMouseInputForCombat()) {
            MouseInputForMovement();
        }
        CheckForAbilityInput();
    }

    bool CheckMouseInputForCombat() {
        RaycastHit hit;
        if (IsRaycastToGameobjectSuccessful(out hit)) {
            if (Input.GetMouseButtonDown(0)) {
                if (hit.collider.gameObject.tag.Equals("Enemy")) {
                    SelectTarget(hit.collider.gameObject);
                    return true;
                }
                if (hit.collider.gameObject.tag.Equals("Ally")) {
                    SelectTarget(hit.collider.gameObject);
                    return true;
                }
            }

            if (Input.GetMouseButtonDown(1)) {
                if (hit.collider.gameObject.tag.Equals("Enemy")) {
                    SelectTarget(hit.collider.gameObject);
                    combat.BasicAttack();
                    return true;
                }
                if (hit.collider.gameObject.tag.Equals("Ally")) {
                    SelectTarget(hit.collider.gameObject);
                    return true;
                }
            }
        }
        return false;
    }

    void MouseInputForMovement() {
        Vector3 mousehitPosition;
        RaycastHit hitInfo;
        // TODO: Make the Raycast only happen if the mouse button is used
        if (IsRaycastToGroundSuccessful(out mousehitPosition)) {
            if (Input.GetMouseButtonDown(0)) {
                combat.approachingTarget = false;
                isMoving = true;
            }

            if (Input.GetMouseButton(0) && isMoving) {
                combat.approachingTarget = false;
                movement.MoveToPosition(mousehitPosition);
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            isMoving = false;
        }
    }

    bool IsRaycastToGroundSuccessful(out Vector3 raycastPosition) {
        raycastPosition = new Vector3();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool isCastSuccessful = Physics.Raycast(ray, out hitInfo, 100, whatCanBeClickedOn);
        raycastPosition = hitInfo.point;
        return isCastSuccessful && !EventSystem.current.IsPointerOverGameObject(); // Note EventSystem.current.IsPointerOverGameObject() only applies for UI gameObjects
    }

    bool IsRaycastToGameobjectSuccessful(out RaycastHit hitInfo) {
        //hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isCastSuccessful = Physics.Raycast(ray, out hitInfo, 100);
        return isCastSuccessful && !EventSystem.current.IsPointerOverGameObject(); // Note EventSystem.current.IsPointerOverGameObject() only applies for UI gameObjects
    }

    void CheckForAbilityInput() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            abilitySystem.ActivateAbilityByAbilityNumber(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            abilitySystem.ActivateAbilityByAbilityNumber(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            abilitySystem.ActivateAbilityByAbilityNumber(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            abilitySystem.ActivateAbilityByAbilityNumber(3);
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            abilitySystem.ActivateAbilityByAbilityNumber(4);
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
