using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;

    void FixedUpdate()
    {
        if (Input.GetKey("w") || Input.GetKey("up")) {
            if (rb.transform.position.z <= 200) {
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
            }
        } else if (Input.mousePosition.y >= Screen.height * .95 && rb.transform.position.z <= 200) {
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey("a") || Input.GetKey("left")) {
            if (rb.transform.position.x >= -200) {
                transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
            }
        } else if (Input.mousePosition.x <= Screen.width * .05 && rb.transform.position.x >= -200) {
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey("s") || Input.GetKey("down")) {
            if (rb.transform.position.z >= -200) {
                transform.Translate(-Vector3.forward * Time.deltaTime * speed, Space.World);
            }
        } else if (Input.mousePosition.y <= Screen.height * .05 &&rb.transform.position.z >= -200) {
            transform.Translate(-Vector3.forward * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey("d") || Input.GetKey("right")) {
            if (rb.transform.position.x <= 200) {
                transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);
            }
        } else if (Input.mousePosition.x >= Screen.width * .95 && rb.transform.position.x <= 200) {
            transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey("e") && rb.transform.position.y <= 50) {
            transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f && rb.transform.position.y <= 50) {
            transform.Translate((Vector3.up * 3) * Time.deltaTime * (speed * 3), Space.World);
        }

        if (Input.GetKey("q") && rb.transform.position.y >= 10) {
            transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
        } else if (Input.GetAxis("Mouse ScrollWheel") > 0f && rb.transform.position.y >= 10) {
            transform.Translate((Vector3.down * 3) * Time.deltaTime * (speed * 3), Space.World);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            speed = 15f;
        } 
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            speed = 30f;
        }
    }
}