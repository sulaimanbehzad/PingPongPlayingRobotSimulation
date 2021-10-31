using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnContact : MonoBehaviour
{
    public Material colMaterial;
    private MeshRenderer colMeshRenderer;
    private int cnt_opp_court = 0;
    private bool ball_collision = false;
    public int get_cnt_opp_court(){
        return cnt_opp_court;
    }
    public  void set_ball_collision(bool col){
        this.ball_collision = col;
    }
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
		/*GetComponent<Renderer>().material.SetColor(
			"_Color", OnGround ? Color.black : Color.white
		);
	*/
    }
    private void OnTriggerEnter(Collider col) {
        // Debug.Log("Hi malekom");
        if(col.gameObject.CompareTag("ball")) {
            colMeshRenderer = col.gameObject.GetComponent<MeshRenderer>();
            colMeshRenderer.material = colMaterial;
            if(!ball_collision){
                cnt_opp_court++;
                ball_collision = true;

            }
            
        }

    }
}
