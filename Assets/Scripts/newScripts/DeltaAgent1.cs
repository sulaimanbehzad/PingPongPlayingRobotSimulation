using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class DeltaAgent1 : Agent
{
    [Header("Specific to Tennis")]
    public GameObject myArea;
    public GameObject ball;
    private Rigidbody _ballRb;
    public GameObject opponent;

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

    //environmental settings
    public bool isOpponent;
    public bool use_torque;
    public bool is2D = false;
    //observation
    public bool isSpinObservable = false;
    public bool isTiming = true;
    public bool isBallTouch = false;
    public bool isMemory = false;
    public bool isHitting_info = false;

    public float score;

    //public float ball_force_coef;
    //private int i_cnt = 0;

    //正規化
    float max_Rx = 7;
    float max_Rv = 10;
    //float max_Rw = 60;
    //float max_Rf = 20;
    //float max_Rt = 10F;
    float max_Bw = 1*2*Mathf.PI;

    public Vector3 BallTouch = Vector3.zero;
    public Vector3 Hitting_V = Vector3.zero;
    public Quaternion Hitting_Q = Quaternion.identity;

    Matrix4x4 Convert_x;
    Matrix4x4 Convert_w;
    Quaternion Convert_wq;
    float turn;

    // Looks for the scoreboard based on the name of the gameObjects.
    // Do not modify the names of the Score GameObjects

    public override void Initialize()
    {
        _ballRb = ball.GetComponent<Rigidbody>();
        sl1_rb = sl1.GetComponent<Rigidbody>();
        sl2_rb = sl2.GetComponent<Rigidbody>();
        sl3_rb = sl3.GetComponent<Rigidbody>();

        sl1_tr = sl1.GetComponent<Transform>();
        sl2_tr = sl2.GetComponent<Transform>();
        sl3_tr = sl3.GetComponent<Transform>();

        Convert_x = Matrix4x4.identity;
        Convert_x.m33 = 0;
        Convert_w = Matrix4x4.identity;
        Convert_w.m33 = 0;
        Convert_wq = Quaternion.AngleAxis(0, Vector3.up);
        if (isOpponent)
        {
            Convert_x.m00 = -1;
            Convert_x.m22 = -1;
            Convert_w.m00 = -1;
            Convert_w.m22 = -1;
            Convert_wq = Quaternion.AngleAxis(180, Vector3.up);
        }
        if (is2D)
        {
            Convert_x.m00 = 0;
            Convert_w.m11 = 0;
            Convert_w.m22 = 0;
        }
        SetRobot();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Agent & opponent racket position & velocity

        //ball position & velocity
        Vector3 ball_pos = _ballRb.position - myArea.transform.position;
        ball_pos = Convert_x * ball_pos;
        Vector3 ball_vel = Convert_x * _ballRb.velocity;
        Vector3 ball_rotvel = Convert_w * _ballRb.angularVelocity;

        //normalize
        ball_pos = ball_pos / max_Rx;
        ball_vel = ball_vel / max_Rv;
        ball_rotvel = ball_rotvel / max_Bw;

        //racket 
       
        //ball
        //position
        sensor.AddObservation(ball_pos);
        //velocity
        sensor.AddObservation(ball_vel);
        //spin
        if (isSpinObservable)
        {
            sensor.AddObservation(ball_rotvel);
        }
        if (isBallTouch)
        {
            sensor.AddObservation(BallTouch);
        }
        if (isHitting_info)
        {
            sensor.AddObservation(Hitting_V);
            sensor.AddObservation(Hitting_Q);
            sensor.AddObservation(opponent.GetComponent<DeltaAgent1>().Hitting_V);
            sensor.AddObservation(opponent.GetComponent<DeltaAgent1>().Hitting_Q);
        }
        if (isTiming)
        {
            Rule rule = ball.GetComponent<Rule>();
            if (rule.log.hit == 0 && rule.log.bound == 1)
            {
                if (!isOpponent && rule.log.turn == 1)
                    turn = 1F;
                else if (isOpponent && rule.log.turn == 2)
                    turn = 1F;
                else if (!isOpponent && rule.log.turn == 1)
                    turn = -1F;
                else if (isOpponent && rule.log.turn == 1)
                    turn = -1F;
                else
                    turn = 0F;
            }
            else
                turn = 0F;
            sensor.AddObservation(turn);
            sensor.AddObservation(rule.log.num_return / rule.max_return);
        }
    }

    public float changeScale(float v, float min, float max, float newMin, float newMax)
    {
        float vP = ((((v - min) / (max - min)) * (newMax - newMin)) + newMin);
        return vP;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var i = -1;
        if (use_torque)
        {
            var actionSl1 = changeScale(actions.ContinuousActions[++i], -1f, 1f, 0, 2);
            var actionSl2 = changeScale(actions.ContinuousActions[++i], -1f, 1f, 0, 2);
            var actionSl3 = changeScale(actions.ContinuousActions[++i], -1f, 1f, 0, 2);

            sl1_rb.AddTorque(new Vector3(0, 0, actionSl1), ForceMode.Force);
            sl2_rb.AddTorque(new Vector3(0, 0, actionSl2), ForceMode.Force);
            sl3_rb.AddTorque(new Vector3(0, 0, actionSl3), ForceMode.Force);
        }
        else
        {
            var actionSl1 = changeScale(actions.ContinuousActions[++i], -1f, 1f, -60, 90);
            var actionSl2 = changeScale(actions.ContinuousActions[++i], -1f, 1f, -60, 90);
            var actionSl3 = changeScale(actions.ContinuousActions[++i], -1f, 1f, -60, 90);

            sl1_tr.eulerAngles = new Vector3(0, 180, actionSl1);
            sl2_tr.eulerAngles = new Vector3(0, 60f, actionSl2);
            sl3_tr.eulerAngles = new Vector3(0, -62f, actionSl3);
        }
    }
   
    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.gameObject.CompareTag("ball"))
    //    {
    //        if (turn == 1)
    //            AddReward(0.3F);
    //        else
    //            AddReward(-0.1F);
    //    }
    //}

    private void FixedUpdate()
    {
        //Restrict_Area();
        if (!isMemory)
        {
            BallTouch = Vector3.zero;
            Hitting_V = Vector3.zero;
            Hitting_Q = Quaternion.identity;
        }
    }

    public override void Heuristic(in ActionBuffers actionBuffers)
    {
        var continousActionsOut = actionBuffers.ContinuousActions;
        continousActionsOut[2] = Input.GetAxis("Horizontal");    // Racket Movement
        continousActionsOut[1] = Input.GetAxis("Vertical");   // Racket Rotation
    }

    public override void OnEpisodeBegin()
    {
        SetRobot();
    }

    public void SetRobot()
    {

        sl1_tr.eulerAngles = new Vector3(0, 180f, 0);
        sl2_tr.eulerAngles = new Vector3(0, 60f, 0);
        sl3_tr.eulerAngles = new Vector3(0, -62, 0f);

    }
    //public void Restrict_Area()
    //{
    //    Vector3 area_pos = new Vector3(0, 1, -2);
    //    Vector3 area_size = new Vector3(4, 2, 4);
    //    Vector3 table_pos = new Vector3(0, 0, 0);
    //    Vector3 table_size = new Vector3(1.525F, 0.76F*2, 2.74F);
    //    in_the_box(AgentRb, area_pos, area_size);
    //    out_the_box(AgentRb, table_pos, table_size);
    //}

    //private void in_the_box(Rigidbody rb, Vector3 box_pos, Vector3 box_size)
    //{
    //    Vector3 rb_pos = Convert_x * (rb.position - myArea.transform.position);
    //    Vector3 Re_v = Convert_x * rb.velocity;
    //    if ((rb_pos.x - box_pos.x) * Mathf.Sign(Re_v.x) > box_size.x / 2)
    //    {
    //        AddReward(-Mathf.Abs((Re_v.x+max_Rv*0.1F) / (max_Rv+max_Rv*0.1F)));
    //        Re_v.x = 0;
    //        rb_pos.x = (box_size.x / 2) * Mathf.Sign(rb_pos.x - box_pos.x) + box_pos.x;
    //    }
    //    if ((rb_pos.y - box_pos.y) * Mathf.Sign(Re_v.y) > box_size.y / 2)
    //    {
    //        AddReward(-Mathf.Abs((Re_v.y+max_Rv*0.1F) / (max_Rv+max_Rv*0.1F)));
    //        Re_v.y = 0;
    //        rb_pos.y = (box_size.y / 2) * Mathf.Sign(rb_pos.y - box_pos.y) + box_pos.y;
    //    }
    //    if ((rb_pos.z - box_pos.z) * Mathf.Sign(Re_v.z) > box_size.z / 2)
    //    {
    //        AddReward(-Mathf.Abs((Re_v.z+max_Rv*0.1F) / (max_Rv+max_Rv*0.1F)));
    //        Re_v.z = 0;
    //        rb_pos.z = (box_size.z / 2) * Mathf.Sign(rb_pos.z - box_pos.z) + box_pos.z;
    //    }
    //    rb.velocity = Convert_x * Re_v;
    //    rb_pos = Convert_x * rb_pos;
    //    rb.position = rb_pos + myArea.transform.position;
    //}

    //private void out_the_box(Rigidbody rb, Vector3 box_pos, Vector3 box_size)
    //{
    //    Vector3 rb_pos = Convert_x * (rb.position - myArea.transform.position);
    //    Vector3 Re_v = Convert_x * rb.velocity;
    //    if (Mathf.Abs(rb_pos.x - box_pos.x) <= box_size.x / 2)
    //    {
    //        if (Mathf.Abs(rb_pos.y - box_pos.y) <= box_size.y / 2)
    //        {
    //            if (Mathf.Abs(rb_pos.z - box_pos.z) <= box_size.z / 2)
    //            {
    //                Vector3 dis = rb_pos - (box_pos + box_size / 2);
    //                Vector3 _dis = rb_pos - (box_pos - box_size / 2);
    //                List<float> dis_list = new List<float> { Mathf.Abs(dis.x), Mathf.Abs(dis.y), Mathf.Abs(dis.z), Mathf.Abs(_dis.x), Mathf.Abs(_dis.y), Mathf.Abs(_dis.z) };
    //                float min = dis_list[0];
    //                int min_id = 0;
    //                for(int i = 1; i < dis_list.Count; i++)
    //                {
    //                    if(min > dis_list[i])
    //                    {
    //                        min = dis_list[i];
    //                        min_id = i;
    //                    }
    //                }
    //                if (min_id == 0)
    //                {
    //                    rb_pos.x = box_pos.x + box_size.x / 2;
    //                    if (Re_v.x < 0)
    //                    {
    //                        AddReward(-Mathf.Abs((Re_v.x+1) / (max_Rv+1)));
    //                        Re_v.x = 0;
    //                    }
    //                }
    //                else if (min_id == 1)
    //                {
    //                    rb_pos.y = box_pos.y + box_size.y / 2;
    //                    if (Re_v.y < 0)
    //                    {
    //                        AddReward(-Mathf.Abs((Re_v.y+max_Rv*0.1F) / (max_Rv+max_Rv*0.1F)));
    //                        Re_v.y = 0;
    //                    }
    //                }
    //                else if (min_id == 2)
    //                {
    //                    rb_pos.z = box_pos.z + box_size.z / 2;
    //                    if (Re_v.z < 0)
    //                    {
    //                        AddReward(-Mathf.Abs((Re_v.z+max_Rv*0.1F) / (max_Rv+max_Rv*0.1F)));
    //                        Re_v.z = 0;
    //                    }
    //                }
    //                else if (min_id == 3)
    //                {
    //                    rb_pos.x = box_pos.x - box_size.x / 2;
    //                    if (Re_v.x > 0)
    //                    {
    //                        AddReward(-Mathf.Abs((Re_v.x+max_Rv*0.1F) / (max_Rv+max_Rv*0.1F)));
    //                        Re_v.x = 0;
    //                    }
    //                }
    //                else if (min_id == 4)
    //                {
    //                    if (Re_v.y > 0)
    //                    {
    //                        AddReward(-Mathf.Abs((Re_v.y+max_Rv*0.1F) / (max_Rv+max_Rv*0.1F)));
    //                        Re_v.y = 0;
    //                    }
    //                }
    //                else
    //                {
    //                    rb_pos.z = box_pos.z - box_size.z / 2;
    //                    if (Re_v.z > 0)
    //                    {
    //                        AddReward(-Mathf.Abs((Re_v.z+max_Rv*0.1F) / (max_Rv+max_Rv*0.1F)));
    //                        Re_v.z = 0;
    //                    }
    //                }

    //            }
    //        }
    //    } 
    //    rb.velocity = Convert_x * Re_v;
    //    rb_pos = Convert_x * rb_pos;
    //    rb.position = rb_pos + myArea.transform.position;
    //}

}