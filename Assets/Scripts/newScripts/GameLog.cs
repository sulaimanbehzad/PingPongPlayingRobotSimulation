using UnityEngine;

public class GameLog : MonoBehaviour
{
    public int turn; //0:None, 1:agentA, 2:agentB
    public int last_succeeded_agent; //0:None, 1:agentA, 2:agentB
    public int hit; //0:yet, 1:hit
    public int bound; //0:yet, 1:bounded
    public int num_return;
    public GameLog()
    {
        Reset();
    }
    public void Reset()
    {
        turn = 0;
        last_succeeded_agent = 0;
        hit = 0;
        bound = 0;
        num_return = 0;
    }
}
