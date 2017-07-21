using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public static GameObject emptyPlayerGameObject;
    float speed = 5f;
    public GameObject ballPrefab;

    void Start() {
        if (!hasAuthority) { return; }
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    [Command]
    public void CmdSpawnBall(GameObject ownerObject) {
        GameObject go = (GameObject)Instantiate(ballPrefab);
        go.transform.position = transform.position + Vector3.up * 1.3f;
        NetworkServer.SpawnWithClientAuthority(go, ownerObject);
    }

    void Update() {
        if (!hasAuthority) { return; }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 pos = transform.position;
        transform.position = pos + input * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.L)) {
            CmdSpawnBall(gameObject);
        }
    }
}
                   

        
    
