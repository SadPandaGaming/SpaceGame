using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class ShipPilotScript : NetworkBehaviour {

    public GameObject ship;
    public Rigidbody2D shipBody;
    float speed = 8f;
    float rotationSpeed = 60f;

    Vector2 input = Vector2.zero;

    [SyncVar][HideInInspector]
    public bool authorityEnabled = false;

    private void Start() {
        authorityEnabled = false;
        ship = gameObject.transform.parent.parent.parent.gameObject;
    }

    public void InstantiatedOnNetwork() {
        ship = gameObject.transform.parent.parent.parent.gameObject;

        shipBody = ship.GetComponent<Rigidbody2D>();
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

    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.gameObject == GameManager.clientGameObject) {
            if (GameManager.clientController.useAction == UseConsole) {
                GameManager.clientController.useAction = null;
            }
        }
    }

    public void UseConsole() {
        if (!authorityEnabled) {

            authorityEnabled = true;
            GameManager.clientController.control = false;
            GameManager.clientController.CmdGainControl(gameObject);
            GameManager.clientController.CmdGainControl(ship);
            //temp
            GameManager.clientController.weaponScript.UseConsole();
        }
    }


    public void QuitConsole() {
        
        authorityEnabled = false;

        GameManager.clientController.CmdRemoveControl(gameObject);
        GameManager.clientController.CmdRemoveControl(ship);
        GameManager.clientController.control = true;
        //temp
        GameManager.clientController.weaponScript.QuitConsole();
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

        if (!(hasAuthority && authorityEnabled)) { return; }


        print("test");
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 pos = ship.transform.position;
        ship.transform.position = pos + (Vector2)ship.transform.up * input.y * speed * Time.deltaTime;



        ship.transform.Rotate(0, 0, -input.x * rotationSpeed * Time.deltaTime);



        if (Input.GetKeyDown(KeyCode.F)) {
            QuitConsole();
            input = Vector2.zero;
        }

    }

    
}
