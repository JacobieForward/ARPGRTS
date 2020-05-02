using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    [SerializeField]
    float scrollInMax = 2.0f;

    [SerializeField]
    float scrollOutMax = 10.0f;

    void Awake() {
        offset = new Vector3(0, 5, -5);
    }

    void Update() {
        CameraFollowsPlayer();
        MouseInput();
        KeyboardInput();
    }

    void CameraFollowsPlayer() {
        transform.position = target.position + offset;
    }

    void MouseInput() {
        if (Input.mouseScrollDelta.y < 0) {
            ScrollCameraOut();
        }

        if (Input.mouseScrollDelta.y > 0) {
            ScrollCameraIn();
        }
    }

    void ScrollCameraIn() {
        if (offset.y > scrollInMax) {
            offset.y -= 1;
            offset.z += 1;
        }
    }

    void ScrollCameraOut() {
        if (offset.y < scrollOutMax) {
            offset.y += 1;
            offset.z -= 1;
        }
    }

    void KeyboardInput() {
        if (Input.GetKeyDown("q")) {
            RotateCameraLeft();
        }
    }

    void RotateCameraLeft() {
        //gameObject.transform.Rotate();
    }
}
