using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MoveMotor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        using(var reader = new StreamReader(@"C:\Users\Sulaiman's PC\PingPongPlayingRobotSimulation\Assets\Scripts\Trajectory_LinCirLin_MaxAcc.csv_Teta_BSpline_Motor1.csv"))
        {
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                listA.Add(values[0]);
                listB.Add(values[1]);
            }
            reader.Close();
        }
        Console.WriteLine("hello");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
