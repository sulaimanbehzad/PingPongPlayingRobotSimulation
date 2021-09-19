using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class MoveMotor : MonoBehaviour
{
    // This two lists will hold the csv data
    private List<string> timeList = new List<string>();
    private List<string> angleList = new List<string>();
    private List<string> torqueList = new List<string>();
    private List<string> torqueTimeList = new List<string>();


    private int ang_cnt=0;
    private float y_angle = 0f;
    private Vector3 torque;

    public GameObject torqueText;
    
    Rigidbody rb;

    private HingeJoint hj;
    private WheelCollider _wheelCollider;
    private void Awake()
    {
        Debug.Log("Scale time is: " +  Time.timeScale);
        Debug.Log("Delta time is: " +  Time.deltaTime);
        hj = gameObject.GetComponent<HingeJoint>();
        _wheelCollider = gameObject.GetComponent<WheelCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // reader object takes path as input
        // !!! path is system dependent
        // TODO: make path system independent
        String path_CSV1 = @"C:\Users\Sulaiman's PC\PingPongPlayingRobotSimulation\Assets\Scripts\theta1.csv";
        String path_CSV2 = @"C:\Users\Sulaiman's PC\PingPongPlayingRobotSimulation\Assets\Scripts\theta2.csv";
        String path_CSV3 = @"C:\Users\Sulaiman's PC\PingPongPlayingRobotSimulation\Assets\Scripts\theta3.csv";
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

                angleList.Add(values[0]);
                // angleList.Add(values[1]);
                // Debug.Log("value 0");
                // Debug.Log(values[0].ToString());
                // Debug.Log("value 1");
                // Debug.Log(values[1].ToString());
            }
            reader.Close();
        }
        // Debug.Log(timeList.Count.ToString());
    }

    private float stepAngle = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        // if (ang_cnt < angleList.Count)
        // {
            Vector3 oldPoint = transform.eulerAngles;
            stepAngle -= 0.01f;
            transform.eulerAngles = new Vector3(0f, y_angle, stepAngle);
            Vector3 newPoint = transform.eulerAngles;
            Vector3 x = Vector3.Cross(oldPoint.normalized, newPoint.normalized);
            float theta = Mathf.Asin(x.magnitude);
            // Vector3 w = x.normalized * theta / (Time.timeScale * Time.fixedDeltaTime);
            Vector3 w = x.normalized * theta / Time.fixedDeltaTime;

            // Debug.Log(float.Parse(angleList[ang_cnt]).ToString());
            // Debug.Log("count: " + ang_cnt.ToString());
            rb = GetComponent<Rigidbody>();
            Quaternion q = transform.rotation * rb.inertiaTensorRotation;
            torque = q * Vector3.Scale(rb.inertiaTensor, (Quaternion.Inverse(q) * w));
            // Vector3 torque2 = hj.currentTorque
            float torque3 = _wheelCollider.motorTorque;
            Debug.Log(rb.name + "'s Torque: " + torque3.ToString("F4") + " N*m.");
            torqueTimeList.Add(Time.time.ToString());
            torqueList.Add(torque3.ToString("F4"));
            torqueText.GetComponent<Text>().text = rb.name + "'s Torque: " + torque.ToString("F4") + " N*m.";
            // Debug.Log(rb.name + ": | velocity: "+ rb.angularVelocity);
            ang_cnt++;
        // }
        
    }
    public string ToCSV()
    {
        var sb = new StringBuilder("Torque Z");
        for(int i=0; i<torqueList.Count; i++)
        {
            torqueList[i] = removeChars(torqueList[i]);
            sb.Append('\n').Append(torqueList[i]);
        }

        return sb.ToString();
    }
    public string removeChars(string in_str)
    {
        string[] charsToRemove;
        charsToRemove = new[] {"(", ")"};
        foreach (string c in charsToRemove)
        {
            in_str = in_str.Replace(c, string.Empty);
        }
        return in_str;
    }
    public void saveToFile()
    {
        var content = ToCSV();
        var folder = Application.streamingAssetsPath;

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
            
        }
        // else
        // {
        //     var folder = Application.persistentDataPath;
        // }

        var filePath = Path.Combine(folder, "export_"  + rb.name + ".csv");

        using(var writer = new StreamWriter(filePath, false))
        {
            writer.Write(content);
        }

        // Or just
        //File.WriteAllText(content);

        Debug.Log($"CSV file written to \"{filePath}\"");
    }
    // void OnGUI()
    // {
    //     GUI.contentColor = Color.black;
    //     if (rb.name == "SL1")
    //     {
    //         GUI.Label(new Rect(800, 400, 100, 20), rb.name + "'s Torque: " +message+ " N*m.");
    //     }
    //     else if (rb.name == "SL2")
    //     {
    //         GUI.Label(new Rect(800, 450, 100, 20), rb.name + "'s Torque: " + torque.ToString("F4") + " N*m.");
    //     }
    //     else if (rb.name == "SL3")
    //     {
    //         GUI.Label(new Rect(800, 500, 100, 20), rb.name + "'s Torque: " + torque.ToString("F4") + " N*m.");
    //     }
    // }
    private void OnApplicationQuit()
    {
        saveToFile();
    }
}
