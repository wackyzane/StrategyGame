using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class formFormation : MonoBehaviour {
    
    private mouseClick mouseClick;
    private unitMovement unitMovement;
    private Vector3 changingMovePoint;
    private Vector3 origin;
    private float oddEven = 0f;
    private bool reset = false;

    private void Awake() {
        GameObject mouseManager = GameObject.Find("mouseManager");
        mouseClick = mouseManager.GetComponent<mouseClick>();
    }

    public void formation(List<Vector3> movePoint, List<bool> moveAttack, List<bool> hitEnemy, float speed) {
        float formationSize = Mathf.Sqrt(mouseClick.selectedObjects.Count);
        // Use Mathf.Round(formationSize); for the bottom row to be less
        // Check if formationSize is even or odd
        if (mouseClick.selectedObjects.Count >= 0f) {
            if (mouseClick.selectedObjects.Count % 2 == 0) {
                oddEven = 0f;
            } else {
                oddEven = 1f;
            }
        }
        
        // If odd, place first unit on movePoint
        if (oddEven == 1) {
            changingMovePoint = movePoint[0];
            Vector3 origin = changingMovePoint;
            // Subtract 1 from formation size
            // Divide formation size by 2, This is how many units will be on each side of the middle Unit
            float formationSideSize = (formationSize - 1f) / 2;
            Debug.Log(formationSideSize);
            // Counter for how many units are on a side
            float formationCurrentSide = 0f;
            float formationLineTotal = 0f;

            unitMovement = mouseClick.selectedObjects[0].GetComponent<unitMovement>();
            unitMovement.startMove(mouseClick.selectedObjects[0], changingMovePoint, moveAttack[0], hitEnemy[0], speed);
            for (int i = 1; i < mouseClick.selectedObjects.Count - 1; i++) {
                unitMovement = mouseClick.selectedObjects[i].GetComponent<unitMovement>();
                unitMovement.startStayFormation(mouseClick.selectedObjects[0]);
            }
            // Repeat until all units have their movement
            // for (int i = 0; i < mouseClick.selectedObjects.Count - 1; i++) {
            //     // Place unit on new movePoint
            //     // Cant find selectedObjects above 0

            //     // Not giving movePoints to other units
            //     unitMovement = mouseClick.selectedObjects[i].GetComponent<unitMovement>();
            //     unitMovement.startMove(mouseClick.selectedObjects[i], changingMovePoint, moveAttack[0], hitEnemy[0], speed);
                
            //     // Check to see if the formation has the correct amount of units on one side.
            //     if (formationCurrentSide <= formationSideSize) {
            //         // Add 2 to movePoint.x
            //         changingMovePoint.x += 10f;
            //         formationCurrentSide += 1f;
            //         formationLineTotal += 1f;
            //         if (formationCurrentSide >= formationSideSize) {
            //             reset = true;
            //         }
            //     } else {
            //         // Reset counter and origin if one side is done
            //         if (reset) {
            //             changingMovePoint = origin;
            //             formationCurrentSide = 0f;
            //             formationLineTotal += 1f;
            //             reset = false;
            //         }
            //         // Reset movePoint.x and do again but with -2 for other side of formation
            //         changingMovePoint.x -= 10;
            //         // Add unit counter for side
            //         formationCurrentSide += 1f;
            //     }
            //     // Add 2 to movePoint.y and do it all again
            //     if (formationLineTotal >= formationSize) {
            //         if (formationSize > 1) {
            //             Debug.Log("Go up a line");
            //             origin.z += 10f;
            //             changingMovePoint = origin;
            //             formationCurrentSide = 0f;
            //             formationLineTotal = 0f;
            //             changingMovePoint.z += 2f;
            //         }
            //     }
            // }
        } else {
            Debug.Log("Dont have Code for even. But it should be very similar");
        }

        // float formationSize = Mathf.Sqrt(mouseClick.selectedObjects.Count);
        // if (mouseClick.selectedObjects.Count % 2 == 1) {
        //     oddEven = 1f;
        // } else {
        //     oddEven = 0f;
        // }

        // if (oddEven == 1f) {
        //     changingMovePoint = movePoint[0];
        //     origin = changingMovePoint;
        //     float formationSideSize = (formationSize - 1f) / 2f;
        //     Debug.Log(formationSideSize);
        //     // float formationCurrentSide = 0f;
        //     // float formationLineTotal = 0f;
        // }
    }
}
