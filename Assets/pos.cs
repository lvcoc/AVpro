using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;

public class pos : MonoBehaviour {
    public Transform[] obj;
    private Vector3 o1;
    private Vector3 o2;
    private Vector3 o3;
    private Vector3 o4;

    
    // Use this for initialization
    void Start () {
        // WriteFileByLine("E:\\", "my_newfile1.txt", "");
        Screen.SetResolution(1920,1080,true);
        if (File.Exists("D:\\my_newfile1.txt"))
            Read("D:\\my_newfile1.txt");
    }
	
	// Update is called once per frame
	void Update () {
        o1 = obj[0].position;
       
        o2 = obj[1].position;
        
        o3 = obj[2].position;
        
        o4 = obj[3].position;
        
    }
    public void WriteFileByLine(string file_path, string file_name, string str_info)
    {
        StreamWriter sw;
        if (!File.Exists(file_path + "//" + file_name))
        {
            sw = File.CreateText(file_path + "//" + file_name);//创建一个用于写入 UTF-8 编码的文本
            Debug.Log("文件创建成功！");
        }
        else
        {
              sw = File.AppendText(file_path + "//" + file_name);//打开现有 UTF-8 编码文本文件以进行读取
            // Debug.Log(sw.);
           
        }
        sw.WriteLine("1"+":"+str_info);//以行为单位写入字符串
        sw.Close();
        sw.Dispose();//文件流释放
    }
    public void save()
    {
        WriteFileByLine("D:\\", "my_newfile1.txt", o1.ToString());
        WriteFileByLine("D:\\", "my_newfile1.txt", o2.ToString());
        WriteFileByLine("D:\\", "my_newfile1.txt", o3.ToString());
        WriteFileByLine("D:\\", "my_newfile1.txt", o4.ToString());
    }
    private void Read(string path)
    {
        StreamReader sr = new StreamReader(path, Encoding.Default);
        string line;

        for (int i = 0; i < 4; i++)
        {
            line = sr.ReadLine();
            //Debug.Log(line + "line");
            string[] s = line.Split(':');
            switch (i)
            {
                case 0:
                    obj[0].position = Parse(s[1]);
                    //Debug.Log(s[1] + "s[1]");
                    break;
                case 1:
                    obj[1].position = Parse(s[1]);
                    break;
                case 2:
                    obj[2].position = Parse(s[1]);
                    //Debug.Log(s[1] + "s[2]");
                    break;
                case 3:
                    obj[3].position = Parse(s[1]);
                    //Debug.Log(s[1] + "s[2]");
                    break;
            }
        }
    }

    public static Vector3 Parse(string s)
    {
        s = s.Replace("(", "").Replace(")", "");
        string[] name = s.Split(',');
        return new Vector3(float.Parse(name[0]), float.Parse(name[1]), float.Parse(name[2]));
    }
}
