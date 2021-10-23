using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class RacketAgent : Agent
{
    public GameObject ball;
    private Rigidbody _ballRb;

    /*
     * for accessing a data set on the Academy called �EnvironmentParameters.� 
     * We�ll use these to set and get the default size of the ball.
     */
    EnvironmentParameters defaultParameters;

    public Transform table;

    public Transform racket;
    public bool use_torque;
    public float ball_force_coef;
    private static int cnt_eps = 0;
    private static int cnt_frame = 0;
    private float max_X = 1.83f;
    private float min_X = 0.93f;
    private float max_Y = 1.2f;
    private float min_Y = 0.884f;
    private float max_Z = 0.766f;
    private float min_Z = -0.766f;
    

    public int get_episode(){
        return cnt_eps;
    }

    public int get_frame(){
        return cnt_frame;
    }

    public override void Initialize()
    {
        Debug.Log("delta time: " + Time.fixedDeltaTime);
        _ballRb = ball.GetComponent<Rigidbody>();
        defaultParameters = Academy.Instance.EnvironmentParameters;
        ResetScene();
    }

    //this function resets the position and velocity of the ball, the rotation of the agent, 
    public override void OnEpisodeBegin()
    {
        racket.position = new Vector3(1.35f,0.94f,0f);
        racket.eulerAngles = new Vector3(0f,0f,0f);
   
        // _ballRb.velocity = new Vector3(0f, 0f, 0f);
        // ball.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 4f, Random.Range(-1.5f, 1.5f)) + gameObject.transform.position;
        Vector3 randVect = new Vector3(UnityEngine.Random.Range(-1.5f, -1.2f), UnityEngine.Random.Range(1.1f,1.3f), UnityEngine.Random.Range(-0.6f, 0.6f));
        // randVect += table.transform.position;
        // Debug.Log(randVect);
        ball.transform.position = randVect;
        Vector3 direction = new Vector3(1, -0.1f, 0f);
        ball_force_coef = Random.Range(7, 10);
        _ballRb.velocity = new Vector3(ball_force_coef, 0, 0); 
        // _ballRb.AddForce(direction * ball_force_coef, ForceMode.Impulse);
        cnt_eps++;
        // Debug.Log("Episode number: " + cnt_eps.ToString());
        ResetScene();
          
    }


    /*�Initialize� functions similar to the �Start� method except this is called a little bit earlier than �Start�.
    // Start is called before the first frame update
    void Start()
    {
    }
    */


    /*on the Behaviour Parameter component at the �Vector Observation� the �Space Size.�
    * should be set accourding to CollectObservations function
    * for example space size should be 8 
    * if you get 2 3-valued vectors of vlocity and position and 2 1-valued vectors of specific axis rotation
    */

    /* The �StackedVectors� slider allows you to set 
     * how many vectors must be observed before it is sent to the Academy.
     */
    public override void CollectObservations(VectorSensor sensor)
    {

        // get ball position and rotation values
        // sensor.AddObservation(ball.transform.rotation.x);
        // sensor.AddObservation(ball.transform.rotation.y);
        // sensor.AddObservation(ball.transform.rotation.z);
        // Vector3 refVec = new Vector3;
        // refVec = table.transform.position + ball.transform.position;
        sensor.AddObservation(changeScale(ball.transform.position.x, -2f, 2f, 0f, 1f));
        sensor.AddObservation(changeScale(ball.transform.position.y, -2f, 2f, 0f, 1f));
        sensor.AddObservation(changeScale(ball.transform.position.z, -2f, 2f, 0f, 1f));
        // Debug.Log(ball.transform.position);
        sensor.AddObservation(changeScale(racket.position.x, -2f, 2f, 0f, 1f));
        sensor.AddObservation(changeScale(racket.position.y, -2f, 2f, 0f, 1f));
        sensor.AddObservation(changeScale(racket.position.z, -2f, 2f, 0f, 1f));

        /*
         * sensor.AddObservation(ballRb.velocity);
         * sensor.AddObservation(ball.transform.position);
         */



    }

    public float changeScale(float v,float min, float max, float newMin, float newMax)
    {
        float vP = ((((v - min) / (max - min)) * (newMax - newMin)) + newMin);
        return vP;
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        var i = -1;
        var X = racket.position.x + Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f) / 20f;
        var Y = racket.position.y + Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f) / 20f;
        var Z = racket.position.z + Mathf.Clamp(actions.ContinuousActions[2], -1f, 1f) / 20f;
        Debug.Log(Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f));
        if (X > min_X && X < max_X) {
            racket.position = new Vector3(X,racket.position.y, racket.position.z);
        }
        if (Y > min_Y && Y < max_Y) {
            racket.position = new Vector3(racket.position.x ,Y, racket.position.z);

        }
        if (Z > min_Z && Z < max_Z) {
            racket.position = new Vector3(racket.position.x, racket.position.y, Z);

        }
        // racket.position = Vector3.MoveTowards(racket.position, new Vector3(X,Y,Z), Time.fixedDeltaTime * 10);
        // Debug.Log(actions.ContinuousActions[0] + "    "+ actions.ContinuousActions[1]+ "    " +actions.ContinuousActions[2]);
        // Debug.Log(X + "    "+ Y+ "    " + Z);
        

    }

    void ResetScene()
    {
        _ballRb.mass = defaultParameters.GetWithDefault("mass", 0.0027f);
        var scale = defaultParameters.GetWithDefault("scale", 0.04f);
        ball.transform.localScale = new Vector3(scale, scale, scale);
        // Debug.Log("end reset scene");
    }

    private void FixedUpdate()
    {
        cnt_frame++;
        // Debug.Log("Number of frames: " + cnt_frame.ToString());
    }
}
