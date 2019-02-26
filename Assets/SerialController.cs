using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;

public class SerialController : MonoBehaviour
{
    public static SerialController _instance;
    public string comeName = "COM2";
    public int baudRate = 115200;
    public int dataBits = 8;
    public StopBits Bit = StopBits.One;
    public Parity parity = Parity.None;

    SerialPort usbDonglePort;
    Thread portThread;

    Queue<string> _QueueCom;

    private string path;//配置文件路径
    public string cmd;
    public string jiguangbi="180";
    static byte[] cmd_read_off = new byte[] { 0x55, 0x01, 0x01, 0x02, 0x02, 0x02, 0x02, 0x5F };//关灯
    static byte[] cmd_read_on = new byte[] { 0x55, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x5B };//开灯

    void Start()
    {
      // Debug.Log( System.IO.Directory.GetCurrentDirectory());
        _instance = this;
        path = "ComConfig.txt";
        InitPort();
        
        //Debug.Log(comeName);
        _QueueCom = new Queue<string>();

        portThread = new Thread(new ThreadStart(ThreadProc));
        portThread.Start();
    }
    void Update()
    {
        
    }
    private void ThreadProc()
    {
       
       
        _QueueCom = new Queue<string>();
        while (true)
        {
            if (usbDonglePort.IsOpen)
            {
                usbDonglePort.ReadLine();
               jiguangbi= usbDonglePort.ReadLine();
               // Debug.Log(jiguangbi);
            }
            Thread.Sleep(3);
        }
    }
    private void InitPort()
    {
        Read(path);
        usbDonglePort = new SerialPort();
        usbDonglePort.PortName = comeName;
        usbDonglePort.BaudRate = baudRate;
        usbDonglePort.DataBits = dataBits;
        usbDonglePort.StopBits = Bit;
        usbDonglePort.Parity = parity;
        try
        {
            usbDonglePort.Open();
        }
        catch (Exception e)
        {

            Debug.Log(e);
        }
       
    }
    private void Read(string path)
    {
        StreamReader sr = new StreamReader(path, Encoding.Default);
        string line;

        for (int i = 0; i < 2; i++)
        {
            line = sr.ReadLine();
            //Debug.Log(line + "line");
            string[] s = line.Split(':');
            switch (i)
            {
                case 0:
                    comeName = s[1];
                    //Debug.Log(s[1] + "s[1]");
                    break;
                case 1:
                    baudRate = int.Parse(s[1]);
                    //Debug.Log(s[1] + "s[2]");
                    break;
            }
        }
    }
    void OnDisable()
    {
        portThread.Abort();
        try
        {
            usbDonglePort.Close();
        }
        catch (Exception e)
        {

            Debug.Log(e);
        }
        
    }
}

