using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowAttack : MonoBehaviour
{
    Rigidbody myBody;
    private float lifeTimer = 10f;
    private float timer;
    private bool hitSomething = false;

    void Start()
    {
        myBody = GetComponent<Rigidbody>();
        if (transform.rotation != Quaternion.identity) {
            transform.rotation = Quaternion.LookRotation(myBody.velocity);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        // Destroy weapon
        // Need to only be on arrow
        if (timer >= lifeTimer) {
            Destroy(gameObject);
        }
        if (!hitSomething) {
            transform.rotation = Quaternion.LookRotation(myBody.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // Make it so arrows can't hit other arrows or weapons
        if (collision.collider.tag != "Weapon") {
            hitSomething = true;
            Stick(collision);
        }
    }

    private void Stick(Collision enemy) {
        myBody.constraints = RigidbodyConstraints.FreezeAll;
        if (enemy.collider.tag == "Enemy") {
            myBody.transform.parent = enemy.transform;
        }
    }
}
