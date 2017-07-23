using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
    bool fire = false;
    float speed = 20f;
    public float lifeTime = 10f;

    [ClientRpc]
	public void RpcShoot() {
        //direction = dir;
        fire = true;
    }

    public void Shoot() {

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
