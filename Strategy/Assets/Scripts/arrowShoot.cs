using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowShoot : MonoBehaviour
{
    public GameObject unit;
    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    public float shootForce = 20f;

    public void arrowAttack() {
        GameObject spawn = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity);
        spawn.transform.parent = unit.transform;
        Rigidbody rb = spawn.GetComponent<Rigidbody>();
        rb.velocity = unit.transform.forward * shootForce;
    }
}