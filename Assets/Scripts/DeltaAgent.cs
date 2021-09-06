using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class DeltaAgent : Agent
{
    public GameObject ball;
    private Rigidbody rb;

    private GameObject slGameObject1;

    private GameObject slGameObject2;

    private GameObject slGameObject3;

    private EnvironmentParameters m_ResetParams;
    public override void Initialize()
    {
        rb = ball.GetComponent<Rigidbody>();
        slGameObject1 = GameObject.Find("SL1");
        slGameObject2 = GameObject.Find("SL2");
        slGameObject3 = GameObject.Find("SL3");
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        SetResetParameters();
    }

    private void SetResetParameters()
    {
        setBall();
        setDelta();
    }

    private void setDelta()
    {
        throw new System.NotImplementedException();
    }

    private void setBall()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // If agent hits the ball generate new ball
    // If agent fails to hit the ball ?
    // What should happen in each episode ?

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
        
        // TODO: set reward when racket hits the ball, ball hit the opponent's side of table
        if ((ball.transform.position.y - gameObject.transform.position.y) < -2f ||
            Mathf.Abs(ball.transform.position.x - gameObject.transform.position.x) > 3f ||
            Mathf.Abs(ball.transform.position.z - gameObject.transform.position.z) > 3f)
        {
            SetReward(-1f);
            EndEpisode();
        }
        else
        {
            SetReward(0.1f);
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        throw new NotImplementedException();
    }

    public override void OnEpisodeBegin()
    {
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(1, 0, 0), Random.Range(-10f, 10f));
        gameObject.transform.Rotate(new Vector3(0, 0, 1), Random.Range(-10f, 10f));
        rb.velocity = new Vector3(0f, 0f, 0f);
        ball.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 4f, Random.Range(-1.5f, 1.5f))
                                  + gameObject.transform.position;
        //Reset the parameters when the Agent is reset.
        SetResetParameters();
    }
}
