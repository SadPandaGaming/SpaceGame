using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerScript : NetworkBehaviour {
    //
    Text countdown;

    void Start() {

        if (isServer) {
            StartCoroutine(SpawnShip());
        }
    }

    IEnumerator SpawnShip() {

        yield return new WaitForSecondsRealtime(1);
        RpcTimer("Starting in 3...");
        yield return new WaitForSecondsRealtime(1);
        RpcTimer("Starting in 2...");
        yield return new WaitForSecondsRealtime(1);
        RpcTimer("Starting in 1...");
        yield return new WaitForSecondsRealtime(1);
        RpcTimer("");
        //spawn ship
        //create background from resource folder
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Ship") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        NetworkServer.Spawn(go);

        //place users inside the ship
        foreach (var obj in NetworkServer.objects) {
            
        }
           
    }

    [ClientRpc]
    public void RpcTimer(string text) {
        
        if (countdown == null) { countdown = GameObject.Find("Countdown").transform.GetComponent<Text>(); countdown.enabled = true; }
        if (text == "") { countdown.enabled = false; }
        countdown.text = text;
    }

}
        
    


