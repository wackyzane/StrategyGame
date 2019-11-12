using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class formFormation : MonoBehaviour {
    private mouseClick mouseClick;
    private unitMovement unitMovement;
    private Vector3 firstMovePoint;
    private Coroutine moveCoroutine = null;
    private float oddEven = 0f;
    private bool reset = false;

    private void Formation(List<Vector3> movePoint, float speed) {
        float formationSize = Mathf.Sqrt(mouseClick.selectedObjects.Count);
        Debug.Log(formationSize);
        // Use Mathf.Round(formationSize); for the bottom row to be less
        // Check if formationSize is even or odd
        if (formationSize <= 0f) {
            if (formationSize % 2 == 0) {
                Debug.Log("Even");
                oddEven = 0f;
            } else {
                Debug.Log("Odd");
                oddEven = 1f;
            }
        }
        
        // If odd, place first unit on movePoint
        if (oddEven == 1) {
            firstMovePoint = movePoint[0];
            Vector3 origin = firstMovePoint;
            // Subtract 1 from formation size
            // Divide formation size by 2, This is how many units will be on each side of the middle Unit
            float formationSideSize = (formationSize - 1f) / 2f;
            // Counter for how many units are on a side
            float formationCurrentSide = 0f;
            float formationLineTotal = 0f;
            // Repeat until all units have their movement
            for (int i = 0; i < mouseClick.selectedObjects.Count; i++) {
                // Place unit on new movePoint
                moveCoroutine = StartCoroutine(unitMovement.moveOverSpeed(mouseClick.selectedObjects[i], movePoint[0], speed));
                // Check to see if the formation has the correct amount of units on one side.
                if (formationCurrentSide <= formationSideSize) {
                    // Add 2 to movePoint.x
                    firstMovePoint.x += 2;
                    // Change First movePoint to new movePoint
                    movePoint[0] = firstMovePoint;
                    formationCurrentSide += 1f;
                    formationLineTotal += 1f;
                    if (formationCurrentSide >= formationSideSize) {
                        reset = true;
                    }
                } else {
                    // Reset movePoint.x and do again but with -2 for other side of formation
                    firstMovePoint.x -= 2;
                    // Change First movePoint to new movePoint
                    movePoint[0] = firstMovePoint;
                    // Add unit counter for side
                    formationCurrentSide += 1f;
                    // Reset counter and origin if one side is done
                    if (reset) {
                        firstMovePoint = origin;
                        formationCurrentSide = 0f;
                        formationLineTotal += 1f;
                        reset = false;
                    }
                }
                // Add 2 to movePoint.y and do it all again
                if (formationLineTotal >= formationSize) {
                    origin.y += 2f;
                    firstMovePoint = origin;
                    formationCurrentSide = 0f;
                    formationLineTotal = 0f;
                    firstMovePoint.y += 2f;
                }
            }
        } else {
            Debug.Log("Dont have Code for even. But it should be very similar");
        }
    }
}
