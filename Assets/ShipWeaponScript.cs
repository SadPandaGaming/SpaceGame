using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShipWeaponScript : NetworkBehaviour {

    public GameObject turretPivot;

    float rotationSpeed = 7f;

    [SyncVar]
    [HideInInspector]
    public bool authorityEnabled = false;

    private void Start() {
        authorityEnabled = false;
        turretPivot = transform.Find("Pivot").gameObject;
    }

    //trigger requires a rigidbody on one of the components && the collider component to be marked as isTrigger
    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject == GameManager.clientGameObject) {
            GameManager.clientController.useAction = UseConsole;
            //display ui for using console hotkey
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.gameObject == GameManager.clientGameObject) {
            if (GameManager.clientController.useAction == UseConsole) {
                GameManager.clientController.useAction = null;
            }
        }
    }

    public void UseConsole() {
        if (!authorityEnabled) {

            authorityEnabled = true;
            GameManager.clientController.control = false;
            GameManager.clientController.CmdGainControl(gameObject);
        }
    }


    public void QuitConsole() {

        authorityEnabled = false;

        GameManager.clientController.CmdRemoveControl(gameObject);
        GameManager.clientController.control = true;
    }

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        print("start auth");
    }
    public override void OnStopAuthority() {
        base.OnStopAuthority();
        print("end auth");
    }

    float shotTimer = 0;
    float shotTimeWaited = 0.3f;

    [Command]
    void CmdShoot(/*float angle, Vector2 dir*/) {
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/NetworkPrefabs/Bullet") as GameObject, turretPivot.transform.Find("BulletSpawner").position, turretPivot.transform.rotation);
        NetworkServer.Spawn(go);
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.RpcShoot();
        bullet.Shoot();
    }

    

    public void Update() {

        if (!(hasAuthority && authorityEnabled)) { return; }

        if (shotTimer < shotTimeWaited) {
            shotTimer += Time.deltaTime;
        }

        
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = turretPivot.transform.position.z;
        Vector3 targetDir = mousePos - turretPivot.transform.position;

        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
        //turretPivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        turretPivot.transform.rotation = Quaternion.Slerp(turretPivot.transform.rotation, q, Time.deltaTime * rotationSpeed);
        
        if (Input.GetMouseButton(0)) {

            if (shotTimer > shotTimeWaited) {
                shotTimer = 0;
                CmdShoot(/*angle, targetDir*/);
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            QuitConsole();
        }


    }
}
