using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;

public class unitMovement : MonoBehaviour 
{
    public int health = 100;
    public int attack = 32;
    public float speed = 5f;
    public float rotateSpeed = 500f;
    public Coroutine moveCoroutine = null;
    public Coroutine rotateCoroutine = null;
    public List<Vector3> movePoint;

    // public MaskLayer groundLayer;
    // public NavMeshAgent playerAgent;

    private bool isTurning = false;
    private mouseClick mouseClick;
    private arrowShoot arrowShoot;
    private meleeAttack meleeAttack;
    private GameObject hitObject;
    private Coroutine arrow = null;
    private Coroutine meleeDealDamage = null;
    private Coroutine unitMoveAttackCoroutine = null;
    private bool moving = false;
    private bool movingAttack = false;
    public List<bool> moveAttack;
    private List<bool> setTrue;
    
    
    private void Awake() {
        GameObject MouseManager = GameObject.Find("MouseManager");
        mouseClick = MouseManager.GetComponent<mouseClick>();
        movePoint = new List<Vector3>();
        moveAttack = new List<bool>();
        setTrue = new List<bool>();
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
        // if (gameObject.tag == "unit" && mouseClick.enemies.Count > 0) {
        //     isEnemyInRange();
        // }
        if (Input.GetKey("b")) {
            closeCoroutines();
        }
    }

