using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseClick : MonoBehaviour
{
    private unitMovement unitMovement;
    private GameObject hitObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            isObjectSelected();
            unitMovement = hitObject.GetComponent<unitMovement>();
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
            // selectObject(hitObject);
        } else {
            clearSelection();
        }
    }
    void clearSelection() {
        hitObject = null;
    }
}
