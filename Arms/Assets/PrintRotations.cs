using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;

public class PrintRotations : MonoBehaviour
{

    public GameObject RArmCollarbone;
    public GameObject RArmUpper1;
    public GameObject RArmUpper2;
    public GameObject RArmForearm1;
    public GameObject RArmForearm2;
    public GameObject RArmHand;
    public GameObject RArmIndex1;
    public GameObject RArmMid1;
    public GameObject RArmPinky1;
    public GameObject RArmRing1;
    public GameObject RArmThumb1;

    private List<float> rotations = new List<float>();
    private List<float> simRotations = new List<float>();
    private List<float> restingRotations = new List<float>();
    private List<float> deltaRotations = new List<float>();
    private List<int> directions = new List<int>();
    public SerialPort sp;
    public float stepPerDegree;
    public float degreePerFrame;
    public float gearRatio;
    public int frameRate;
    public int baudRate;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = frameRate;
        sp = new SerialPort("COM4", baudRate);
        sp.ReadTimeout = 50;
        sp.Open();

        //initial rotations set to resting position
        restingRotations.Add(Mathf.Repeat(RArmCollarbone.transform.localEulerAngles.z, 360.0f));
        restingRotations.Add(Mathf.Repeat(RArmUpper1.transform.localEulerAngles.y, 360.0f));
        //rotations.Add(RArmUpper2.transform.localEulerAngles);
        restingRotations.Add(Mathf.Repeat(RArmForearm1.transform.localEulerAngles.x, 360.0f));
        restingRotations.Add(Mathf.Repeat(RArmForearm2.transform.localEulerAngles.x, 360.0f));
        restingRotations.Add(Mathf.Repeat(RArmHand.transform.localEulerAngles.z, 360.0f));
        Quaternion indexMiddle = RArmIndex1.transform.localRotation;
        restingRotations.Add(Mathf.Repeat(indexMiddle.eulerAngles.z, 360.0f));
        Quaternion pinkyRing = RArmRing1.transform.localRotation;
        restingRotations.Add(Mathf.Repeat(pinkyRing.eulerAngles.z, 360.0f));
        restingRotations.Add(Mathf.Repeat(RArmThumb1.transform.localEulerAngles.z, 360.0f));

        simRotations = new List<float>(restingRotations);
        rotations = new List<float>(simRotations);
        deltaRotations = new List<float>(simRotations);
        foreach (float i in simRotations)
        {
            directions.Add(0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Target Rotations
        //Axis to change for respective body part: z y x x z z z z
        rotations[0] = Mathf.Repeat(RArmCollarbone.transform.localEulerAngles.z,360.0f);
        rotations[1] = Mathf.Repeat(RArmUpper1.transform.localEulerAngles.y, 360.0f);
        //rotations[2] = RArmUpper2.transform.localEulerAngles; //remove
        rotations[2] = Mathf.Repeat(RArmForearm1.transform.localEulerAngles.x, 360.0f);
        rotations[3] = Mathf.Repeat(RArmForearm2.transform.localEulerAngles.x, 360.0f);
        rotations[4] = Mathf.Repeat(RArmHand.transform.localEulerAngles.z, 360.0f);
        //setting fingers based on index ring thumb
        rotations[5] = Mathf.Repeat(RArmIndex1.transform.localEulerAngles.z, 360.0f);
        rotations[6] = Mathf.Repeat(RArmRing1.transform.localEulerAngles.z, 360.0f);
        rotations[7] = Mathf.Repeat(RArmThumb1.transform.localEulerAngles.z, 360.0f);

        //sending rotation commands to motors 1 frame at a time
        string package = "";
        int numSteppers = 3;//set to rotations.Count for all motors
        for (int i = 0; i < numSteppers; i++)
        {
            int steps = (int)Mathf.Round((rotations[i] * stepPerDegree));
            package = package + steps.ToString() + ' ';
        }
        Debug.Log(package);
        WriteToArduino(package);

        //testing single index finger motor
        //WriteToArduino(((int)Mathf.Round((directions[5] * degreePerFrame * stepPerDegree / gearRatio))).ToString());

        //rotations = target
        //simulate arduino, only pass how many steps it can make in a frame
        //if ((deltaRotations[5]>=10f) || (deltaRotations[5]<=-10f))
        //    WriteToArduino((10).ToString());

        //WriteToArduino(((int)Mathf.Round((deltaRotations[5] * stepPerDegree * gearRatio))).ToString());
        //simRotations = new List<float>(rotations);
    }
    void OnApplicationQuit()
    {
        ReturnToResting();
        sp.Close();
    }

    //Todo: Setup calibration to set arm to position detailed by static model.
    //Default arm position to arm by side. Set model to arm by side.
    //Use arm by side values as initial point.  Return roboarm to arm by side on exit.
    void ReturnToResting()
    {
        return;
    }
    public void WriteToArduino(string message)
    {
        sp.WriteLine(message);
        sp.BaseStream.Flush();
    }
    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }
}
