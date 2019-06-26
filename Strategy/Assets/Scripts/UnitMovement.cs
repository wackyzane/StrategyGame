using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public Transform unit;
    public float speed = 5f;
    private IEnumerator move;
    private bool unitSelected = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            isUnitSelected();
            Debug.Log(unitSelected);
        }
        if (Input.GetMouseButtonDown(1) && unitSelected == true) {
            findMovePoint();
        }
    }

    public bool isUnitSelected() {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity) && hit.transform.tag == "unit") {
            unitSelected = true;
            return unitSelected;
        } else {
            unitSelected = false;
            return unitSelected;
        }
    }

    public void findMovePoint() {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) {
            Vector3 movePoint = hit.point;
            movePoint.y += .5f;
            move = moveOverSpeed(gameObject, movePoint, speed);
            StopAllCoroutines();
            StartCoroutine(move);
        }
    }

    public IEnumerator moveOverSpeed(GameObject unit, Vector3 movePoint, float speed) {
        unit.transform.LookAt(movePoint);
        while(unit.transform.position != movePoint) {
            unit.transform.position = Vector3.MoveTowards(unit.transform.position, movePoint, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    // public IEnumerator moveOverSeconds(GameObject unit, Vector3 movePoint, float seconds) {
    //     float elapsedTime = 0;
    //     Vector3 startingPos = unit.transform.position;
    //     while (elapsedTime < seconds) {
    //         unit.transform.position = Vector3.Lerp(startingPos, movePoint, (elapsedTime / seconds));
    //         elapsedTime += Time.deltaTime;
    //         yield return new WaitForEndOfFrame();
    //     }
    //     transform.position = movePoint;
    // }
}