using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    public static GameManager access;
    public Sprite whiteBlock;
    Vector3 input = Vector2.zero;

    //public NetworkIdentity client;


    public PlayerMovement currentPlayer;
    GameObject playerSymbol;

    public GameObject ship;

    //public PlayerMovement[] players;

    IEnumerator Start() {
        access = this;
        
        yield return 2;
        ship = GameObject.Find("Ship");
        yield return 4;

        //SwitchPlayer(3);

    }

    public void SelectPlayer(PlayerMovement player) {
        //currentPlayer = player;
        //player.currentShip = ship;
        //player.ExitShipView();
        //player.transform.SetParent(ship.transform.FindChild("Inside").transform);
        //player.transform.localPosition = Vector2.zero;
        //CameraManager.access.player = player;
    }

    public void SwitchPlayer(int num) {
        //if (currentPlayer != null) {
        //    currentPlayer.playerActive = false;
        //}
        

        ////currentPlayer = players[num];

        //if (playerSymbol == null) {
        //    playerSymbol = currentPlayer.transform.Find("p1").gameObject;
        //}

        //CameraManager.access.UpdatePlayer(currentPlayer);

        //playerSymbol.transform.SetParent(currentPlayer.transform);
        //playerSymbol.transform.localPosition = new Vector3(0.1f, 0.42f, 0);
        //currentPlayer.playerActive = true;
    }

    // Update is called once per frame
    void Update () {
        if (currentPlayer == null) { return; }

        //if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //    SwitchPlayer(3);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2)) {
        //    SwitchPlayer(0);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3)) {
        //    SwitchPlayer(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4)) {
        //    SwitchPlayer(2);
        //}

        input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        if (Input.GetKeyDown(KeyCode.F)) {
            //USE EVENT
            currentPlayer.UseAction();
        }

        currentPlayer.Movement(input);
    }
}


public static class Vector2Extension {

    public static Vector2 Rotate(this Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
