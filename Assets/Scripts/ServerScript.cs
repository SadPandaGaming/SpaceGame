using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerScript : NetworkBehaviour {
    
    Text countdown;

    void Start() {

        if (isServer) {
            StartCoroutine(SpawnShip());
        }
    }

    IEnumerator SpawnShip() {

        //yield return new WaitForSecondsRealtime(1);
        //RpcTimer("Starting in 3...");
        //yield return new WaitForSecondsRealtime(1);
        //RpcTimer("Starting in 2...");
        //yield return new WaitForSecondsRealtime(1);
        //RpcTimer("Starting in 1...");
        yield return new WaitForSecondsRealtime(1);
        RpcTimer("");


        //spawn ship
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/NetworkPrefabs/Ship") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);

        GameObject driver = (GameObject)Instantiate(Resources.Load("Prefabs/NetworkPrefabs/ShipConsoleDriver") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);

        

        NetworkServer.Spawn(driver);

        NetworkServer.Spawn(go);

        //place users inside the ship
        RpcMoveIntoShip(go, driver); //in the case where a server is not also a client, will i have to run the same code for the server too?
    }

    [ClientRpc]
    public void RpcMoveIntoShip(GameObject ship, GameObject driver) {

        driver.transform.SetParent(ship.transform.Find("Inside").Find("Spot_Driver"));
        driver.transform.localPosition = Vector2.zero;
        

        Transform insideShip = ship.transform.Find("Inside").transform;
        foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
            player.transform.SetParent(insideShip);
        }

        driver.GetComponent<ShipPilotScript>().InstantiatedOnNetwork();


        //quick ntc test...
        //GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        //NetworkTransformChild ntc = go.AddComponent<NetworkTransformChild>();
        //ntc.target = gos[0].transform;
        //ntc = go.AddComponent<NetworkTransformChild>();
        //ntc.target = gos[1].transform;
        //

    }

    [ClientRpc]
    public void RpcTimer(string text) {
        
        if (countdown == null) { countdown = GameObject.Find("Countdown").transform.GetComponent<Text>(); countdown.enabled = true; }
        if (text == "") { countdown.enabled = false; }
        countdown.text = text;
    }

}
        
    


