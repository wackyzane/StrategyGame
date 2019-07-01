using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowShoot : MonoBehaviour
{
    public GameObject unit;
    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    public float shootForce = 100f;

    public void arrowAttack(GameObject enemy) {
        Vector3 Vo = calculateVelocity(enemy.transform.position, unit.transform.GetChild(1).position, 1f);
        unit.transform.rotation = Quaternion.LookRotation(Vo);
        GameObject spawn = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity);
        Rigidbody rb = spawn.GetComponent<Rigidbody>();
        rb.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        rb.velocity = Vo;
    }

    Vector3 calculateVelocity(Vector3 target, Vector3 origin, float time) {
        // Define the distence x and y first
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;

        // Create a float the reporesent our distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;
        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }
}
