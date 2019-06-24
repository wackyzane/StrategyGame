using UnityEngine;




public class cameraMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float forwardForce = 200f;
    public float sidewaysForce = 200f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            rb.AddForce(0, 0, forwardForce * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("s"))
        {
            rb.AddForce(0, 0, -forwardForce * Time.deltaTime);
        }
        while (rb.velocity != 30)
        {
            Debug.Log(rb.velocity);
        }
    }
}