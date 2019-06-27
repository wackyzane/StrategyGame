using UnityEngine;

public class unitSpawn : MonoBehaviour
{
    private bool fTrue = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("f")) {
            fTrue = true;
        }
}
