using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseClick : MonoBehaviour
{
    public GameObject SwordsmanPrefab;
    public GameObject CrossbowmanPrefab;
    public string swordsmanSpawnHotkey = "f";
    public string crossbowmanSpawnHotkey = "g";
    public RectTransform selectSquareImage;
    public List<GameObject> selectedObjects;
    public List<GameObject> selectableObjects;
    public List<GameObject> enemies;
    public float currentlySelected = 0f;
    private float firstSelect = 0f;
    private float secondSelect = 0f;
    private GameObject hitObject;
    private GameObject lastSelectedUnit = null;
    private Vector3 movePoint;
    private unitMovement unitMovemet;
    private bool fTrue = false;
    private bool hotKey = false;
    private bool hasSelected = false;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 mousePos1;
    private Vector3 mousePos2;
    

    private void Awake() {
        selectSquareImage.gameObject.SetActive(false);
        selectedObjects = new List<GameObject>();
        selectableObjects = new List<GameObject>();
        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(swordsmanSpawnHotkey)) {
            fTrue = true;
            hotKey = false;
        }

        if (Input.GetKey(crossbowmanSpawnHotkey)) {
            hotKey = true;
            fTrue = false;
        }

        if (Input.GetMouseButtonDown(0)) {
            movePoint = mouseMovePoint();
            startPos = movePoint;
            startPos.y -= .5f;
            mousePos1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
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
                    // Add On Double click
                    if (lastSelectedUnit == hitObject) {
                        secondSelect = Time.time;
                        if (secondSelect - firstSelect <= 1) {
                            // Camera.main.enabled = false;
                            // var camera = lastSelectedUnit.GetComponentInChildren<Camera>();
                        } else {

                        }
                        // Set Camera to child of gameObject
                    }
                    selectedObjects.Clear();
                    selectedObjects.Add(hitObject);
                    unitMovemet = hitObject.GetComponent<unitMovement>();
                    unitMovemet.setVisible();
                    currentlySelected += 1;
                    firstSelect = Time.time;
                    lastSelectedUnit = hitObject;
                } else {
                    selectedObjects.Clear();
                    lastSelectedUnit = null;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            selectSquareImage.gameObject.SetActive(false);
        }

        if (Input.GetMouseButton(0)) {
            if (!selectSquareImage.gameObject.activeInHierarchy) {
                selectSquareImage.gameObject.SetActive(true);
            }
            endPos = Input.mousePosition;

            Vector3 squareStart = Camera.main.WorldToScreenPoint(startPos);
            
            squareStart.z = 0f;

            Vector3 centre = (squareStart + endPos) / 2f;

            selectSquareImage.position = centre;

            float sizeX = Mathf.Abs(squareStart.x - endPos.x);
            float sizeY = Mathf.Abs(squareStart.y - endPos.y);

            selectSquareImage.sizeDelta = new Vector2(sizeX, sizeY);
            mousePos2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if (mousePos1 != mousePos2) {
                selectObjects();
            }
        }

        if (Input.GetMouseButtonDown(1) && hitObject != null) {
            if (selectedObjects.Count > 0) {
                for (int i = 0; i < selectedObjects.Count; i++) {
                    unitMovemet = selectedObjects[i].GetComponent<unitMovement>();
                    unitMovemet.findAction();
                }
            }
        }

        if (selectedObjects.Count != currentlySelected) {
            foreach (GameObject selectable in selectableObjects) {
                unitMovemet = selectable.GetComponent<unitMovement>();
                unitMovemet.setInvisible();
                currentlySelected = 0;
            }
            foreach (GameObject selected in selectedObjects) {
                unitMovemet = selected.GetComponent<unitMovement>();
                unitMovemet.setVisible();
                currentlySelected += 1;
            }
        }
    }

    private void selectObjects() {
        if (!Input.GetKey("left ctrl")) {
            selectedObjects.Clear();
        }

        Rect selectRect = new Rect(mousePos1.x, mousePos1.y, mousePos2.x - mousePos1.x, mousePos2.y - mousePos1.y);
        foreach (GameObject selectObject in selectableObjects) {
            if (selectObject != null) {
                if (selectRect.Contains(Camera.main.WorldToViewportPoint(selectObject.transform.position), true)) {
                    selectedObjects.Add(selectObject);
                }
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
