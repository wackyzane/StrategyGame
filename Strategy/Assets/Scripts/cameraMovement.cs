using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce;
    public float sidewaysForce;

    void FixedUpdate()
    {
        if ((Input.GetKey("w") && rb.velocity.z < 30) || (Input.GetKey("up") && rb.velocity.z < 30))
        {
            rb.AddForce(0, 0, forwardForce * Time.deltaTime);
        } else if (rb.velocity.z > 1) {
            rb.AddForce(0, 0, -forwardForce * Time.deltaTime);
        } else if (rb.velocity.z <= 1 && rb.velocity.z > 0) {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        }
        if ((Input.GetKey("a") && rb.velocity.x > -30) || (Input.GetKey("left") && rb.velocity.x > -30))
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0);
        } else if (rb.velocity.x < -1) {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0);
        } else if (rb.velocity.x >= -1 && rb.velocity.x < 0) {
            rb.velocity = new  Vector3(0, 0, rb.velocity.z);
        }
        if ((Input.GetKey("s") && rb.velocity.z > -30) || (Input.GetKey("down") && rb.velocity.z > -30))
        {
            rb.AddForce(0, 0, -forwardForce * Time.deltaTime);
        } else if (rb.velocity.z < -1) {
            rb.AddForce(0, 0, forwardForce * Time.deltaTime);
        } else if (rb.velocity.z >= -1 && rb.velocity.z < 0) {
            rb.velocity = new  Vector3(rb.velocity.x, 0, 0);
        }
        if ((Input.GetKey("d") && rb.velocity.x < 30) || (Input.GetKey("right") && rb.velocity.x < 30))
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0);
        } else if (rb.velocity.x > 1) {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0);
        } else if (rb.velocity.x <= 1 && rb.velocity.x > 0) {
            rb.velocity = new Vector3(0, 0, rb.velocity.z);
        }
    }
    void Update() {
        Debug.Log(rb.velocity);
    }
}