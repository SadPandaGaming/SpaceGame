using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallTest : NetworkBehaviour {

    NetworkIdentity ballID;
    public GameObject owner;
    
    public void Start() {
        ballID = GetComponent<NetworkIdentity>();
        
    }

    public override void OnStopAuthority() {
        print("Stopped authority");
    }

    public override void OnStartAuthority() {
        print("Started authority");
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        //PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
        //if (pm == null) { return; }
        //NetworkIdentity id = other.gameObject.GetComponent<NetworkIdentity>();
        //if (id != null && id.hasAuthority) {
        //    CmdGainControl(other.gameObject);
        //}

    }

    [Command]
    public void CmdGainControl(GameObject owner) {
        ballID.AssignClientAuthority(owner.GetComponent<NetworkIdentity>().clientAuthorityOwner);
    }

    [Command]
    public void CmdReleaseControl(GameObject owner) {
        ballID.RemoveClientAuthority(owner.GetComponent<NetworkIdentity>().clientAuthorityOwner);
        
    }

    void Update () {
		if (hasAuthority) {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            Vector3 pos = transform.position;
            transform.position = pos + input * 5 * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space)) {
                CmdReleaseControl(gameObject);
                
            }
        }
	}
}
