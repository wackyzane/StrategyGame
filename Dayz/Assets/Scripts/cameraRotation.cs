using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour {

    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private Transform playerBody;

    private float xAxisClamp = 0f;

    void Awake() {
        lockCursor();
    }

    // Update is called once per frame
    void Update() {
        CameraRotation();
    }

    private void lockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraRotation() {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if (xAxisClamp > 90f) {
            xAxisClamp = 90f;
            mouseY = 0f;
            clampXAxisRotationToValue(270f);
        } else if (xAxisClamp < -90f) {
            xAxisClamp = -90f;
            mouseY = 0f;
            clampXAxisRotationToValue(90f);
        }
        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void clampXAxisRotationToValue(float value) {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

    private float cameraMove(float moveAmount, float amountOfTime) {
        Vector3 eulerRotation = transform.eulerAngles;
        float angleMovement = moveAmount / time;
        eulerRotation.x -= angleMovement;
        transform.eulerAngles = eulerRotation;
        remainingRecoil -= angleMovement;
        time -= Time.deltaTime;
        return time;
    }

    public void gunRecoil(float recoil) {
        StopCoroutine(recoilOverTime(recoil));
        StartCoroutine(recoilOverTime(recoil));
    }

    public void startGunSway() {
        float sway = Random.Range(-1f, 1f);
        StopCoroutine(gunSway(sway));
        StartCoroutine(gunSway(sway));
    }

    private IEnumerator gunSway(float sway) {
        // Pick random camera rotation and slowly turn toward it
        Vector3 eulerRotation = transform.eulerAngles;
        Vector3 startAngles = transform.eulerAngles;
        eulerRotation.x += sway;
        transform.eulerAngles = eulerRotation;
        // Move back to where they were originally aiming

        // transform.eulerAngles = startAngles;
        yield return null;
    }

    private IEnumerator recoilOverTime(float recoil) {
        float remainingRecoil = recoil;
        float time = 2.5f;
        do {
            Vector3 eulerRotation = transform.eulerAngles;
            float angleMovement = remainingRecoil / time;
            eulerRotation.x -= angleMovement;
            transform.eulerAngles = eulerRotation;
            remainingRecoil -= angleMovement;
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } while (time >= 1);
        yield return null;
    }
}
