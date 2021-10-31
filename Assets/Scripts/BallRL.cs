using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallRL : MonoBehaviour
{
    public GameObject canvasText;
    // public GameObject myArea;
    public RacketAgent agent_A;
    public ChangeColorOnContact changecolorclass;
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
            cnt_win++;
            agent_A.SetReward(1f);
            // if(!agent_A.get_first_hit()){
            //     // Debug.Log("Racket");
            //     cnt_win++;
            //     // Debug.Log("Episodes won: " + cnt_win.ToString());
            //     agent_A.SetReward(1f);
            //     agent_A.set_first_hit(false);
            // }
        }
        else if(collision.gameObject.CompareTag("court_B")){
            agent_A.SetReward(5f);
        }
        else
        {
            // Debug.Log("other");
            agent_A.SetReward(0f);
        }
        if (collision.gameObject.CompareTag("floor")  || collision.gameObject.CompareTag("wall"))
        {
            
            string logText = "Total Frame:" + agent_A.get_frame().ToString() + "\t Total Episode: " + agent_A.get_episode().ToString() + "\t Total Won:" +
                             cnt_win.ToString() +"\t Total Oponnent Court: " + changecolorclass.get_cnt_opp_court();
            Debug.Log(logText);
            canvasText.GetComponent<Text>().text = logText;
            agent_A.EndEpisode();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
