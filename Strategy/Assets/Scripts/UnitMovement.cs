﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitMovement : MonoBehaviour 
{
    public int health = 100;
    public int attack = 32;
    public float speed = 5f;
    public float rotateSpeed = 500f;
    public Coroutine moveCoroutine = null;
    
    private mouseClick mouseClick;
    private Vector3 movePoint;
    private arrowShoot arrowShoot;
    private meleeAttack meleeAttack;
    private GameObject hitObject;
    private Coroutine arrow = null;
    private Coroutine meleeDealDamage = null;
    private Coroutine rotateCoroutine = null;
    private bool moving = false;
    private bool isTurning = true;
    
    private void Awake() {
        GameObject MouseManager = GameObject.Find("MouseManager");
        mouseClick = MouseManager.GetComponent<mouseClick>();
    }

    private void Start() {
        if (gameObject.tag == "unit") {
            mouseClick.selectableObjects.Add(gameObject);
        } else if (gameObject.tag == "Enemy") {
            mouseClick.enemies.Add(gameObject);
        }
    }

    private void Update() {
        // Change this to Health Script?
        if (health <= 0) {
            if (gameObject.tag == "Enemy") {
                mouseClick.enemies.Remove(gameObject);
            } else if (gameObject.tag == "unit") {
                mouseClick.selectableObjects.Remove(gameObject);
            }
            Destroy(gameObject);
        }
        if (gameObject.tag == "unit" && mouseClick.enemies.Count > 0) {
            isEnemyInRange();
        }
    }

    public void findAction() {
        movePoint = mouseClick.mouseMovePoint();
        hitObject = mouseClick.isObjectSelected();
        if (hitObject.tag == "Enemy") {
            //|| gameObject.name == "Bowman" || gameObject.name == "Bowman(Clone)"
            if (gameObject.name == "Crossbowman" || gameObject.name == "Crossbowman(Clone)") {
                arrowShoot = GetComponentInChildren<arrowShoot>();
                closeCoroutines();
                arrow = StartCoroutine(arrowShoot.arrowAttack(hitObject));
            } else if (gameObject.name == "Swordsman" || gameObject.name == "Swordsman(Clone)" || gameObject.name == "Pikeman" || gameObject.name == "Pikeman(Clone)" || gameObject.name == "Axemen" || gameObject.name == "Axemen(Clone)") {
                meleeAttack = GetComponent<meleeAttack>();
                closeCoroutines();
                meleeDealDamage = StartCoroutine(meleeAttack.attack(hitObject));
            }
        } else {
            closeCoroutines();
            moveCoroutine = StartCoroutine(moveOverSpeed(gameObject, movePoint, speed));
        }
    }

    public IEnumerator moveOverSpeed(GameObject unit, Vector3 movePoint, float speed) {
        moving = true;
        rotateCoroutine = StartCoroutine(turnTowards(unit, movePoint));
        // Move towards movePoint
        while(unit.transform.position != movePoint) {
            unit.transform.position = Vector3.MoveTowards(unit.transform.position, movePoint, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        moving = false;
        yield return null;
    }

    public IEnumerator turnTowards(GameObject unit, Vector3 movePoint) {
        isTurning = true;
        var q = Quaternion.LookRotation(movePoint - unit.transform.position);
        // Turn towards movePoint
        while (unit.transform.rotation != q) {
            // rotateCoroutine = StartCoroutine(turnTowards(unit, q));
            unit.transform.rotation = Quaternion.RotateTowards(unit.transform.rotation, q, rotateSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        isTurning = false;
    }

    public void setVisible() {
        foreach (Transform child in transform) {
            if (child.name == "Selection Indicator") {
                Renderer visible = child.GetComponent<Renderer>();
                visible.enabled = true;
            }
        }
    }

    public void setInvisible() {
        foreach (Transform child in transform) {
            if (child.name == "Selection Indicator") {
                Renderer visible = child.GetComponent<Renderer>();
                visible.enabled = false;
            }
        }
    }

    private GameObject GetClosestEnemy(List<GameObject> enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    public IEnumerator attackWhileMoving(Vector3 movePoint) {
        moving = true;
        rotateCoroutine = StartCoroutine(turnTowards(gameObject, movePoint));
        while(gameObject.transform.position != movePoint) {
            if (gameObject.tag == "unit" && mouseClick.enemies.Count > 0) {
                if (gameObject.name == "Crossbowman" || gameObject.name == "Crossbowman(Clone)") {
                    arrowShoot = this.gameObject.GetComponentInChildren<arrowShoot>();
                    if (arrowShoot.shooting == false && isTurning == false) {
                        GameObject closestTarget = GetClosestEnemy(mouseClick.enemies);
                        float distance = Vector3.Distance(closestTarget.transform.position, gameObject.transform.position);
                        if (distance <= arrowShoot.range) {
                            arrow = StartCoroutine(arrowShoot.arrowAttack(closestTarget));
                        } else {
                            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePoint, speed * Time.deltaTime);
                        }
                    }
                    yield return new WaitForEndOfFrame();
                } else if (gameObject.name == "Swordsman" || gameObject.name == "Swordsman(Clone)" || gameObject.name == "Pikeman" || gameObject.name == "Pikeman(Clone)" || gameObject.name == "Axemen" || gameObject.name == "Axemen(Clone)") {
                    // meleeAttack = gameObject.GetComponent<meleeAttack>();
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePoint, speed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                } else {
                    Debug.Log("WTF How can you not be a unit type?");
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePoint, speed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            } else {
                Debug.Log("There is no units on the map???? Game Should Probably End");
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePoint, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        moving = false;
    }

    private void isEnemyInRange() {
        if (gameObject.name == "Crossbowman" || gameObject.name == "Crossbowman(Clone)") {
            arrowShoot = this.gameObject.GetComponentInChildren<arrowShoot>();
            if (arrowShoot.shooting == false && moving == false && isTurning == false) {
                GameObject closestTarget = GetClosestEnemy(mouseClick.enemies);
                float distance = Vector3.Distance(closestTarget.transform.position, gameObject.transform.position);
                if (distance <= arrowShoot.range) {
                    arrow = StartCoroutine(arrowShoot.arrowAttack(closestTarget));
                }
            }
        }
        // } else if (gameObject.name == "Swordsman" || gameObject.name == "Swordsman(Clone)" || gameObject.name == "Pikeman" || gameObject.name == "Pikeman(Clone)" || gameObject.name == "Axemen" || gameObject.name == "Axemen(Clone)") {
        //     meleeAttack = gameObject.GetComponent<meleeAttack>();

        // }
    }

    public void closeCoroutines() {
        StopAllCoroutines();
        if (arrow != null) {
            StopCoroutine(arrow);
            arrowShoot.shooting = false;
        }
        if (meleeDealDamage != null) {
            StopCoroutine(meleeDealDamage);
            meleeAttack.isAttacking = false;
        }
        if (rotateCoroutine != null) {
            StopCoroutine(rotateCoroutine);
            isTurning = false;
        }
        if (mouseClick.unitMoveAttackCoroutine != null) {
            StopCoroutine(mouseClick.unitMoveAttackCoroutine);
            moving = false;
        }
    }
}