    public void findAction() {
        Vector3 mousePoint = mouseClick.mouseMousePoint();
        hitObject = mouseClick.isObjectSelected();

        if (mousePoint != null) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                // if (hitobject.tag == "Enemy") {
                    // movePoint.add(hitobject);
                // } else {
                    // movePoint.Add(mousePoint);
                // }
                movePoint.Add(mousePoint);
            } else {
                movePoint.Clear();
                // if (hitobject.tag == "Enemy") {
                    // movePoint.add(hitobject);
                // } else {
                    // movePoint.Add(mousePoint);
                // }
                movePoint.Add(mousePoint);
            }
            if (mouseClick.moveAttack) {
                // Checking to see if anything in moveAttack list is true
                for (int i = 0; i < moveAttack.Count; i++) {
                    if (moveAttack[i] == true) {
                        // If True add the number it is on to setTrue list
                        setTrue.Add(true);
                    } else {
                        setTrue.Add(false);
                    }
                }
                // Clear the moveAttack List so we can remake it
                moveAttack.Clear();
                for (int i = 0; i < setTrue.Count; i++) {
                    if (setTrue[i] != true) {
                        moveAttack.Add(false);
                    } else {
                        moveAttack.Add(true);
                    }
                }
                while (moveAttack.Count != movePoint.Count) {
                    moveAttack.Add(false);
                }
                moveAttack.Add(mouseClick.moveAttack);
                mouseClick.moveAttack = false;
            }

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
    }

    public IEnumerator moveOverSpeed(GameObject unit, List<Vector3> movePoint, float speed) {
        moving = true;
        // Move towards mousePoint
        while (movePoint.Count > 0) {
            if (moveAttack.Count > 0) {
                if (moveAttack[0]) {
                    closeCoroutines();
                    if (!movingAttack) {
                        closeCoroutines();
                        unitMoveAttackCoroutine = StartCoroutine(attackWhileMoving(movePoint[0]));
                        yield return new WaitForEndOfFrame();
                    } else {
                        if (unit.transform.position != movePoint[0]) {
                            Debug.Log("Waiting for Unit to get to MovePoint");
                            yield return new WaitForEndOfFrame();
                        } else {
                            movePoint.RemoveAt(0);
                            if (moveAttack.Count > 0) {
                                moveAttack.RemoveAt(0);
                            }
                        }
                    }
                    // while(unit.transform.position != movePoint[0]) {
                    //     yield return new WaitForEndOfFrame();
                    // }
                    // movePoint.RemoveAt(0);
                    // if (moveAttack.Count > 0) {
                    //     moveAttack.RemoveAt(0);
                    // }
                } else {
                    while (unit.transform.position != movePoint[0]) {
                        rotateCoroutine = StartCoroutine(turnTowards(unit, movePoint[0]));
                        unit.transform.position = Vector3.MoveTowards(unit.transform.position, movePoint[0], speed * Time.deltaTime);
                        yield return new WaitForEndOfFrame();
                    }
                    movePoint.RemoveAt(0);
                    if (moveAttack.Count > 0) {
                        moveAttack.RemoveAt(0);
                    }
                }
            } else {
                while(unit.transform.position != movePoint[0]) {
                    rotateCoroutine = StartCoroutine(turnTowards(unit, movePoint[0]));
                    unit.transform.position = Vector3.MoveTowards(unit.transform.position, movePoint[0], speed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                movePoint.RemoveAt(0);
                if (moveAttack.Count > 0) {
                    moveAttack.RemoveAt(0);
                }
            }
        }
        moving = false;
        yield return null;
    }

    public IEnumerator turnTowards(GameObject unit, Vector3 mousePoint) {
        isTurning = true;
        var q = Quaternion.LookRotation(mousePoint - unit.transform.position);
        // Turn towards mousePoint
        while (unit.transform.rotation != q) {
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
        movingAttack = true;
        while(gameObject.transform.position != movePoint) {
            Debug.Log("Not at MovePoint Yet");
            if (gameObject.tag == "unit" && mouseClick.enemies.Count > 0) {
                if (gameObject.name == "Crossbowman" || gameObject.name == "Crossbowman(Clone)") {
                    arrowShoot = gameObject.GetComponentInChildren<arrowShoot>();
                    if (!arrowShoot.shooting) {
                        Debug.Log("Not Shooting");
                        GameObject closestTarget = GetClosestEnemy(mouseClick.enemies);
                        float distance = Vector3.Distance(closestTarget.transform.position, gameObject.transform.position);
                        if (distance <= arrowShoot.range && !isTurning) {
                            Debug.Log("Enemy is Close enough to attack! And I have Finished Turning");
                            arrow = StartCoroutine(arrowShoot.arrowAttack(closestTarget));
                        } else {
                            if (!isTurning) {
                                rotateCoroutine = StartCoroutine(turnTowards(gameObject, movePoint));
                            }
                            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePoint, speed * Time.deltaTime);
                        }
                    }
                    yield return new WaitForEndOfFrame();
                } else if (gameObject.name == "Swordsman" || gameObject.name == "Swordsman(Clone)" || gameObject.name == "Pikeman" || gameObject.name == "Pikeman(Clone)" || gameObject.name == "Axemen" || gameObject.name == "Axemen(Clone)") {
                    meleeAttack = gameObject.GetComponent<meleeAttack>();
                    if (!meleeAttack.isAttacking) {
                        GameObject closestTarget = GetClosestEnemy(mouseClick.enemies);
                        float distance = Vector3.Distance(closestTarget.transform.position, gameObject.transform.position);
                        if (distance <= 20f && !isTurning) {
                            meleeDealDamage = StartCoroutine(meleeAttack.attack(closestTarget));
                        } else {
                            if (!isTurning) {
                                rotateCoroutine = StartCoroutine(turnTowards(gameObject, movePoint));
                            }
                            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePoint, speed * Time.deltaTime);
                        }
                    }
                    yield return new WaitForEndOfFrame();
                } else {
                    Debug.Log("WTF, How can you not be a unit type?");
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePoint, speed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            } else {
                if (!isTurning) {
                    rotateCoroutine = StartCoroutine(turnTowards(gameObject, movePoint));
                }
                Debug.Log("There is no units on the map???? Game Should Probably End");
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePoint, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        mouseClick.moveAttack = false;
        movingAttack = false;
    }

    private void isEnemyInRange() {
        if (gameObject.name == "Crossbowman" || gameObject.name == "Crossbowman(Clone)") {
            arrowShoot = gameObject.GetComponentInChildren<arrowShoot>();
            if (!arrowShoot.shooting && !moving && !isTurning && !movingAttack) {
                GameObject closestTarget = GetClosestEnemy(mouseClick.enemies);
                float distance = Vector3.Distance(closestTarget.transform.position, gameObject.transform.position);
                if (distance <= arrowShoot.range) {
                    arrow = StartCoroutine(arrowShoot.arrowAttack(closestTarget));
                }
            }
        } else if (gameObject.name == "Swordsman" || gameObject.name == "Swordsman(Clone)" || gameObject.name == "Pikeman" || gameObject.name == "Pikeman(Clone)" || gameObject.name == "Axemen" || gameObject.name == "Axemen(Clone)") {
            meleeAttack = gameObject.GetComponent<meleeAttack>();
            if (!meleeAttack.isAttacking && !isTurning && !moving && !movingAttack) {
                GameObject closestTarget = GetClosestEnemy(mouseClick.enemies);
                float distance = Vector3.Distance(closestTarget.transform.position, gameObject.transform.position);
                if (distance <= 20f) {
                    meleeDealDamage = StartCoroutine(meleeAttack.attack(closestTarget));
                }
            }
        }
    }

    public void closeCoroutines() {
        StopAllCoroutines();
        // Need to cancel Tab pressed if want to cancel action
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
        if (unitMoveAttackCoroutine != null) {
            StopCoroutine(unitMoveAttackCoroutine);
            movingAttack = false;
            mouseClick.moveAttack = false;
        }
    }
}
