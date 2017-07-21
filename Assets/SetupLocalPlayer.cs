using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

    [SyncVar] public string playerName = "Player";

    [Command]
    public void CmdChangeName(string _newName) {
        playerName = _newName;
    }

    public GameObject ball;

    IEnumerator Start() {
        yield return 5;
        if (hasAuthority) {

            PlayerMovement pm = GetComponent<PlayerMovement>();
            pm.enabled = true;
            GameManager.access.SelectPlayer(pm);
           // GameManager.access.client = GetComponent<NetworkIdentity>();
           // pm.identity = GetComponent<NetworkIdentity>();
            pm.currentShip = GameManager.access.ship;
            Debug.Log("test");
        }
    }

    [Command]
    public void CmdSpawnBall(GameObject owner) {
        GameObject go = (GameObject)Instantiate(ball);
        go.GetComponent<BallTest>().owner = gameObject;
        go.transform.position = transform.position + Vector3.up * 1.3f;
        NetworkServer.SpawnWithClientAuthority(go, owner.GetComponent<NetworkIdentity>().clientAuthorityOwner);
    }

    private void Update() {
        if (hasAuthority) {
            if (Input.GetKeyDown(KeyCode.L)) {
                CmdSpawnBall(gameObject);
            }
        }
    }

}
