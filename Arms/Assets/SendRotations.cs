using UnityEngine;
using System.Collections;
using System.IO;
using System.IO.Ports;

public class SendRotations : MonoBehaviour
{
    private SerialPort sp;
    public float stepPerDegree;

    void Start()
    {
        Application.targetFrameRate = 90;
        sp = new SerialPort("COM4", 9600);
        sp.ReadTimeout = 50;
        sp.Open();
        stepPerDegree = 5.68888888f;
    }
    void FixedUpdate()
    {

    }
    public void WriteToArduino(string message)
    {
        sp.WriteLine(message);
        sp.BaseStream.Flush();
    }
}
