using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseClick : MonoBehaviour
{
    private unitMovement unitMovement;
    public GameObject hitObject;
    public GameObject unitPrefab;
    private bool fTrue = false;
    private Vector3 movePoint;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("f")) {
            fTrue = true;
        }
        if (Input.GetMouseButtonDown(0)) {
            isObjectSelected();
            // if (fTrue) {
            //     Instantiate(unitPrefab, movePoint, Quaternion.identity);
            //     fTrue = false;
            // }
            try {
                if (hitObject.tag == "unit") {
                    unitMovement = hitObject.GetComponent<unitMovement>();
                }
            } catch {
                Debug.Log(hitObject.tag);
            }
        }
        if (Input.GetMouseButtonDown(1) && hitObject != null) {
            if(hitObject.tag == "unit") {
                unitMovement.findMovePoint();
            }
        }
    }

    void isObjectSelected() {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) {
            hitObject = hit.transform.root.gameObject;
        }
    }
    
    // Tuple<Vector3, GameObject> mouseRaycast() {
    //     Vector3 mouse = Input.mousePosition;
    //     Ray castPoint = Camera.main.ScreenPointToRay(mouse);
    //     RaycastHit hit;
    //     if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) {
    //         hitObject = hit.transform.root.gameObject;
    //         Vector3 movePoint = hit.point;
    //         movePoint.y += .5f;
    //         if (type == "movePoint") {
    //             return movePoint;
    //         } else if (type == "hitObject") {
    //             return hitObject;
    //         }
    //     }
    // }
}
