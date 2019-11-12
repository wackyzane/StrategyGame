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
    public List<bool> moveAttack;
    public List<bool> hitEnemy;
    public List<GameObject> enemyToAttack;
    public bool currentEnemy = false;
    // public MaskLayer groundLayer;
    // public NavMeshAgent playerAgent;

    
    
    private mouseClick mouseClick;
    private arrowShoot arrowShoot;
    private meleeAttack meleeAttack;
    private formFormation formFormation;
    private GameObject hitObject;
    private Coroutine arrow = null;
    private Coroutine meleeDealDamage = null;
    private Coroutine unitMoveAttackCoroutine = null;
    private bool moving = false;
    private bool movingAttack = false;
    private bool movePointDestroy = false;
    private bool isTurning = false;
    
    private void Awake() {
        GameObject MouseManager = GameObject.Find("MouseManager");
        mouseClick = MouseManager.GetComponent<mouseClick>();
        movePoint = new List<Vector3>();
        moveAttack = new List<bool>();
        hitEnemy = new List<bool>();
        enemyToAttack = new List<GameObject>();
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
            movePoint.Clear();
            hitEnemy.Clear();
            moveAttack.Clear();
            closeCoroutines();
        }
        
        if (movePoint.Count > 0) {
            if (!moving) {
                closeCoroutines();
                moveCoroutine = StartCoroutine(moveOverSpeed(gameObject, movePoint[0], speed));
                // Change to formFormation
            }
            if (movePointDestroy) {
                movePoint.RemoveAt(0);
                if (moveAttack.Count > 0) {
                    moveAttack.RemoveAt(0);
                }
                if (hitEnemy.Count > 0) {
                    hitEnemy.RemoveAt(0);
                }
                movePointDestroy = false;
            }
        }
    }
    public void findAction() {
        Vector3 mousePoint = mouseClick.mouseMousePoint();
        hitObject = mouseClick.isObjectSelected();

        if (mousePoint != null) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                movePoint.Add(mousePoint);
                if (mouseClick.moveAttack) {
                    moveAttack.Add(true);
                    mouseClick.moveAttack = false;
                } else {
                    moveAttack.Add(false);
                }

                if (hitObject.tag == "Enemy") {
                    hitEnemy.Add(true);
                    enemyToAttack.Add(hitObject);
                } else {
                    hitEnemy.Add(false);
                }
            } else {
                movePoint.Clear();
                moveAttack.Clear();
                hitEnemy.Clear();
                enemyToAttack.Clear();
                movePoint.Add(mousePoint);
                if (mouseClick.moveAttack) {
                    moveAttack.Add(true);
                    mouseClick.moveAttack = false;
                } else {
                    moveAttack.Add(false);
                }

                if (hitObject.tag == "Enemy") {
                    hitEnemy.Add(true);
                    enemyToAttack.Add(hitObject);
                } else {
                    hitEnemy.Add(false);
                }
                closeCoroutines();
            }
        }
    }

    public IEnumerator moveOverSpeed(GameObject unit, Vector3 movePoint, float speed) {
        moving = true;
        // Move towards mousePoint
            if (moveAttack[0]) {
                if (!movingAttack) {
                    closeCoroutines();
                    unitMoveAttackCoroutine = StartCoroutine(attackWhileMoving(movePoint));
                    yield return new WaitForEndOfFrame();
                } else {
                    while (unit.transform.position != movePoint) {
                        yield return new WaitForEndOfFrame();
                    }
                }
            } else if (hitEnemy[0]) {
                currentEnemy = true;
                Debug.Log(currentEnemy);
                if (gameObject.name == "Crossbowman(Clone)" || gameObject.name == "Bowman(Clone)") {
                    arrowShoot = GetComponentInChildren<arrowShoot>();
                    if (!arrowShoot.shooting) {
                        closeCoroutines();
                        arrow = StartCoroutine(arrowShoot.arrowAttack(enemyToAttack[0]));
                    }
                } else if (gameObject.name == "Swordsman" || gameObject.name == "Swordsman(Clone)" || gameObject.name == "Pikeman" || gameObject.name == "Pikeman(Clone)" || gameObject.name == "Axemen" || gameObject.name == "Axemen(Clone)") {
                    meleeAttack = GetComponent<meleeAttack>();
                    if (!meleeAttack.isAttacking) {
                        closeCoroutines();
                        meleeDealDamage = StartCoroutine(meleeAttack.attack(enemyToAttack[0]));
                    }
                }
                // This isn't repeating
                while (currentEnemy == false) {
                    Debug.Log("Repeating");
                    yield return new WaitForEndOfFrame();
                }
            } else {
                while (unit.transform.position != movePoint) {
                    rotateCoroutine = StartCoroutine(turnTowards(unit, movePoint));
                    unit.transform.position = Vector3.MoveTowards(unit.transform.position, movePoint, speed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            }
            movePointDestroy = true;
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
        Debug.Log("Activated");
        while(gameObject.transform.position != movePoint) {
            if (gameObject.tag == "unit" && mouseClick.enemies.Count > 0) {
                if (gameObject.name == "Crossbowman" || gameObject.name == "Crossbowman(Clone)") {
                    arrowShoot = gameObject.GetComponentInChildren<arrowShoot>();
                    if (!arrowShoot.shooting) {
                        GameObject closestTarget = GetClosestEnemy(mouseClick.enemies);
                        float distance = Vector3.Distance(closestTarget.transform.position, gameObject.transform.position);
                        if (distance <= arrowShoot.range && !isTurning) {
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
        moving = false;
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
