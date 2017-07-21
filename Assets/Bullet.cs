using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    float timer = 0;
    float speed = 38f;
    Vector3 direction = Vector2.up;
    bool shoot = false;

    public void Shoot(Vector3 dir) {
        direction = dir;
        shoot = true;
    }

	void Update () {
        if (shoot) {
            Vector3 pos = transform.position;
            pos = pos + direction * speed * Time.deltaTime;
            transform.position = pos;
            timer += Time.deltaTime;
            if (timer > 10) {
                GameObject.Destroy(gameObject);
            }
        }
	}
}
