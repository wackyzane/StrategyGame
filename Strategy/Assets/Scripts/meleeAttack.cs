using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttack : MonoBehaviour
{
    public float range = 1f;
    public float attackSpeed = 1f;
    public float attackCooldown = 0f;
    private unitMovement unitMovement;
    private unitMovement enemyUnitMovement;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;

    private void Awake() {
        attackCooldown = Time.time + 1;
    }

    public IEnumerator attack(GameObject enemy) {
        isAttacking = true;
        while (enemy != null) {
            if (Vector3.Distance(transform.position, enemy.transform.position) > Mathf.Abs(range)) {
                unitMovement = gameObject.GetComponent<unitMovement>();
                while (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) > Mathf.Abs(range)) {
                    transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, unitMovement.speed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            }
            attackCooldown = Time.time - lastAttackTime;
            if (attackCooldown >= 1f / attackSpeed && enemy != null) {
                lastAttackTime = Time.time;
                enemyUnitMovement = enemy.GetComponent<unitMovement>();
                enemyUnitMovement.health -= unitMovement.attack;
            }
            yield return new WaitForEndOfFrame();
        }
        isAttacking = false;
        yield return null;
    }
}
