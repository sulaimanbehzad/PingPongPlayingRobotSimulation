using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule : MonoBehaviour
{
    public GameObject myArea;
    public GameObject gamelog;
    public GameLog log;
    PingPongArea area;
    DeltaAgent1 agent_A;
    DeltaAgent1 agent_B;
    public int max_return = 6;

    // Start is called before the first frame update
    void Start()
    {
        area = myArea.GetComponent<PingPongArea>();
        agent_A = area.agentA.GetComponent<DeltaAgent1>();
        agent_B = area.agentB.GetComponent<DeltaAgent1>();
        log = gamelog.GetComponent<GameLog>();
    }
    private void FixedUpdate()
    {
        //agent_A.AddReward(-0.001F);
        //agent_B.AddReward(-0.001F);
    }

    void Reset()
    {
        //Debug.Log("Game Finished!");
        agent_A.EndEpisode();
        agent_B.EndEpisode();
        log.Reset();
        area.MatchReset();
    }

    void AgentAWins()
    {
        //Debug.Log("A wins");
        agent_A.SetReward(5);
        agent_B.SetReward(-5);
        agent_A.score += 1;
        Reset();
    }
    void AgentAMiss()
    {
        //Debug.Log("A miss");
        agent_A.SetReward(-2.5F);
        Reset();
    }
    void AgentBWins()
    {
        //Debug.Log("B wins");
        agent_A.SetReward(-5);
        agent_B.SetReward(5);
        agent_B.score += 1;
        Reset();
    }
    void AgentBMiss()
    {
        //Debug.Log("B miss");
        agent_B.SetReward(-2.5F);
        Reset();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("net"))
        {
            if (log.last_succeeded_agent == 0)
            {
                if (log.turn == 1)
                {
                    if (log.bound == 1)
                        AgentAMiss();
                    else
                        Reset();
                }
                else if (log.turn == 2)
                {
                    if (log.bound == 1)
                        AgentBMiss();
                    else
                        Reset();
                }
                else
                    Reset();
            }
            else if(log.last_succeeded_agent == 1)
            {
                AgentAWins();
            }
            else if (log.last_succeeded_agent == 2)
            {
                AgentBWins();
            }
        }
        if (collision.gameObject.CompareTag("racket_A"))
        {
            Debug.Log("hit racket a");
            if (log.turn == 1) //agent_Aのターンなら
            {
                if (log.hit == 0 && log.bound == 1)
                {
                    log.hit = 1;
                    log.bound = 0;
                }
                else if (log.last_succeeded_agent == 2)
                    AgentBWins();
                else
                    AgentAMiss();
            }
            else if (log.turn == 2) //agent_Bのターンなら
                AgentBWins();
            else
                Reset();
        }
        if (collision.gameObject.CompareTag("racket_B"))
        {
            Debug.Log("hit racket b");

            if (log.turn == 2) //agent_Bのターンなら
            {
                if (log.hit == 0 && log.bound == 1)
                {
                    log.hit = 1;
                    Debug.Log("hit: " + log.hit.ToString());
                    log.bound = 0;
                }
                else if (log.last_succeeded_agent == 1)
                    AgentAWins();
                else
                    AgentBMiss();
            }
            else if (log.turn == 1) //agent_Aのターンなら
                AgentAWins();
            else
                Reset();
        }

        if (collision.gameObject.CompareTag("court_A"))
        {
            Debug.Log(("Court A"));
            if (log.turn == 1) //agent_Aのターンなら
            {
                if (log.last_succeeded_agent == 2)
                {
                    AgentBWins();
                }
                else if (log.last_succeeded_agent == 0)
                {
                    AgentAMiss();
                }
            }
            else if (log.turn == 2) //agent_Bのターンなら
            {
                if(log.hit == 1)
                    log.last_succeeded_agent = 2;
                log.turn = 1;
                log.hit = 0;
                log.bound = 1;
                log.num_return += 1;
                agent_B.AddReward(0.5F);
                if (log.num_return == max_return)
                    AgentBWins();
            }
        }
        if (collision.gameObject.CompareTag("court_B"))
        {
            Debug.Log("Court B");
            if (log.turn == 2) //agent_Bのターンなら
            {
                if (log.last_succeeded_agent == 1)
                {
                    AgentAWins();
                }
                else if (log.last_succeeded_agent == 0)
                {
                    AgentBMiss();
                }
            }
            else if (log.turn == 1) //agent_Aのターンなら
            {
                if(log.hit == 1)
                    log.last_succeeded_agent = 1;
                log.turn = 2;
                log.hit = 0;
                log.bound = 1;
                log.num_return += 1;
                agent_A.AddReward(0.5F);
                if (log.num_return == max_return)
                    AgentAWins();
            }
        }
    }
}
