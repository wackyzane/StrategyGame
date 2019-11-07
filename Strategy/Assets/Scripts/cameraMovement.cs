using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public float speedLimit;
    public float otherSpeed;

    void FixedUpdate()
    {
        if ((Input.GetKey("w") && rb.velocity.z < speedLimit) || (Input.GetKey("up") && rb.velocity.z < speedLimit))
        {
            rb.AddForce(0, 0, force * Time.deltaTime);
        } else if (rb.velocity.z > .1) {
            rb.AddForce(0, 0, -force * Time.deltaTime);
        } else if (rb.velocity.z <= .1 && rb.velocity.z > 0) {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        }

        if ((Input.GetKey("a") && rb.velocity.x > -speedLimit) || (Input.GetKey("left") && rb.velocity.x > -speedLimit))
        {
            rb.AddForce(-force * Time.deltaTime, 0, 0);
        } else if (rb.velocity.x < -.1) {
            rb.AddForce(force * Time.deltaTime, 0, 0);
        } else if (rb.velocity.x >= -.1 && rb.velocity.x < 0) {
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }

        if ((Input.GetKey("s") && rb.velocity.z > -speedLimit) || (Input.GetKey("down") && rb.velocity.z > -speedLimit))
        {
            rb.AddForce(0, 0, -force * Time.deltaTime);
        } else if (rb.velocity.z < -.1) {
            rb.AddForce(0, 0, force * Time.deltaTime);
        } else if (rb.velocity.z >= -.1 && rb.velocity.z < 0) {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        }

        if ((Input.GetKey("d") && rb.velocity.x < speedLimit) || (Input.GetKey("right") && rb.velocity.x < speedLimit))
        {
            rb.AddForce(force * Time.deltaTime, 0, 0);
        } else if (rb.velocity.x > .1) {
            rb.AddForce(-force * Time.deltaTime, 0, 0);
        } else if (rb.velocity.x <= .1 && rb.velocity.x > 0) {
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }

        if ((Input.GetKey("e") && rb.velocity.y < otherSpeed)) {
            rb.AddForce(0, force * Time.deltaTime, 0);
        } else if (rb.velocity.y > .1) {
            rb.AddForce(0, -force * Time.deltaTime, 0);
        } else if (rb.velocity.y <= .1 && rb.velocity.y > 0) {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }

        if ((Input.GetKey("q") && rb.velocity.y > -otherSpeed)) {
            rb.AddForce(0, -force * Time.deltaTime, 0);
        } else if (rb.velocity.y < -.1) {
            rb.AddForce(0, force * Time.deltaTime, 0);
        } else if (rb.velocity.y >= -.1 && rb.velocity.y < 0) {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }
    void Update() {
        if (Input.mousePosition.y >= Screen.height * .95) {
            transform.Translate(Vector3.forward * Time.deltaTime * 30, Space.World);
        } else if (Input.mousePosition.y <= Screen.height * .05) {
            transform.Translate(-Vector3.forward * Time.deltaTime * 30, Space.World);
        } else if (Input.mousePosition.x >= Screen.width * .95) {
            transform.Translate(Vector3.right * Time.deltaTime * 30, Space.World);
        } else if (Input.mousePosition.x <= Screen.width * .05) {
            transform.Translate(Vector3.left * Time.deltaTime * 30, Space.World);
        }
    }
}