using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public Transform unit;

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        if (Input.GetMouseButtonDown(1)) {
            unit.transform.position = mouse;
        }
        // Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        // RaycastHit hit;
        // if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) {
        //     unit.transform.position = hit.point;
        // }
    }
}
