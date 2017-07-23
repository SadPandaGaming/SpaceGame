using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallScript : NetworkBehaviour {

    float speed = 5f;
    [SyncVar]
    public bool authorityEnabled = true;

    private void Start() {
        CmdCommandGain();
    }

    public override void OnStartAuthority() {
        print("gained authority");
    }

    public override void OnStopAuthority() {
        print("lost authority");
    }

    [Command]
    public void CmdCommandGain() {
        authorityEnabled = true;
    }

    [Command]
    public void CmdRelease(GameObject ownerObject) {
        bool success = gameObject.GetComponent<NetworkIdentity>().RemoveClientAuthority(ownerObject.GetComponent<NetworkIdentity>().clientAuthorityOwner);
        print("release: " +success);
        authorityEnabled = false;
    }

   

    void Update () {
        if (!authorityEnabled) { return; }
        if (!hasAuthority) { return; }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 pos = transform.position;
        transform.position = pos + input * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)) {
            CmdRelease(gameObject);
        }
    }
}
