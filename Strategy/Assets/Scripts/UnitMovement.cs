using System.Collections;
using UnityEngine;

public class unitMovement : MonoBehaviour 
{
    public int health = 100;
    public int attack = 32;
    public float speed = 5f;
    private Transform unit;
    private mouseClick mouseClick;
    private Vector3 movePoint;
    
    public void Start() {
        GameObject MouseManager = GameObject.Find("MouseManager");
        mouseClick = MouseManager.GetComponent<mouseClick>();
    }

    public void findMovePoint() {
        movePoint = mouseClick.mouseMovePoint();
        StopAllCoroutines();
        StartCoroutine(moveOverSpeed(gameObject, movePoint, speed));
    }

    public IEnumerator moveOverSpeed(GameObject unit, Vector3 movePoint, float speed) {
        unit.transform.LookAt(movePoint);
        while(unit.transform.position != movePoint) {
            unit.transform.position = Vector3.MoveTowards(unit.transform.position, movePoint, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
