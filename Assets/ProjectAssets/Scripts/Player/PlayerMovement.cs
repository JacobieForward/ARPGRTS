using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {
    CharacterStats characterStats;

    public LayerMask whatCanBeClickedOn;
    NavMeshAgent navMeshAgent;

    void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterStats = GetComponent<CharacterStats>();
    }
    
    void Update() {
        MouseInputForMovement();
        navMeshAgent.speed = characterStats.speed;
    }

    void MouseInputForMovement() {
        // 0 value passed to Input.GetMouseButtonDown is right mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Draw Ray from camera to mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            // Use the ray to raycast and set the destination to the mouse point
            if (Physics.Raycast(ray, out hitInfo, 100, whatCanBeClickedOn)) {
                navMeshAgent.SetDestination(hitInfo.point);
            }
        }
    }
}
