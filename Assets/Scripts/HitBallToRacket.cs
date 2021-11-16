using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// using Random = System.Random;

public class HitBallToRacket : MonoBehaviour
{
    // public TextAsset coordinates;
    public float moveSpeed;

    public GameObject obj1;
    public GameObject obj2;

    public bool moveTowardsRacket = false;
    void Start()
    {
	
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("racket_A"))
        {
            moveTowardsRacket = true;
            // Debug.Log("moveTowardsRacket false");
            // wait 5 seconds before running the code below	
            yield return new WaitForSeconds(2);
            // // change obj1's speed
            moveSpeed = 5f;
            // Vector3 randVect = new Vector3(UnityEngine.Random.Range(-2f, -1.5f), UnityEngine.Random.Range(2f,2.13f),
            //     UnityEngine.Random.Range(0f, 0.87f));
            // obj1.transform.position = randVect;
            // moveTowardsRacket = true;
            // Debug.Log("moveTowardsRacket true");

            // new Vector3(-0.059f, 2.026f, 0.864f)	
        } 
        if (other.gameObject.CompareTag("court_B") || other.gameObject.CompareTag("floor") || other.gameObject.CompareTag("wall"))
        {
            moveTowardsRacket = false;
            // change obj1's speed
            // moveSpeed = Random.Range(2, 7);
            // Debug.Log("collided with table");
        }
    }

    private void Update()
    {
		
    }

    void FixedUpdate()
    {
        

        // Move in the direction of the direction vector every frame.
        if (moveTowardsRacket)
        {
                // Calculate direction vector.
            Vector3 dirction = obj1.transform.position - obj2.transform.position;

            // Normalize resultant vector to unit Vector.
            dirction = -dirction.normalized;
            obj1.transform.position += dirction * Time.deltaTime * moveSpeed;
        }
        // else
        // {
        // 	obj1.transform.position = new Vector3(-0.059f, 2.026f, 0.864f);
        // }
    }
}