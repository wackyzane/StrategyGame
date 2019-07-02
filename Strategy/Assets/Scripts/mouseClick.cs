using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseClick : MonoBehaviour
{
    public GameObject SwordsmanPrefab;
    public GameObject CrossbowmanPrefab;
    public string swordsmanSpawnHotkey = "f";
    public string crossbowmanSpawnHotkey = "g";
    public List<GameObject> selectedObjects;
    private GameObject hitObject;
    private Vector3 movePoint;
    private unitMovement unitMove;
    private bool fTrue = false;
    private bool hotKey = false;
    private bool hasSelected = false;

    private void Start() {
        selectedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(swordsmanSpawnHotkey)) {
            fTrue = true;
        }
        if (Input.GetKey(crossbowmanSpawnHotkey)) {
            hotKey = true;
        }
        if (Input.GetMouseButtonDown(0)) {
            movePoint = mouseMovePoint();
            if (fTrue) {
                Instantiate(SwordsmanPrefab, movePoint, Quaternion.identity);
                fTrue = false;
            } else if (hotKey) {
                Instantiate(CrossbowmanPrefab, movePoint, Quaternion.identity);
                hotKey = false;
            }
            hitObject = isObjectSelected();
            if (Input.GetKey("left ctrl")) {
                if (hitObject.tag == "unit") {
                    // UI changes to unit stats
                    foreach (GameObject obj in selectedObjects) {
                        if (obj == hitObject) {
                            hasSelected = true;
                        }
                    }
                    if (hasSelected ==  true) {
                        selectedObjects.Remove(hitObject);
                        hasSelected = false;
                    } else {
                        selectedObjects.Add(hitObject);
                    }
                } else if (hitObject.tag == "enemy") {
                    // UI changes to enemy unit stats
                    selectedObjects.Clear();
                }
            } else {
                if (hitObject.tag == "unit") {
                    selectedObjects.Clear();
                    selectedObjects.Add(hitObject);
                    // unitMove = hitObject.GetComponent<unitMovement>();
                } else {
                    selectedObjects.Clear();
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && hitObject != null) {
            if(selectedObjects.Count > 0) {
                for (int i = 0; i < selectedObjects.Count; i++) {
                    unitMove = selectedObjects[i].GetComponent<unitMovement>();
                    unitMove.findAction();
                }
                // unitMove.findAction();
            }
        }
    }

    public GameObject isObjectSelected() {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) {
            GameObject hitObject = hit.transform.root.gameObject;
            return hitObject;
        }
        return hitObject;
    }
    
    public Vector3 mouseMovePoint() {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) {
            Vector3 movePoint = hit.point;
            movePoint.y += .5f;
            return movePoint;
        }
        return movePoint;
    }
}
