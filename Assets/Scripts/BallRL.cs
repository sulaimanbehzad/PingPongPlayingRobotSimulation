using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRL : MonoBehaviour
{
    // public GameObject myArea;
    public RacketAgent agent_A;
    // PingPongArea area;
    private static int cnt_win = 0;
    // Start is called before the first frame update
    void Start()
    {
        // area = myArea.GetComponent<PingPongArea>();
        // agent_A = area.agentA.GetComponent<DeltaAgent>();

    }
    private void OnTriggerEnter(Collider collision)
    {
        // Debug.Log("Collided with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("racket_A"))
        {
            // Debug.Log("Racket");
            cnt_win++;
            // Debug.Log("Episodes won: " + cnt_win.ToString());
            agent_A.SetReward(1f);
        }
        else
        {
            // Debug.Log("other");
            agent_A.SetReward(0f);
        }
        if (collision.gameObject.CompareTag("floor")  || collision.gameObject.CompareTag("wall"))
        {
            Debug.Log("Total Frame:" + agent_A.get_frame() + "\t Total Episode: " + agent_A.get_episode() + "\t Total Won:" + cnt_win);
            agent_A.EndEpisode();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
