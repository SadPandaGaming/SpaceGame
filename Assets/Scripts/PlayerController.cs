using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public static GameObject emptyPlayerGameObject;
    float speed = 5f;
    public GameObject ballPrefab;
    public LayerMask wallsLayer;

    public Action useAction = null;
    public bool control = true;


    IEnumerator Start() {
        if (!hasAuthority) { yield break; }
        yield return 5; //wait 5 frames for other objects to finish starting up
        GetComponent<SpriteRenderer>().color = Color.green;
        GameManager.clientGameObject = gameObject;
        GameManager.clientController = this;
    }

    //[Command]
    //public void CmdSpawnBall(GameObject ownerObject) {
    //    GameObject go = (GameObject)Instantiate(ballPrefab);
    //    go.transform.position = transform.position + Vector3.up * 1.3f;
    //    NetworkServer.SpawnWithClientAuthority(go, ownerObject);
    //}

    [Command]
    public void CmdRemoveControl(GameObject targetObject) {
        targetObject.GetComponent<NetworkIdentity>().RemoveClientAuthority(gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner);
    }

    [Command]
    public void CmdGainControl(GameObject targetObject) {
        targetObject.GetComponent<NetworkIdentity>().AssignClientAuthority(gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner);
    }

    void Update() {

        if (!hasAuthority) { return; }

        if (!control) { return; } //if ocntrol is given to another script

        if (useAction != null && Input.GetKeyDown(KeyCode.F)) {
            useAction();
            print("f was pressed");
        }


        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 pos = transform.position;

        if (CollisionX(input.x)) { input.x = 0; }
        if (CollisionY(input.y)) { input.y = 0;}

        transform.position = pos + new Vector2(input.x, input.y) * speed * Time.deltaTime;

    }

    bool CollisionX(float x) {
        Debug.DrawRay(transform.position + new Vector3(x, 0, 0) * 0.5f, new Vector2(x, 0) * speed * Time.deltaTime, Color.red);
        return Physics2D.Raycast(transform.position, new Vector2(x, 0), 0.5f + x * speed * Time.deltaTime, wallsLayer);
    }

    bool CollisionY(float y) {
        Debug.DrawRay(transform.position + new Vector3(0, y, 0) * 0.5f, new Vector2(0, y) * speed * Time.deltaTime, Color.red);
        return Physics2D.Raycast(transform.position, new Vector2(0, y), 0.5f + y * speed * Time.deltaTime, wallsLayer);
    }
}
                   

        
    
