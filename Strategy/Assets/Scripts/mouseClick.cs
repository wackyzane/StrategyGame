using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseClick : MonoBehaviour
{
    public GameObject hitObject;
    public GameObject SwordsmanPrefab;
    public string unitSpawnHotkey = "f";
    private Vector3 movePoint;
    private unitMovement unitMove;
    private bool fTrue = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(unitSpawnHotkey)) {
            fTrue = true;
        }
        if (Input.GetMouseButtonDown(0)) {
            if (fTrue) {
                movePoint = mouseMovePoint();
                Instantiate(SwordsmanPrefab, movePoint, Quaternion.identity);
                fTrue = false;
            }
            hitObject = isObjectSelected();
            try {
                if (hitObject.tag == "unit") {
                    unitMove = hitObject.GetComponent<unitMovement>();
                }
            } catch {
                Debug.Log(hitObject.tag);
            }
        }
        if (Input.GetMouseButtonDown(1) && hitObject != null) {
            if(hitObject.tag == "unit") {
                unitMove.findAction();
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
