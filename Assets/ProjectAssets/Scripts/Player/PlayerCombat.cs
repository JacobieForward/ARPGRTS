using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterActions))]
public class PlayerCombat : MonoBehaviour {
    CharacterActions characterActions;

    public GameObject currentTarget;

    public Material outlineMaterial;

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
                    SelectTarget(hitInfo.collider.gameObject);
                    characterActions.Attack();
                }
            }
        }
    }

    void SelectTarget(GameObject selectedObject) {
        selectedObject.GetComponent<Outliner>().AddOutline();
        characterActions.currentTarget = selectedObject;
    }

    void DeSelectTarget() {
        characterActions.currentTarget.GetComponent<Outliner>().RemoveOutline();
        characterActions.currentTarget = null;
    }
}
