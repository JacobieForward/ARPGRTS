using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterCombat))]
public class PlayerCombat : MonoBehaviour {
    CharacterCombat characterCombat;

    public Material outlineMaterial;

    void Awake() {
        characterCombat = GetComponent<CharacterCombat>();
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
                    characterCombat.Attack();
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
