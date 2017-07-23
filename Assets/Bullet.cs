using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
    bool fire = false;
    float speed = 20f;
    public float lifeTime = 10f;
    GameObject owner;


    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.tag == "Ship") {
            print("hit");
            if (owner == GameManager.clientGameObject) {
                print("???!!!");

                if (GameManager.clientController.shipScript.gameObject != collision.gameObject) {

                    GameManager.clientController.CmdDamage(collision.gameObject, gameObject);
                    print("DMG!!!");
                }
            }
            
        }
    }

    [ClientRpc]
	public void RpcShoot(GameObject _owner) {
        owner = _owner;
        //direction = dir;
        fire = true;
    }

    public void Shoot(GameObject _owner) {
        owner = _owner;
        //direction = dir;
        fire = true;
    }

    [Server]
    public void BulletDestroy() {
        NetworkServer.Destroy(gameObject);
    }

    public void Update() {

        if (fire) {
            transform.position = (Vector2)transform.position + (Vector2)transform.up * speed  * Time.deltaTime;
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0) {
                //destroy
                if (isServer) {
                    BulletDestroy();
                }
            }
        }
    }
}
