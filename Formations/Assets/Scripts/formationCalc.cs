using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class formationCalc : MonoBehaviour {

    private bool odd;
    private float formationSqr = 0f;
    private float formationSide = 0f;
    private Vector3 leaderXRotation;
    private float unitPosition;
    private Vector3 leaderPos;
    private Vector3 changingRow;
    private Vector3 changingColumn;
    private List<Vector3> formationPositions;
    private unitScript firstUnit;

    private void Awake() {
        formationPositions = new List<Vector3>();
    }

    public Vector3 formation(GameObject leader, int unitNumber) {
        formationPositions.Clear();
        firstUnit = leader.GetComponent<unitScript>();
        formationSqr = Mathf.Sqrt(firstUnit.currentGroup.Count + 1);
        leaderPos = leader.transform.position;
        changingColumn = leaderPos;
        changingRow = leaderPos;
        leaderXRotation = leader.transform.eulerAngles;
        if ((firstUnit.currentGroup.Count + 1)  % 2 == 1) {
            odd = true;
        } else {
            odd = false;
        }
        if (odd) {
            formationSide = (formationSqr - 1) / 2;
            // Y axis for Rows
            for (int i = 0; i < formationSqr; i++) {
                // Set units just behind leader
                if (changingColumn != leaderPos) {
                    formationPositions.Add(changingColumn);
                }
                // X Axis for Columns, Right Side
                for (int j = 0; j < formationSide; j++) {
                    unitPosition = leaderXRotation.x + 90f;
                    Debug.Log(unitPosition);
                    changingColumn += Vector3.right * 2;
                    formationPositions.Add(changingColumn);
                }
                changingColumn = changingRow;
                // X Axis for Columns, Right Side
                for (int j = 0; j < formationSide; j++) {
                    changingColumn -= Vector3.right * 2;
                    formationPositions.Add(changingColumn);
                }
                changingRow -= Vector3.forward * 2;
                changingColumn = changingRow;
            }
        }
        return formationPositions[unitNumber];
    }

}
