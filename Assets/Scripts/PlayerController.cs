using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    float speed = 3.5f;
    public LayerMask wallsLayer;

    public Action useAction = null;

    public bool control = true;

    public ShipPilotScript pilotScript;
    public ShipWeaponScript weaponScript;
    public Ship shipScript;


    [SyncVar]
    public float health = 30;
    [SyncVar]
    public float maxHealth = 30;

    public Transform healthParent;
    public Image healthSlider;

    [TargetRpc]
    public void TargetDamaged(NetworkConnection target) {

        
        healthSlider.fillAmount = health / maxHealth;

        if (health <= 0) {
            //dead
            print("DEAAAAAAAAAAAAAAAD");
        }

    }


    public void ShowHealth() {
        healthParent = GameManager.access.WorldCanvasObject.transform.Find("health");
        healthSlider = healthParent.Find("HealthSlider").GetComponent<Image>();
        healthParent.gameObject.SetActive(true);
    }


    //random test
    bool random = false;
    int[] randMove = { 1, -1 };
    int randomX = 0;
    int randomY = 0;
    float time = 0.2f;

    public void EnterShip(GameObject insideShip) {

        transform.SetParent(insideShip.transform);
        pilotScript = insideShip.transform.Find("Spot_Driver").GetComponentInChildren<ShipPilotScript>();
        weaponScript = insideShip.transform.Find("Spot_Weapon").GetComponentInChildren<ShipWeaponScript>();
        shipScript = insideShip.transform.parent.gameObject.GetComponent<Ship>();
        shipScript.pc = this;
        if (hasAuthority) {
            transform.localPosition = Vector3.zero;
            
            CameraFollow.access.AssignShip(insideShip.transform.parent.Find("CameraPosition").gameObject);
            ShowHealth();
            
        }

    }

    [Command]
    public void CmdDamage(GameObject ship, GameObject bullet) {
        ship.GetComponent<Ship>().pc.health -= 1;
        ship.GetComponent<Ship>().pc.TargetDamaged(ship.GetComponent<Ship>().pc.GetComponent<NetworkIdentity>().clientAuthorityOwner);
        NetworkServer.Destroy(bullet);
    }

    IEnumerator Start() {
        if (!hasAuthority) { yield break; }
        yield return 5; //wait 5 frames for other objects to finish starting up
        GetComponent<SpriteRenderer>().color = Color.green;
        GameManager.clientGameObject = gameObject;
        GameManager.clientController = this;
    }

    //[Command]
    //public void CmdSpawnBall(GameObject ownerObject) {
    //    GameObject go = (GameObject)Instantiate(ballPrefab);
    //    go.transform.position = transform.position + Vector3.up * 1.3f;
    //    NetworkServer.SpawnWithClientAuthority(go, ownerObject);
    //}

    [Command]
    public void CmdRemoveControl(GameObject targetObject) {
        targetObject.GetComponent<NetworkIdentity>().RemoveClientAuthority(gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner);
        //TargetRecieved(gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner);
    }

    [Command]
    public void CmdGainControl(GameObject targetObject) {
        targetObject.GetComponent<NetworkIdentity>().AssignClientAuthority(gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner);
        //TargetRecieved(gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner);
    }

    [Command]
    public void CmdShipStopped(GameObject ship, Vector3 position) {
        ship.transform.position = position;
    }

    [Command]
    public void CmdDestroyObject(GameObject target) {
        Network.Destroy(target);
    }


    //[TargetRpc]
    //public void TargetRecieved(NetworkConnection target) {
    //    print("called here!");
    //    if (serverCallback != null) {
    //        serverCallback();
    //    }
    //    serverCallback = null;
    //}

    void Update() {

        transform.rotation = Camera.main.transform.rotation;

        if (!hasAuthority) { return; }

        if (healthParent != null) {
            healthParent.transform.position = shipScript.transform.position + Vector3.down * 4.3f;
        }

        if (!control) { return; } //if control is given to another script

        if (useAction != null && Input.GetKeyDown(KeyCode.F)) {
            useAction();
            print("f was pressed");
        }

        //random activate
        if (Input.GetKeyDown(KeyCode.T)) {
            random = !random;
        }


        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = input.normalized;

        //random test
        if (random) {
            time += Time.deltaTime;
            if (time > 3) {
                time = 0;
                randomX = UnityEngine.Random.Range(0, 2);
                randomY = UnityEngine.Random.Range(0, 2);
            }
            input.x = randMove[randomX];
            input.y = randMove[randomY];
        }
        Vector2 pos = transform.position;
        
        if (CollisionX(transform.right.x * input.x)) { input.x = 0; }
        if (CollisionY(transform.up.y * input.y)) { input.y = 0; }
        

        //the left side: transform.right * -input.x
        //the left side: transform.up * input.y

        transform.position = pos + new Vector2(transform.right.x * input.x, transform.up.y * input.y) * speed * Time.deltaTime;


    }

    bool CollisionX(float x) {
        Debug.DrawRay(transform.position + new Vector3(x, 0, 0) * 0.75f, new Vector2(x, 0) * speed * Time.deltaTime, Color.red);
        return Physics2D.Raycast(transform.position, new Vector2(x, 0), 0.75f + x * speed * Time.deltaTime, wallsLayer);
    }

    bool CollisionY(float y) {
        Debug.DrawRay(transform.position + new Vector3(0, y, 0) * 0.75f, new Vector2(0, y) * speed * Time.deltaTime, Color.red);
        return Physics2D.Raycast(transform.position, new Vector2(0, y), 0.75f + y * speed * Time.deltaTime, wallsLayer);
    }
}
                   

        
    
