using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseClick : MonoBehaviour
{
    public GameObject SwordsmanPrefab;
    public GameObject CrossbowmanPrefab;
    public string swordsmanSpawnHotkey = "f";
    public string crossbowmanSpawnHotkey = "g";
    private GameObject hitObject;
    private Vector3 movePoint;
    private unitMovement unitMove;
    private bool fTrue = false;
    private bool hotKey = false;

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
