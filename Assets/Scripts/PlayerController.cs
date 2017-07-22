using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    float speed = 3.5f;
    public LayerMask wallsLayer;

    public Action useAction = null;
    public bool control = true;

    //random test
    bool random = false;
    int[] randMove = { 1, -1 };
    int randomX = 0;
    int randomY = 0;
    float time = 0.2f;


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

        if (!control) { return; } //if control is given to another script

        if (useAction != null && Input.GetKeyDown(KeyCode.F)) {
            useAction();
            print("f was pressed");
        }

        //random activate
        if (Input.GetKeyDown(KeyCode.T)) {
            random = !random;
        }


        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //random test
        if (random) {
            time += Time.deltaTime;
            if (time > 3) {
                time = 0;
                randomX = UnityEngine.Random.Range(0, 2);
                randomY = UnityEngine.Random.Range(0, 2);
            }
            input.x = randMove[randomX];
            input.y = randMove[randomY];
        }
        Vector2 pos = transform.position;
        
        if (CollisionX(input.x)) { input.x = 0; }
        if (CollisionY(input.y)) { input.y = 0; }
        if (input.x != 0 && input.y != 0) { input = input.normalized; }

        transform.position = pos + input * speed * Time.deltaTime;

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
                   

        
    
