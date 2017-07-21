using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

    Vector3 input = Vector2.zero;
    float speed = 2.5f;

    public Action<PlayerMovement> onUseAction = null;
    public bool playerActive = false;

    public NetworkIdentity identity;

    public GameObject viewPosition;
    public GameObject currentShip = null;
    public LayerMask wall;

    public ViewType viewType = ViewType.player;

    public bool Immobile { get; set; }
	
	 IEnumerator Start() {

        yield return 0; //0 will wait 1 frame.
        //viewPosition = currentShip; //we start viewing and moving as a regular player
        
    }
	
    public void UseAction() {
        //USE EVENT
        if (onUseAction != null) {
            onUseAction(this);
        }
    }


    [Command]
    public void CmdControl(bool val, NetworkIdentity identity, GameObject theShip) {

        if (val) {
            //give control
            theShip.GetComponent<SteeringControl>().shipNetwork.AssignClientAuthority(identity.clientAuthorityOwner);
            //currentShip.shipNetwork.AssignClientAuthority(identity.clientAuthorityOwner);
        }
        else {
            //take control
            theShip.GetComponent<SteeringControl>().shipNetwork.RemoveClientAuthority(identity.clientAuthorityOwner);
            //shipNetwork.RemoveClientAuthority(identity.clientAuthorityOwner);
        }
    }

    public void Movement(Vector3 input) {
        if (!Immobile) {
            Vector2 direction = input;
            direction = direction.Rotate(transform.rotation.eulerAngles.z);
            Vector3 pos = transform.position;

            if (CollisionX(direction)) {
                direction.x = 0;
            }
            if (CollisionY(direction)) {
                direction.y = 0;
            }
            transform.position = pos + (Vector3)direction * speed * Time.deltaTime;
        }
        transform.rotation = currentShip.transform.rotation;
    }

    bool CollisionY(Vector3 direction) {
        bool upDown = direction.y != 0 ? Physics2D.Raycast(transform.position, new Vector2(0, direction.y), 0.45f + speed * Time.deltaTime, wall) : false;
        Debug.DrawRay(transform.position, new Vector2(0, direction.y) * 0.45f, Color.red);
        

        return upDown;
    }

    bool CollisionX(Vector3 direction) {
        
        bool leftRight = direction.x != 0 ? Physics2D.Raycast(transform.position, new Vector2(direction.x, 0), 0.45f + speed * Time.deltaTime, wall) : false;
        Debug.DrawRay(transform.position, new Vector2(direction.x, 0) * 0.45f, Color.red);

        return leftRight;
    }

    public void ShipView(GameObject componentView = null) {
        if (componentView != null) { viewPosition = componentView; }
        if (viewPosition == gameObject) {
            //we view the whole ship
            viewPosition = currentShip;
        }
        // if the current view position was not our character... that means we have another view point (probably some ship console type)
        // print (componentView.name);
        CameraManager.access.ShipView();
        viewType = ViewType.ship;
    }

    public void ExitShipView() {
        viewPosition = currentShip;

        CameraManager.access.PlayerView();
        viewType = ViewType.player;
    }
}
