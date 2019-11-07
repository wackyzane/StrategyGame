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
    public bool moveAttack = false;
    public string [,] ctrlArray;
    public List<GameObject> ctrl0;
    public List<GameObject> ctrl1;
    public List<GameObject> ctrl2;
    public List<GameObject> ctrl3;
    public List<GameObject> ctrl4;
    public List<GameObject> ctrl5;
    public List<GameObject> ctrl6;
    public List<GameObject> ctrl7;
    public List<GameObject> ctrl8;
    public List<GameObject> ctrl9;
    
    private float firstSelect = 0f;
    private float secondSelect = 0f;
    private GameObject hitObject;
    private GameObject lastSelectedUnit = null;
    private unitMovement unitMovement;
    private bool fTrue = false;
    private Vector3 mousePoint;
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
        string[,] ctrlArray = new string[10,10];
        // ctrlArray[0,0]
        ctrl0 = new List<GameObject>();
        ctrl1 = new List<GameObject>();
        ctrl2 = new List<GameObject>();
        ctrl3 = new List<GameObject>();
        ctrl4 = new List<GameObject>();
        ctrl5 = new List<GameObject>();
        ctrl6 = new List<GameObject>();
        ctrl7 = new List<GameObject>();
        ctrl8 = new List<GameObject>();
        ctrl9 = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(swordsmanSpawnHotkey)) {
            fTrue = true;
            hotKey = false;
            moveAttack = false;
        }

        if (Input.GetKey(crossbowmanSpawnHotkey)) {
            hotKey = true;
            fTrue = false;
            moveAttack = false;
        }

        if (Input.GetKey(KeyCode.Tab)) {
            moveAttack = true;
            fTrue = false;
            hotKey = false;
        }

        // Make this more condensed with a for (int i = 0; i < 10; i++)
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                ctrl0.Clear();
                foreach (GameObject obj in selectedObjects) {
                    ctrl0.Add(obj);
                }
            } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
                ctrl1.Clear();
                foreach (GameObject obj in selectedObjects) {
                    ctrl1.Add(obj);
                }
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                ctrl2.Clear();
                foreach (GameObject obj in selectedObjects) {
                    ctrl2.Add(obj);
                }
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                ctrl3.Clear();
                foreach (GameObject obj in selectedObjects) {
                    ctrl3.Add(obj);
                }
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                ctrl4.Clear();
                foreach (GameObject obj in selectedObjects) {
                    ctrl4.Add(obj);
                }
            } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
                ctrl5.Clear();
                foreach (GameObject obj in selectedObjects) {
                    ctrl5.Add(obj);
                }
            } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
                ctrl6.Clear();
                foreach (GameObject obj in selectedObjects) {
                    ctrl6.Add(obj);
                }
            } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
                ctrl7.Clear();
                foreach (GameObject obj in selectedObjects) {
                    ctrl7.Add(obj);
                }
            } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
                ctrl8.Clear();
                foreach (GameObject obj in selectedObjects) {
                    ctrl8.Add(obj);
                }
            } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
                ctrl9.Clear();
                foreach (GameObject obj in selectedObjects) {
                    ctrl9.Add(obj);
                }
            }
        } else {
            // for (int i = 0; i < 10; i++) {
            //     string alpha = "Alpha" + i.ToString();;
            //     if (Input.GetKey(KeyCode.Alpha + i)) {
            //         selectedObjects.Clear();
            //         foreach (GameObject obj in ("ctrl" + i.ToString())) {
            //             selectedObjects.Add(obj);
            //         }
            //     }
            // }
            if (Input.GetKey(KeyCode.Alpha0)) {
                selectedObjects.Clear();
                foreach (GameObject obj in ctrl0) {
                    selectedObjects.Add(obj);
                }
            }
            if (Input.GetKey(KeyCode.Alpha1)) {
                selectedObjects.Clear();
                foreach (GameObject obj in ctrl1) {
                    selectedObjects.Add(obj);
                }
            }
            if (Input.GetKey(KeyCode.Alpha2)) {
                selectedObjects.Clear();
                foreach (GameObject obj in ctrl2) {
                    selectedObjects.Add(obj);
                }
            }
            if (Input.GetKey(KeyCode.Alpha3)) {
                selectedObjects.Clear();
                foreach (GameObject obj in ctrl3) {
                    selectedObjects.Add(obj);
                }
            }
            if (Input.GetKey(KeyCode.Alpha4)) {
                selectedObjects.Clear();
                foreach (GameObject obj in ctrl4) {
                    selectedObjects.Add(obj);
                }
            }
            if (Input.GetKey(KeyCode.Alpha5)) {
                selectedObjects.Clear();
                foreach (GameObject obj in ctrl5) {
                    selectedObjects.Add(obj);
                }
            }
            if (Input.GetKey(KeyCode.Alpha6)) {
                selectedObjects.Clear();
                foreach (GameObject obj in ctrl6) {
                    selectedObjects.Add(obj);
                }
            }
            if (Input.GetKey(KeyCode.Alpha7)) {
                selectedObjects.Clear();
                foreach (GameObject obj in ctrl7) {
                    selectedObjects.Add(obj);
                }
            }
            if (Input.GetKey(KeyCode.Alpha8)) {
                selectedObjects.Clear();
                foreach (GameObject obj in ctrl8) {
                    selectedObjects.Add(obj);
                }
            }
            if (Input.GetKey(KeyCode.Alpha9)) {
                selectedObjects.Clear();
                foreach (GameObject obj in ctrl9) {
                    selectedObjects.Add(obj);
                }
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            mousePoint = mouseMousePoint();
            startPos = mousePoint;
            startPos.y -= .5f;
            mousePos1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if (fTrue) {
                Instantiate(SwordsmanPrefab, mousePoint, Quaternion.identity);
                fTrue = false;
            } else if (hotKey) {
                Instantiate(CrossbowmanPrefab, mousePoint, Quaternion.identity);
                hotKey = false;
            } else if (moveAttack) {
                moveAttack = false;
            }

            hitObject = isObjectSelected();

            if (Input.GetKey(KeyCode.LeftControl)) {
                Debug.Log(Screen.height);
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
                    if (lastSelectedUnit == hitObject) {
                        secondSelect = Time.time;
                        if (secondSelect - firstSelect <= 1) {
                            // bool firstObject = true;
                            foreach (GameObject selectable in selectableObjects) {
                                if (selectable.name == hitObject.name) {
                                    Vector3 screenPoint = Camera.main.WorldToViewportPoint(selectable.transform.position);
                                    bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
                                    if (onScreen) {
                                        selectedObjects.Add(selectable);
                                    }
                                }
                            }
                        }
                    } else {
                        selectedObjects.Clear();
                        selectedObjects.Add(hitObject);
                        unitMovement = hitObject.GetComponent<unitMovement>();
                        unitMovement.setVisible();
                        currentlySelected += 1;
                    }
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

        if (Input.GetMouseButtonDown(1)) {
            if (hitObject != null) {
                if (selectedObjects.Count > 0) {
                    for (int i = 0; i < selectedObjects.Count; i++) {
                        unitMovement = selectedObjects[i].GetComponent<unitMovement>();
                        unitMovement.findAction();
                    }
                }
            }
        }

        if (selectedObjects.Count != currentlySelected) {
            foreach (GameObject selectable in selectableObjects) {
                unitMovement = selectable.GetComponent<unitMovement>();
                unitMovement.setInvisible();
                currentlySelected = 0;
            }
            foreach (GameObject selected in selectedObjects) {
                unitMovement = selected.GetComponent<unitMovement>();
                unitMovement.setVisible();
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
    
    public Vector3 mouseMousePoint() {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) {
            Vector3 mousePoint = hit.point;
            mousePoint.y += .5f;
            return mousePoint;
        }
        return mousePoint;
    }
}
