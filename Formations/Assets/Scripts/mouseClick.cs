using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseClick : MonoBehaviour {

    public List<GameObject> currentSelected;

    private Vector3 mousePoint;
    private GameObject unitHit;
    private unitScript unitScript;
    private unitScript firstUnit;
    private bool unitAlreadyHit = false;
    private bool odd;

    void Start() {
        currentSelected = new List<GameObject>();
    }

    void Update() {

        if (Input.GetMouseButtonDown(0)) {
            unitHit = hitUnit();
            addUnitSelected(unitHit);
        }

        if (Input.GetMouseButtonDown(1)) {
            // Move Unit
            mousePoint = hitPoint();
            // Formation move units to point, if there are units selected.
            if (currentSelected.Count > 0) {
                moveUnitFormation(mousePoint);
            }
        }

    }

    private void addUnitSelected(GameObject unitHit) {

        foreach (GameObject unit in currentSelected) {
            if (unitHit == unit) {
                unitAlreadyHit = true;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            if (!unitAlreadyHit) {
                if (unitHit != null) {
                    currentSelected.Add(unitHit);
                    setVis(unitHit);
                }
            } else {
                if (unitHit != null) {
                    currentSelected.Remove(unitHit);
                    setInv(unitHit);
                }
                unitAlreadyHit = false;
            }
        } else {
            unitAlreadyHit = false;
            foreach (GameObject unit in currentSelected) {
                setInv(unit);
            }
            currentSelected.Clear();
            if (unitHit != null) {
                currentSelected.Add(unitHit);
                setVis(unitHit);
            }
        }
    }

    private void moveUnitFormation(Vector3 mousePoint) {
        // Take all units in currentSelected out of their group
        // Add currentSelected to leader's currentGroup
        // Set all units leader in leader's currentGroup to leader
        firstUnit = currentSelected[0].GetComponent<unitScript>();
        foreach (GameObject unit in firstUnit.currentGroup) {
            unitScript = unit.GetComponent<unitScript>();
            unitScript.leader = null;
        }
        firstUnit.currentGroup.Clear();
        foreach (GameObject unit in currentSelected) {
            unitScript = unit.GetComponent<unitScript>();
            unitScript.removeUnitGroup();
            unitScript.currentGroup.Clear();
            unitScript.setLeader(currentSelected[0]);
            if (unit != currentSelected[0]) {
                firstUnit.currentGroup.Add(unit);
            }
        }
        
        if (currentSelected.Count % 2 == 1) {
            odd = true;
        } else {
            odd = false;
        }
        firstUnit.iAmFirst(mousePoint, odd);
    }

    private void setInv(GameObject unit) {
        unitScript = unit.GetComponent<unitScript>();
        unitScript.setInvisible();
    }

    private void setVis(GameObject unit) {
        unitScript = unitHit.GetComponent<unitScript>();
        unitScript.setVisible();
    }
    
    public GameObject hitUnit() {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Units"))) {
            GameObject unitHit = hit.transform.root.gameObject;
            return unitHit;
        } else {
            return null;
        }
    }

    public Vector3 hitPoint() {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"))) {
            Vector3 mousePoint = hit.point;
            return mousePoint;
        }
        return mousePoint;
    }
}
