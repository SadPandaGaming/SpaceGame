using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SteeringControl : NetworkBehaviour {

    GameObject cameraView = null;

    PlayerMovement pilot;
    public GameObject ship;
    bool drivingActive = false;
    float speed = 2f;
    float rotateSpeed = 10f;
    Vector3 input = Vector2.zero;
    Rigidbody2D body;

    public NetworkIdentity shipNetwork;

    public float rotationSpeed = 10f;
    private Vector3 rotationDirection = new Vector3(0, 0, 1);


    IEnumerator Start() {
        yield return 0;
        cameraView = transform.Find("CameraView").gameObject;
        body = transform.parent.transform.parent.transform.GetComponent<Rigidbody2D>();

        shipNetwork = body.transform.GetComponent<NetworkIdentity>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
        if (pm != null) {
            pm.onUseAction = UseDrivingConsole;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
        if (pm != null) {
            if (pm.onUseAction == UseDrivingConsole) {
                pm.onUseAction = null;
            }
        }
    }

    

    public void UseDrivingConsole(PlayerMovement user) {
        user.Immobile = true;
        user.onUseAction = ExitDrivingConsole;
        //enter driving control mode

        user.ShipView(cameraView);
        drivingActive = true;
        pilot = user;

        user.CmdControl(true, user.identity, user.currentShip);
    }

    public void ExitDrivingConsole(PlayerMovement user) {
        user.Immobile = false;
        user.onUseAction = UseDrivingConsole;
        //leave driving control mode
       
        user.ExitShipView();
        drivingActive = false;
        pilot = null;
        user.CmdControl(false, user.identity, user.currentShip);
    }

    private void Update() {
        if (drivingActive && GameManager.access.currentPlayer == pilot && shipNetwork.clientAuthorityOwner == pilot.identity.clientAuthorityOwner) {

            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

            Vector3 target = cameraView.transform.position;
            Vector2 dir = target - body.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (input.y != 0) {
                body.AddForce(dir * speed * 15000f * Time.deltaTime);
            }
            if (input.x > 0) {
                ship.transform.Rotate(-rotationDirection * (rotationSpeed * Time.deltaTime));
            }
            if (input.x < 0) {
                ship.transform.Rotate(rotationDirection * (rotationSpeed * Time.deltaTime));
            }

        }

        
    }
}

