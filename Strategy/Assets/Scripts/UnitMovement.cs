using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitMovement : MonoBehaviour 
{
    public int health = 100;
    public int attack = 32;
    public float speed = 5f;
    public Coroutine moveCoroutine = null;
    public GameObject crossbow;

    private GameObject closestTarget;
    private mouseClick mouseClick;
    private Vector3 movePoint;
    private arrowShoot arrowShoot;
    private GameObject hitObject;
    private Coroutine arrow = null;
    
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
            Destroy(gameObject);
        }
        if (gameObject.tag == "unit" && mouseClick.enemies.Count > 0) {
            if (gameObject.name == "Crossbowman" || gameObject.name == "Crossbowman(Clone)") {
                GameObject closestTarget = GetClosestEnemy(mouseClick.enemies);
                arrowShoot = this.gameObject.GetComponentInChildren<arrowShoot>();
                float distance = Vector3.Distance(closestTarget.transform.position, gameObject.transform.position);
                if (distance <= arrowShoot.range) {
                    StopAllCoroutines();
                    if (arrow != null) {
                        StopCoroutine(arrow);
                    }
                    arrow = StartCoroutine(arrowShoot.arrowAttack(closestTarget));
                }
            }
            
        }
    }

    public void findAction() {
        movePoint = mouseClick.mouseMovePoint();
        hitObject = mouseClick.isObjectSelected();
        if (hitObject.tag == "Enemy") {
            //|| gameObject.name == "Bowman" || gameObject.name == "Bowman(Clone)"
            if (gameObject.name == "Crossbowman" || gameObject.name == "Crossbowman(Clone)") {
                arrowShoot = GetComponentInChildren<arrowShoot>();
                gameObject.transform.LookAt(hitObject.transform);
                StopAllCoroutines();
                if (arrow != null) {
                    StopCoroutine(arrow);
                }
                arrow = StartCoroutine(arrowShoot.arrowAttack(hitObject));
            } else if (gameObject.name == "Swordsman" || gameObject.name == "Swordsman(Clone)") {
                Debug.Log("Cool");
            }
        } else {
            StopAllCoroutines();
            if (arrow != null) {
                StopCoroutine(arrow);
            }
            moveCoroutine = StartCoroutine(moveOverSpeed(gameObject, movePoint, speed));
        }
    }

    public IEnumerator moveOverSpeed(GameObject unit, Vector3 movePoint, float speed) {
        unit.transform.LookAt(movePoint);
        while(unit.transform.position != movePoint) {
            unit.transform.position = Vector3.MoveTowards(unit.transform.position, movePoint, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
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
}
