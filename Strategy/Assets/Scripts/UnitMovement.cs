using System.Collections;
using UnityEngine;

public class unitMovement : MonoBehaviour 
{
    public int health = 100;
    public int attack = 32;
    public float speed = 5f;
    private mouseClick mouseClick;
    private Vector3 movePoint;
    private arrowShoot arrowShoot;
    private GameObject hitObject;
    private GameObject crossbow;
    
    public void Start() {
        GameObject MouseManager = GameObject.Find("MouseManager");
        mouseClick = MouseManager.GetComponent<mouseClick>();
    }

    public void findAction() {
        movePoint = mouseClick.mouseMovePoint();
        hitObject = mouseClick.isObjectSelected();
        if (hitObject.tag == "Enemy") {
            if (gameObject.name == "Crossbowman" || gameObject.name == "Crossbowman(Clone)") {
                crossbow = gameObject.transform.GetChild(0).gameObject;
                arrowShoot = crossbow.GetComponent<arrowShoot>();
                gameObject.transform.LookAt(hitObject.transform);
                StopAllCoroutines();
                arrowShoot.arrowAttack();
            }
        } else {
            StopAllCoroutines();
            StartCoroutine(moveOverSpeed(gameObject, movePoint, speed));
        }
    }

    public IEnumerator moveOverSpeed(GameObject unit, Vector3 movePoint, float speed) {
        unit.transform.LookAt(movePoint);
        while(unit.transform.position != movePoint) {
            unit.transform.position = Vector3.MoveTowards(unit.transform.position, movePoint, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
