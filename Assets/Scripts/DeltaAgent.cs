using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class DeltaAgent : Agent
{
    public GameObject ball;
    private Rigidbody rb;

    private GameObject slGameObject1;

    private GameObject slGameObject2;

    private GameObject slGameObject3;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slGameObject1 = GameObject.Find("SL1");
        slGameObject2 = GameObject.Find("SL2");
        slGameObject3 = GameObject.Find("SL3");
    }
    // If agent hits the ball generate new ball
    // If agent fails to hit the ball ?
    // What should happen in each episode ?
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // get ball position and rotation values
        sensor.AddObservation(ball.transform.rotation.x);
        sensor.AddObservation(ball.transform.rotation.y);
        sensor.AddObservation(ball.transform.rotation.z);
        
        sensor.AddObservation(ball.transform.position.x);
        sensor.AddObservation(ball.transform.position.y);
        sensor.AddObservation(ball.transform.position.z);
        
        // get robot servo motors angles
        sensor.AddObservation(slGameObject1.transform.rotation.z);
        sensor.AddObservation(slGameObject2.transform.rotation.z);
        sensor.AddObservation(slGameObject3.transform.rotation.z);

        

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionSl1 = actions.ContinuousActions[0];
        var actionSl2 = actions.ContinuousActions[1];
        var actionSl3 = actions.ContinuousActions[2];
        
        // move servo motors using transform and angles in actions received
        slGameObject1.transform.eulerAngles = new Vector3(0,0,actionSl1);
        slGameObject2.transform.eulerAngles = new Vector3(0,0,actionSl2);
        slGameObject3.transform.eulerAngles = new Vector3(0,0,actionSl3);
        
        // TODO: set reward
        
    }
}
