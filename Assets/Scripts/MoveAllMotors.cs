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
    private float y_angle1 = 1800f;
    private float y_angle2 = 60f;
    private float y_angle3 = -62f;
    private Vector3 torque;
    public GameObject sl1;
    public GameObject sl2;
    public GameObject sl3;
    public GameObject torqueText;
    
    private Rigidbody rb;
    private HingeJoint hj;
    private Transform tr;
    private void Awake()
    {
        Debug.Log("Scale time is: " +  Time.timeScale);
        Debug.Log("Delta time is: " +  Time.fixedDeltaTime);
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

    private IEnumerator MoveServoMotor(GameObject servo, List<string> angleList, float y_angle, List<string> torqueList)
    {
        rb = servo.GetComponent<Rigidbody>();
        hj = servo.GetComponent<HingeJoint>();
        tr = servo.GetComponent<Transform>();
   
        tr.eulerAngles = new Vector3(0f, y_angle, float.Parse(angleList[ang_cnt]));
        Vector3 torque2 = hj.currentTorque;
        Debug.Log(rb.name + "'s Torque: " + torque2.ToString("F4") + " N*m.");
        Debug.Log(rb.name + "current angle: " + angleList[ang_cnt].ToString());
        // torqueTimeList.Add(Time.time.ToString());
        torqueList.Add(torque2.ToString("F4"));
        torqueText.GetComponent<Text>().text = rb.name + "'s Torque: " + torque2.ToString("F4") + " N*m.";
        // Debug.Log(rb.name + ": | velocity: "+ rb.angularVelocity);
        yield return new WaitForSeconds(10);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (ang_cnt < angleList1.Count)
        {
            StartCoroutine(MoveServoMotor(sl1, angleList1, y_angle1, torqueList1));
            StartCoroutine(MoveServoMotor(sl2, angleList2, y_angle2, torqueList2));
            StartCoroutine(MoveServoMotor(sl3, angleList3, y_angle3, torqueList3));
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
        StopAllCoroutines();
        saveToFile();
    }
}
