using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public int rotationSpeed;
    public GameObject go;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    { 
        rb = go.GetComponent<Rigidbody>();
        Debug.Log(rb.name);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // rb.transform.Rotate(new Vector3(10 * rotationSpeed * Time.deltaTime, 0, 0));
        rb.AddTorque(new Vector3(0,0, 10), ForceMode.Force);
    }
}
