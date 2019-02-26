using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using UMP;
public class alpha : MonoBehaviour {
    public GameObject[] obj;
    public GameObject an;
    private string path;
    private string tf="t";
    private string rtsp;
    private Color a;
    public UniversalMediaPlayer un;
    // Use this for initialization
    void Start () {
        path = "config.txt";
        //Debug.Log(tf);
        Read(path);
        a = new Color();
        a.a = 0;
        un.Path  = rtsp;
    }
	
	// Update is called once per frame
	void Update () {
        if (tf == "f")
        {
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].GetComponent<Image>().color = a;
            }
            an.SetActive(false);
        }
        else if(tf=="t")
        {
            return;
        }
                
	}
    public void Read(string path)
    {
        StreamReader sr = new StreamReader(path, Encoding.Default);
        string line;

        //print(line);
        for (int i = 0; i < 2; i++)
        {
            line = sr.ReadLine();
            //print(line);
            string[] s = line.Split('=');
            switch (i)
            {
                case 0:
                    tf = s[1];
                  //  print(s[1]);
                    break;
                case 1:
                    rtsp = s[1];
                  //  print(s[1]);
                    break;
            }
        }
    }
}
