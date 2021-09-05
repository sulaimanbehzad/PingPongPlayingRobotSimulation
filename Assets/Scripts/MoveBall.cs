using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// using Random = System.Random;

public class MoveBall : MonoBehaviour
{
	// public TextAsset coordinates;
	public float forceCeof;

	private Rigidbody rb1;
	public bool moveTowardsRacket = true;
	private bool nextBall = false;

	private void OnEnable()
	{
		rb1 = gameObject.GetComponent<Rigidbody>();
	}

	void Start()
	{
	
	}
	void FixedUpdate()
	{
		if (!nextBall)
		{
			StartCoroutine(ThrowNextBall());
		}
	}
	private IEnumerator ThrowNextBall()
	{
		nextBall = true;
		// process pre-yield
		yield return new WaitForSeconds( 10f );
		// process post-yield
		Vector3 randVect = new Vector3(UnityEngine.Random.Range(-2f, -1.5f), UnityEngine.Random.Range(2f,2.13f),
			UnityEngine.Random.Range(0f, 0.87f));
		gameObject.transform.position = randVect;
		Vector3 direction = new Vector3(1, 0f, 0f);
		rb1.AddForce(direction * forceCeof, ForceMode.Impulse);
		// Debug.Log(rb1.name + ": | velocity: "+ rb1.angularVelocity);
		// rb1.velocity = new Vector3(0, 0, 0);
		// Instantiate(obj1, randVect, transform.rotation);
		nextBall = false;
	}
}
