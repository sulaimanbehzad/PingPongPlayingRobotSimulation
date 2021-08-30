using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MoveMotor : MonoBehaviour
{
    // This two lists will hold the csv data
    private List<string> timeList = new List<string>();
    private List<string> angleList = new List<string>();
    private List<string> velocity_x = new List<string>();
    private List<string> velocity_y = new List<string>();
    private List<string> velocity_z = new List<string>();
   
    private int ang_cnt=0;
    private float y_angle = 0f;
    
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        // reader object takes path as input
        // !!! path is system dependent
        // TODO: make path system independent
        String path_CSV1 = @"C:\Users\Sulaiman's PC\PingPongPlayingRobotSimulation\Assets\Scripts\Trajectory_LinCirLin_MaxAcc.csv_Teta_BSpline_Motor1.csv";
        String path_CSV2 = @"C:\Users\Sulaiman's PC\PingPongPlayingRobotSimulation\Assets\Scripts\Trajectory_LinCirLin_MaxAcc.csv_Teta_BSpline_Motor2.csv";
        String path_CSV3 = @"C:\Users\Sulaiman's PC\PingPongPlayingRobotSimulation\Assets\Scripts\Trajectory_LinCirLin_MaxAcc.csv_Teta_BSpline_Motor3.csv";
        String cur_path = "";
        String cur_g_obj = gameObject.name;
        

        switch (cur_g_obj)
        {
            case "SL1":
                cur_path = path_CSV1;
                y_angle = 180f;
                break;
            case "SL2":
                cur_path = path_CSV2;
                y_angle = 60f;
                break;
            case "SL3":
                cur_path = path_CSV3;
                y_angle = -62f;
                break;
        }

        using(var reader = new StreamReader(cur_path))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                timeList.Add(values[0]);
                angleList.Add(values[1]);
                // Debug.Log("value 0");
                // Debug.Log(values[0].ToString());
                // Debug.Log("value 1");
                // Debug.Log(values[1].ToString());
            }
            reader.Close();
        }
        // Debug.Log(timeList.Count.ToString());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ang_cnt < angleList.Count)
        {
            transform.eulerAngles = new Vector3(0f, y_angle, float.Parse(angleList[ang_cnt]));
            // Debug.Log(float.Parse(angleList[ang_cnt]).ToString());
            // Debug.Log("count: " + ang_cnt.ToString());
            rb = GetComponent<Rigidbody>();
            // Debug.Log(rb.name + ": | velocity: "+ rb.angularVelocity.ToString());
            ang_cnt++;
        }
        
    }
}
