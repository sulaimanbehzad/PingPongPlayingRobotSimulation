using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MoveMotor : MonoBehaviour
{
    // Start is called before the first frame update
    private List<string> timeList = new List<string>();
    private List<string> angleList = new List<string>();
    void Start()
    {
        using(var reader = new StreamReader(@"C:\Users\Sulaiman's PC\PingPongPlayingRobotSimulation\Assets\Scripts\Trajectory_LinCirLin_MaxAcc.csv_Teta_BSpline_Motor1.csv"))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                timeList.Add(values[0]);
                angleList.Add(values[1]);
                Debug.Log("value 0");
                Debug.Log(values[0].ToString());
                Debug.Log("value 1");
                Debug.Log(values[1].ToString());
            }
            reader.Close();
        }
        Debug.Log(timeList.Count.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
