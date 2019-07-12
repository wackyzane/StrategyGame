using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttack : MonoBehaviour
{
    public float range = 1f;
    public float attackSpeed = 1f;
    public float attackCooldown = 0f;
    public bool isAttacking = false;
    private unitMovement unitMovement;
    private unitMovement enemyUnitMovement;
    private float lastAttackTime = 0f;
    private Animation anim;
    

    private void Awake() {
        attackCooldown = Time.time + 1;
    }

    public IEnumerator attack(GameObject enemy) {
        isAttacking = true;
        unitMovement = gameObject.GetComponent<unitMovement>();
        while (enemy != null) {
            var q = Quaternion.LookRotation(enemy.transform.position - gameObject.transform.position);
            // Turn towards movePoint
            while (gameObject.transform.rotation != q) {
                gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, q, unitMovement.rotateSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            if (Vector3.Distance(transform.position, enemy.transform.position) > Mathf.Abs(range)) {
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
                // anim = GetComponent<Animation>(); 
                // anim.Play();
            }
            yield return new WaitForEndOfFrame();
        }
        isAttacking = false;
        yield return null;
    }
}
