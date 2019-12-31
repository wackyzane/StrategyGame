using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour {

    private float lifeSpan = 10f;
    private float startLifeSpan;
    private bool groundHit = false;

    void Awake() {
        startLifeSpan = Time.time;
    }

    void Update() {
        if (Time.time - startLifeSpan > lifeSpan) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.collider.name);
        // if (collision.collider. == ) {
        //     groundHit = true;
        // }
        if (collision.collider.tag == "Player") {
            Debug.Log(collision.collider.name);
        }
    }
}
