using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
	// public TextAsset coordinates;
	public int moveSpeed;

	public GameObject obj1;
	public GameObject obj2;

	public bool moveTowardsRacket = true;
	void Start()
	{
	
	}

	private IEnumerator OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("racket"))
		{
			moveTowardsRacket = false;
			Debug.Log("moveTowardsRacket false");
			yield return new WaitForSeconds(5);
			Vector3 randVect = new Vector3(UnityEngine.Random.Range(-1f, 0f), UnityEngine.Random.Range(2f, 3f),
				UnityEngine.Random.Range(0f, 0.87f));
			obj1.transform.position = randVect;
			moveTowardsRacket = true;
			Debug.Log("moveTowardsRacket true");

			// new Vector3(-0.059f, 2.026f, 0.864f)	
		}
	}

	void FixedUpdate()
	{
		// Calculate direction vector.
		Vector3 dirction = obj1.transform.position - obj2.transform.position;

		// Normalize resultant vector to unit Vector.
		dirction = -dirction.normalized;

		// Move in the direction of the direction vector every frame.
		if (moveTowardsRacket)
		{
			obj1.transform.position += dirction * Time.deltaTime * moveSpeed;
		}
		// else
		// {
		// 	obj1.transform.position = new Vector3(-0.059f, 2.026f, 0.864f);
		// }
	}
}
