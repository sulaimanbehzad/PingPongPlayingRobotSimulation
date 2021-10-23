using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketRL : MonoBehaviour
{
    public DeltaAgent agent_A;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider collision){
    //     if (collision.gameObject.CompareTag("Court")){

    //         agent_A.SetReward(-0.01f);
    //     }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
