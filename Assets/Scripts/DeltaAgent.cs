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
    private Rigidbody _ballRb;

    /*
     * for accessing a data set on the Academy called �EnvironmentParameters.� 
     * We�ll use these to set and get the default size of the ball.
     */
    EnvironmentParameters defaultParameters;

    public GameObject sl1;
    public GameObject sl2;
    public GameObject sl3;

    private Rigidbody sl1_rb;
    private Rigidbody sl2_rb;
    private Rigidbody sl3_rb;

    private Transform sl1_tr;
    private Transform sl2_tr;
    private Transform sl3_tr;
    public Transform racket;
    public bool use_torque;
    public float ball_force_coef;
    private int i_cnt = 0;
    public override void Initialize()
    {
        _ballRb = ball.GetComponent<Rigidbody>();
        sl1_rb = sl1.GetComponent<Rigidbody>();
        sl2_rb = sl2.GetComponent<Rigidbody>();
        sl3_rb = sl3.GetComponent<Rigidbody>();

        sl1_tr = sl1.GetComponent<Transform>();
        sl2_tr = sl2.GetComponent<Transform>();
        sl3_tr = sl3.GetComponent<Transform>();

        defaultParameters = Academy.Instance.EnvironmentParameters;
        ResetScene();
    }

    //this function resets the position and velocity of the ball, the rotation of the agent, 
    public override void OnEpisodeBegin()
    {
        // this part was copied from another project
        // slGameObject1.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        sl1_tr.eulerAngles = new Vector3(0, 180f, 0);

        // slGameObject2.transform.rotation = new Quaternion(0f, 60f, 0f, 0f);
        sl2_tr.eulerAngles = new Vector3(0, 60f, 0);

        // slGameObject3.transform.rotation = new Quaternion(0f, -62f, 0f, 0f);
        sl3_tr.eulerAngles = new Vector3(0, -62, 0f);
   
        // _ballRb.velocity = new Vector3(0f, 0f, 0f);
        // ball.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 4f, Random.Range(-1.5f, 1.5f)) + gameObject.transform.position;
        Vector3 randVect = new Vector3(UnityEngine.Random.Range(-1.5f, -0.9f), UnityEngine.Random.Range(2f,2.13f), UnityEngine.Random.Range(0f, 0.87f));
        ball.transform.position = randVect;
        Vector3 direction = new Vector3(1, 0f, 0f);
        _ballRb.AddForce(direction * ball_force_coef, ForceMode.Impulse);
        i_cnt++;
        Debug.Log(i_cnt.ToString());
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
        
        sensor.AddObservation(ball.transform.position.x);
        sensor.AddObservation(ball.transform.position.y);
        sensor.AddObservation(ball.transform.position.z);
        
        // get robot servo motors angles
        sensor.AddObservation(sl1_tr.rotation.z);
        sensor.AddObservation(sl2_tr.rotation.z);
        sensor.AddObservation(sl3_tr.rotation.z);

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
        // Debug.Log("action received");
        
        // Debug.Log("action sl1_tr: " + actionSl1.ToString());
        // move servo motors using transform and angles in actions received
        // slGameObject1.transform.eulerAngles = new Vector3(0,180f,actionSl1 * (rotationSpeed * Time.deltaTime));
        // slGameObject2.transform.eulerAngles = new Vector3(0,60f,actionSl2 * (rotationSpeed * Time.deltaTime));
        // slGameObject3.transform.eulerAngles = new Vector3(0,-62f,actionSl3 * (rotationSpeed * Time.deltaTime));
        
        // sl1_tr.eulerAngles = new Vector3(0,180,actionSl1);
        // sl2_tr.eulerAngles = new Vector3(0,60f,actionSl2);
        // sl3_tr.eulerAngles = new Vector3(0,-62f,actionSl3);
        if (use_torque)
        {
            var actionSl1 = changeScale(actions.ContinuousActions[++i], -1f, 1f, 0, 2);
            var actionSl2 = changeScale(actions.ContinuousActions[++i], -1f, 1f, 0, 2);
            var actionSl3 = changeScale(actions.ContinuousActions[++i], -1f, 1f, 0, 2);

            sl1_rb.AddTorque(new Vector3(0, 0, actionSl1), ForceMode.Force);
            sl2_rb.AddTorque(new Vector3(0, 0, actionSl2), ForceMode.Force);
            sl3_rb.AddTorque(new Vector3(0, 0, actionSl3), ForceMode.Force);
        }
        else {
            var actionSl1 = changeScale(actions.ContinuousActions[++i], -1f, 1f, -60, 90);
            var actionSl2 = changeScale(actions.ContinuousActions[++i], -1f, 1f, -60, 90);
            var actionSl3 = changeScale(actions.ContinuousActions[++i], -1f, 1f, -60, 90);
            
            sl1_tr.eulerAngles = new Vector3(0,180,actionSl1);
            sl2_tr.eulerAngles = new Vector3(0,60f,actionSl2);
            sl3_tr.eulerAngles = new Vector3(0,-62f,actionSl3);
        }

        // // TODO: set reward
        // if ((Mathf.Abs(ball.transform.position.x - racket.position.x) < 0.05) &&
        //     (Mathf.Abs(ball.transform.position.y - racket.position.y) < 0.05) &&
        //     (Mathf.Abs(ball.transform.position.z - racket.position.z) < 0.05))
        // {
        //     SetReward(0.1f);
        //     EndEpisode();
        // }
        // else
        // {
        //     SetReward(-1f);
        //     EndEpisode();
        // }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "racket")
        {
            Debug.Log("Racket");
            SetReward(1f);
            // EndEpisode();
        // else if (other.collider.tag == "")
        }
        else
        {
            Debug.Log("other");
            SetReward(0f);
        }
        if (other.collider.tag == "floor")
        {
            Debug.Log("Floor");
            // SetReward(-1f);
            EndEpisode();
        }
        // else
        // {
        //     SetReward(-0.1f);
        // }
    }
    /*
     * at the Behavior Parameters on the Agent 
     * �Behaviour Type� has three options, �Heuristic Only�, �default�, and �Inference Only�
     * the agent will run �Inference Only� as it uses the neural network to make decisions
     * When no neural network is provided, it will use �Heuristic Only.�
     */
    
    // public override void Heuristic(in ActionBuffers actionBuffers)
    // {
    //     var continousActionsOut = actionBuffers.ContinuousActions;
    //     continousActionsOut[0] = UnityEngine.Random.Range(-60f, 90f);
    //     continousActionsOut[1] = UnityEngine.Random.Range(-60f, 90f);
    //     continousActionsOut[3] = UnityEngine.Random.Range(-60f, 90f);
    //
    // }



    /*
     * This sets the mass and scale of the ball to its default size
     * and the function will be called in �Initialize� and �OnEpisodeBegin�
     */
    void ResetScene()
    {
        _ballRb.mass = defaultParameters.GetWithDefault("mass", 1.0f);
        var scale = defaultParameters.GetWithDefault("scale", 1.0f);
        ball.transform.localScale = new Vector3(scale, scale, scale);
        Debug.Log("end reset scene");
    }

    
}
