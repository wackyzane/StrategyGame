using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string veritcalInputName;

    [SerializeField] private float walkSpeed, runSpeed;
    [SerializeField] private float runBuildUpSpeed;
    [SerializeField] private KeyCode runKey;
    private float movementSpeed;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    private CharacterController charController;
    private bool isJumping;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    void Awake() {
        charController = GetComponent<CharacterController>();
    }

    void Update() {
        playerMove();
    }

    private void playerMove() {
        float horizInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(veritcalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1f) * movementSpeed);

        if ((vertInput != 0 || horizInput != 0) && onSlope()) {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
        }

        setMovementSpeed();
        jumpInput();
    }

    private void setMovementSpeed() {
        if (Input.GetKey(runKey)) {
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        } else {
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
        }
    }

    private bool onSlope() {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }

    private void jumpInput() {
        if (Input.GetKeyDown(jumpKey) && !isJumping) {
            isJumping = true;
            StartCoroutine(jumpEvent());
        }
    }

    private IEnumerator jumpEvent() {
        charController.slopeLimit = 90f;
        float timeInAir = 0.0f;

        do {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);
        charController.slopeLimit = 45f;
        isJumping = false;
    }
}
