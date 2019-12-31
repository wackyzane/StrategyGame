using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitScript : MonoBehaviour {

    public List<GameObject> currentGroup;
    public float speed;
    public float rotateSpeed = 50f;

    private bool moving = false;
    private bool isTurning = false;
    private int groupCount = 0;
    private int groupNumber;
    private int leaderGroupCount;
    private Vector3 formationPos;
    private Vector3 oldLeaderPos;
    private Coroutine moveUnitCoroutine = null;
    private Coroutine rotateCoroutine = null;
    public GameObject leader = null;
    private GameObject formation;
    private GameObject mouseClick;
    private formationCalc formationCalc;
    private unitScript unitsInGroup;
    private unitScript leaderScript;

    void Awake() {
        currentGroup = new List<GameObject>();
        mouseClick = GameObject.Find("MouseManager");
        formation = GameObject.Find("MoveFormation");
        formationCalc = formation.GetComponent<formationCalc>();
    }

    void Update() {
        if (leader == gameObject) {
            if (currentGroup.Count != groupCount) {
                groupCount = 0;
                foreach (GameObject unit in currentGroup) {
                    unitsInGroup = unit.GetComponent<unitScript>();
                    unitsInGroup.setLeader(gameObject);
                    groupCount += 1;
                }
            }
            
        }
        if (leader != null && leader != gameObject) {
            if (leaderScript.moving) {
                if (leader.transform.position != oldLeaderPos) {
                    oldLeaderPos = leader.transform.position;
                    groupNumber = getGroupNumber();
                    formationPos = formationCalc.formation(leader, groupNumber);
                    stopMovement();
                    moveUnitCoroutine = StartCoroutine(moveUnit(formationPos));
                }
            }
        }
    }

    public void removeUnitGroup() {
        if (leader != null) {
            leaderScript = leader.GetComponent<unitScript>();
            leaderScript.currentGroup.Remove(gameObject);
        }
        if (currentGroup.Count > 0) {
            Debug.Log(currentGroup.Count);
            foreach (GameObject unit in currentGroup) {
                unitsInGroup = unit.GetComponent<unitScript>();
                unitsInGroup.leader = null;
            }
            currentGroup.Clear();
        }
    }

    public void setVisible() {
        foreach (Transform child in transform) {
            if (child.name == "Indicator") {
                Renderer visible = child.GetComponent<Renderer>();
                visible.enabled = true;
            }
        }
    }

    public void setInvisible() {
        foreach (Transform child in transform) {
            if (child.name == "Indicator") {
                Renderer visible = child.GetComponent<Renderer>();
                visible.enabled = false;
            }
        }
    }

    public void iAmFirst(Vector3 movePoint, bool odd) {
        movePoint.y += .5f;
        if (!odd) {
            movePoint.x += 1f;
        }
        stopMovement();
        moveUnitCoroutine = StartCoroutine(moveUnit(movePoint));
    }

    public void setLeader(GameObject unitHit) {
        leader = unitHit;
        leaderScript = unitHit.GetComponent<unitScript>();
    }

    private int getGroupNumber() {
        leaderGroupCount = leaderScript.currentGroup.Count - 1;
        for (int i = 0; i < leaderScript.currentGroup.Count; i++) {
            if (leaderScript.currentGroup[i] == gameObject) {
                groupNumber = i;
            }
        }
        return groupNumber;
    }

    private IEnumerator moveUnit(Vector3 movePoint) {
        moving = true;
        while (gameObject.transform.position != movePoint) {
            if (isTurning) {
                StopCoroutine(rotateCoroutine);
                isTurning = false;
            }
            rotateCoroutine = StartCoroutine(turnTowards(movePoint));
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePoint, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        moving = false;
    }

    private IEnumerator turnTowards(Vector3 mousePoint) {
        isTurning = true;
        var q = Quaternion.LookRotation(mousePoint - gameObject.transform.position);
        // Turn towards mousePoint
        while (gameObject.transform.rotation != q) {
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, q, rotateSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        isTurning = false;
    }

    private void stopMovement() {
        if (moving) {
            StopCoroutine(moveUnitCoroutine);
            moving = false;
        }
    }
}
