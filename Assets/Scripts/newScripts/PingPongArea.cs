using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongArea : MonoBehaviour
{
    public GameObject ball;
    public GameObject agentA;
    public GameObject agentB;
    public bool is2D;
    Rigidbody ballRb;
    Rule rule;
    Matrix4x4 Convert_x;

    // Start is called before the first frame update
    void Start()
    {
        Convert_x = Matrix4x4.identity;
        if (is2D)
        {
            Convert_x.m00 = 0;
        }
        ballRb = ball.GetComponent<Rigidbody>();
        rule = ball.GetComponent<Rule>();
        MatchReset();
    }
    
    public void MatchReset()
    {
        var flip = Random.Range(0, 2);
        ballRb.position = new Vector3(0F, 1.0F, 0F) + transform.position;
        ballRb.angularVelocity = Vector3.zero;
        if (flip == 0)
        {
            rule.log.turn = 2;
            //ballRb.velocity = Convert_x * new Vector3(Random.Range(-2F, 2F), Random.Range(-2F, 2F), Random.Range(-6F, -2F));
            ballRb.velocity = Convert_x * new Vector3(4F, 0F, 0F);
        }
        else
        {
            rule.log.turn = 1;
            //ballRb.velocity = Convert_x * new Vector3(Random.Range(-2F, 2F), Random.Range(-2F, 2F), Random.Range(2F, 6F));
            ballRb.velocity = Convert_x * new Vector3(-4F, 0F, 0F);
        }
    }
}
