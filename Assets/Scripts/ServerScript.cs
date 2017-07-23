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

        Vector3 pos = new Vector3(10, 0, 0);
        foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {


            //spawn ship
            GameObject ship = (GameObject)Instantiate(Resources.Load("Prefabs/NetworkPrefabs/Ship") as GameObject, pos, Quaternion.identity);

            GameObject driver = (GameObject)Instantiate(Resources.Load("Prefabs/NetworkPrefabs/ShipConsoleDriver") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);

            GameObject turret = (GameObject)Instantiate(Resources.Load("Prefabs/NetworkPrefabs/ShipConsoleTurret") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);

            NetworkServer.Spawn(turret);

            NetworkServer.Spawn(driver);

            NetworkServer.Spawn(ship);


            //place users inside the ship
            RpcMoveIntoShip(ship, driver, turret, player); //in the case where a server is not also a client, will i have to run the same code for the server too?
            MoveIntoShip(ship, driver, turret, player);

            pos.x += 10f;
        }

       
    }

    [ClientRpc]
    public void RpcMoveIntoShip(GameObject ship, GameObject driver, GameObject turret, GameObject player) {
        MoveIntoShip(ship, driver, turret, player);
    }

    public void MoveIntoShip(GameObject ship, GameObject driver, GameObject turret, GameObject player) {

        //BUILDING SHIP
        Transform insideShip = ship.transform.Find("Inside").transform;

        driver.transform.SetParent(insideShip.Find("Spot_Driver"));
        driver.transform.localPosition = Vector2.zero;

        turret.transform.SetParent(insideShip.Find("Spot_Weapon"));
        turret.transform.localPosition = Vector2.zero;

        driver.GetComponent<ShipPilotScript>().InstantiatedOnNetwork();

        ////Moving players into ship
        //foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {

        //    player.GetComponent<PlayerController>().EnterShip(insideShip.gameObject);
        //}
        player.GetComponent<PlayerController>().EnterShip(insideShip.gameObject);
        //CameraFollow.access.AssignShip(ship.transform.Find("CameraPosition").gameObject);
    }

    [ClientRpc]
    public void RpcTimer(string text) {
        
        if (countdown == null) { countdown = GameObject.Find("Countdown").transform.GetComponent<Text>(); countdown.enabled = true; }
        if (text == "") { countdown.enabled = false; }
        countdown.text = text;
    }

}
        
    


