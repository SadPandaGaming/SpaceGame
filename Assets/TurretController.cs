using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurretController : NetworkBehaviour {

    GameObject cameraView = null;
    PlayerMovement pilot;

    //weapon class will take in the input
    float tempTimer = 0f;
    public GameObject tempBulletPrefab;
    public GameObject turret;

    bool turretActive = false;

    IEnumerator Start() {
        yield return 0;
        cameraView = transform.Find("CameraView").gameObject;
        turret = transform.Find("Turret").gameObject;
    }

    void OnTriggerEnter2D(Collider2D other) {
        PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
        if (pm != null) {
            pm.onUseAction = UseTurretConsole;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
        if (pm != null) {
            if (pm.onUseAction == UseTurretConsole) {
                pm.onUseAction = null;
            }
        }
    }

    public void UseTurretConsole(PlayerMovement user) {
        user.Immobile = true;
        user.onUseAction = ExitTurretConsole;
        //enter turret control mode

        user.ShipView(cameraView);
        turretActive = true;
        pilot = user;

    }


    //[Command]
    //public void GiveControl() {

    //}

    public void ExitTurretConsole(PlayerMovement user) {
        user.Immobile = false;
        user.onUseAction = UseTurretConsole;
        //leave turret control mode

        user.ExitShipView();
        turretActive = false;
        pilot = null;
    }

    public void Update() {

        //TEMP TIMER
        if (tempTimer < 0.4f) {
            tempTimer += Time.deltaTime;
        }
       
       
        if (turretActive && GameManager.access.currentPlayer == pilot) {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = target - turret.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            turret.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            if (Input.GetMouseButton(0)) {
                //tell the weapon that we are shooting!
                //code
                //your weapon.action();
                //end code
                //DEMO CODE

                if (tempTimer > 0.4f) {
                    tempTimer = 0;
                    //shoot
                    GameObject go = Instantiate(tempBulletPrefab, turret.transform.position, turret.transform.rotation);
                    go.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

                    go.GetComponent<Bullet>().Shoot(dir.normalized);
                }
                //end demo code
            }
        }
    }

}
