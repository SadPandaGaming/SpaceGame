using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ViewType { ship, player }

public class CameraManager : MonoBehaviour {

    public static CameraManager access;
   

    private ViewType viewType = ViewType.player;

    public PlayerMovement player;
    //[UnityEngine.SerializeField]
    private int playerOrthoView = 5;
    //[UnityEngine.SerializeField]
    private int shipOrthoView = 18;
    Camera cam;


    public void ShipView() {
        cam.orthographicSize = shipOrthoView;
        viewType = ViewType.ship;
    }

    public void PlayerView() {
        cam.orthographicSize = playerOrthoView;
        viewType = ViewType.player;

    }

    public void UpdatePlayer(PlayerMovement newPlayer) {
        player = newPlayer;
        if (player.viewType == ViewType.ship) {
            ShipView();
        }

        if (player.viewType == ViewType.player) {
            PlayerView();
        }
    }



    IEnumerator Start () {
		yield return 3;
        access = this;
        cam = GetComponent<Camera>();
	}
	
	void Update () {
        //if (player != null && player.viewPosition != null) {
        //    transform.position =  new Vector3(player.viewPosition.transform.position.x, player.viewPosition.transform.position.y, -10);
        //    transform.rotation = player.currentShip.transform.rotation;
        //    //if (viewType == ViewType.player) {
                
        //    //} else {
        //    //    transform.rotation = Quaternion.identity;
        //    //}
        //}
	}

}
