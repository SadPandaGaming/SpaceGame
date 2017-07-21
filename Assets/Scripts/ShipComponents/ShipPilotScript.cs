using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class ShipPilotScript : NetworkBehaviour {

    public GameObject ship;
    float speed = 8f;

    [SyncVar][HideInInspector]
    public bool authorityEnabled = false;

    private void Start() {
        authorityEnabled = false;
        ship = gameObject.transform.parent.parent.parent.gameObject;
    }

    public void InstantiatedOnNetwork() {
        ship = gameObject.transform.parent.parent.parent.gameObject;

        //the server starts with control
        if (isServer) {
            print("try this if it doesnt work");
            //CmdRemovePilot(GameManager.clientGameObject);
        }
    }

    //trigger requires a rigidbody on one of the components && the collider component to be marked as isTrigger
    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject == GameManager.clientGameObject) {
            GameManager.clientController.useAction = UseConsole;
            //display ui for using console hotkey
        }
    }

    public void UseConsole() {
        if (!authorityEnabled) {
            GameManager.clientController.CmdGainControl(gameObject);
            GameManager.clientController.CmdGainControl(ship);
            authorityEnabled = true;
            GameManager.clientController.control = false;
        }
    }

    public void QuitConsole() {

        GameManager.clientController.CmdRemoveControl(gameObject);
        GameManager.clientController.CmdRemoveControl(ship);
        GameManager.clientController.control = true;
        authorityEnabled = false;
    }

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        print("start auth");
    }
    public override void OnStopAuthority() {
        base.OnStopAuthority();
        print("end auth");
    }

    

    

    public void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            QuitConsole();
        }
        if (!(hasAuthority && authorityEnabled)) { return; }

        if (Input.GetKeyDown(KeyCode.F)) {
            QuitConsole();
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 pos = ship.transform.position;

        ship.transform.position = pos + new Vector2(input.x, input.y) * speed * Time.deltaTime;

    }

    
}
