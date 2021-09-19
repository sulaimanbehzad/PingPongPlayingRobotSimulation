using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MoveAllMotors : MonoBehaviour
{
    private String path_csv = @"C:\Users\Sulaiman's PC\PingPongPlayingRobotSimulation\Assets\Scripts\Tetas.csv";
      // This two lists will hold the csv data
    // private List<string> timeList = new List<string>();
    private List<string> angleList1 = new List<string>();
    private List<string> angleList2 = new List<string>();
    private List<string> angleList3 = new List<string>();
    private List<string> torqueList1 = new List<string>();
    private List<string> torqueList2 = new List<string>();
    private List<string> torqueList3 = new List<string>();
    private List<string> torqueTimeList = new List<string>();


    private int ang_cnt=0;
    private const float y_angle1 = 1800f;
    private const float y_angle2 = 60f;
    private const float y_angle3 = -62f;
    public GameObject sl1;
    public GameObject sl2;
    public GameObject sl3;
    public GameObject torqueText;
    
    private Rigidbody sl1_rb;
    private Rigidbody sl2_rb;
    private Rigidbody sl3_rb;

    private Transform sl1_tr;
    private Transform sl2_tr;
    private Transform sl3_tr;

    private HingeJoint sl1_hj;
    private HingeJoint sl2_hj;
    private HingeJoint sl3_hj;
    private void Awake()
    {
        Debug.Log("Scale time is: " +  Time.timeScale);
        Debug.Log("Delta time is: " +  Time.fixedDeltaTime);
        
        sl1_rb = sl1.GetComponent<Rigidbody>();
        sl2_rb = sl2.GetComponent<Rigidbody>();
        sl3_rb = sl3.GetComponent<Rigidbody>();

        sl1_tr = sl1.GetComponent<Transform>();
        sl2_tr = sl2.GetComponent<Transform>();
        sl3_tr = sl3.GetComponent<Transform>();

        sl1_hj = sl1.GetComponent<HingeJoint>();
        sl2_hj = sl2.GetComponent<HingeJoint>();
        sl3_hj = sl3.GetComponent<HingeJoint>();

    }

    
    private void ReadFromCSV()
    {
        using(var reader = new StreamReader(path_csv))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                angleList1.Add(values[0]);
                angleList2.Add(values[1]);
                angleList3.Add(values[2]);
            }
            reader.Close();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ReadFromCSV();
        Debug.Log("DATA READ!");
        Debug.Log(angleList1.Count + " | " + angleList2.Count + " | " +angleList3.Count);
    }

    private void MoveServoMotor(GameObject servo, List<string> angleList, float y_angle, List<string> torqueList, Rigidbody rb, Transform tr, HingeJoint hj)
    {

        // Vector3 oldPoint = tr.eulerAngles;
        sl1.transform.eulerAngles = new Vector3(0f, 0, 0);
        // Vector3 newPoint = tr.eulerAngles;
        // Vector3 x = Vector3.Cross(oldPoint.normalized, newPoint.normalized);
        // float theta = Mathf.Asin(x.magnitude);
        // Vector3 w = x.normalized * theta / (Time.timeScale * Time.fixedDeltaTime);
        // Vector3 w = x.normalized * theta / Time.fixedDeltaTime;
        // Quaternion q = transform.rotation * rb.inertiaTensorRotation;
        // Vector3 torque = q * Vector3.Scale(rb.inertiaTensor, (Quaternion.Inverse(q) * w));
        Vector3 torque2 = hj.currentTorque;
        Debug.Log(rb.name + "'s Torque: " + torque2.ToString("F4") + " N*m.");
        Debug.Log(rb.name + "current angle: " + tr.eulerAngles.ToString());
        // torqueTimeList.Add(Time.time.ToString());
        torqueList.Add(torque2.ToString("F4"));
        // torqueText.GetComponent<Text>().text = rb.name + "'s Torque: " + torque2.ToString("F4") + " N*m.";
        // Debug.Log(rb.name + ": | velocity: "+ rb.angularVelocity);
        // yield return new WaitForSeconds(10);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (ang_cnt < angleList1.Count)
        {
            MoveServoMotor(sl1, angleList1, y_angle1, torqueList1, sl1_rb, sl1_tr, sl1_hj);
            MoveServoMotor(sl2, angleList2, y_angle2, torqueList2, sl2_rb, sl2_tr, sl2_hj);
            MoveServoMotor(sl3, angleList3, y_angle3, torqueList3, sl3_rb, sl3_tr, sl3_hj);
            ang_cnt++;
        }

    }
    public string ToCSV()
    {
        Debug.Log("COUNTS: ______________________ ");
        Debug.Log(torqueList1.Count + " | " + torqueList2.Count + " | " + torqueList3.Count);
        var sb = new StringBuilder("X,Y,Z,X,Y,Z,X,Y,Z");
        for(int i=0; i<torqueList1.Count-1; i++)
        {
            torqueList1[i] = removeChars(torqueList1[i]);
            torqueList2[i] = removeChars(torqueList2[i]);
            torqueList3[i] = removeChars(torqueList3[i]);
            sb.Append('\n').Append(torqueList1[i]).Append(',').Append(torqueList2[i]).Append(',').Append(torqueList3[i]);
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

        var filePath = Path.Combine(folder, "export.csv");

        using(var writer = new StreamWriter(filePath, false))
        {
            writer.Write(content);
        }

        // Or just
        //File.WriteAllText(content);

        Debug.Log($"CSV file written to \"{filePath}\"");
    }
    private void OnApplicationQuit()
    {
        saveToFile();
    }
}
