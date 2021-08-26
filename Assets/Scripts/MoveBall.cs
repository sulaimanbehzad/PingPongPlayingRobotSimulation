using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
	public float speed;
	private Rigidbody rb;
    public bool useTorque = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	public void Control()
     {
		float moveX = Input.GetAxis("Horizontal") * speed;
     	float moveY = Input.GetAxis("Vertical") * speed;
		moveX *= Time.deltaTime;
		moveY *= Time.deltaTime;
         if (useTorque)
         {
             rb.AddTorque(new Vector3(0f, 0f, moveX));	
			//transform.Translate(speed,0,0);
			//transform.Rotate(0, 0, speed);
			 //rb.velocity = new Vector3(speed, 0, 0);
         }
     }
	
	void OnTriggerEnter(Collider other)
 	{
     if(other.tag == "racket")
     {
        useTorque = false;	
		speed = 0f;
     }
 	}
    // Update is called once per frame
    void Update()
    {
     	Control();
    }
}
