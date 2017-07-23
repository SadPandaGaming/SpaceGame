using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public static CameraFollow access;

    public bool copyRotation = false;
    Camera cam;
    Transform target;

    private void Start() {
        access = this;
        cam = GetComponent<Camera>();
    }

    public void AssignShip(GameObject go) {
        target = go.transform;
    }

    private void Update() {
        if (target != null) {
            Vector3 pos = target.transform.position;
            pos.z = -10;
            cam.transform.position = pos;
            if (copyRotation) {
                cam.transform.rotation = target.transform.rotation;
            } else {
                cam.transform.rotation = Quaternion.identity;
            }
        }
    }
}
