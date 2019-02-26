using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using System;
using UnityEngine.UI;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;

public class message : MonoBehaviour {
    private static message instance;

    private Material mRST;

    public GameObject rst;
    public GameObject pianzi;
    public GameObject can;
    public MediaPlayer bg;
    public SerialController ser;
    private double jiguang;
    // Use this for initialization
    private string path;
	void Start () {
        path = "time.txt";
        Read(path);
        Image bg = can.GetComponent<Image>();
        StartCoroutine(Load(bg));
       // mRST = rst.GetComponent<Renderer>().material;
       // mRST.SetColor("_Color", new Vector4(255, 255, 255,255));
	}
    private float time = 0;
    private float timer = 10;
    public float peopletime = 0;

    public bool f = false;
    public bool leave = false;
    public bool s = false;
    public bool xun = false;

    public static message Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    public Color color;
    // Update is called once per frame
    void Update () {
        jiguang = Convert.ToDouble(ser.jiguangbi);
        //Debug.Log(xun);
        if (jiguang<180)
        {
            f = true;
            peopletime += Time.deltaTime;
            if (leave)
            {
                s = true;
              //  Debug.Log("s");
            }
            if (s&&peopletime< (timer-1)&& xun==false)
            {
                xun = true;
                f = false;
               // Debug.Log("x");
                peopletime = 0;
               // peopletime += Time.deltaTime;
            }
            if (peopletime>=(timer-1))
            {
                xun = false;
                f = false;
              //  peopletime = 0;
            }
        }
        if (jiguang > 180 && f)
        {
            peopletime += Time.deltaTime;
            leave = true;
            xun = false;
           // Debug.Log("l");
        }
      //  Debug.Log(jiguang);
        if (jiguang < 180)
        {
            bg.Control.Play();
           // color = Color.Lerp(Color.white,Color.black, peopletime);
            //Debug.Log(color);
           // mRST.SetColor("_Color", color);
            // pianzi.SetActive(true);
            // rst.SetActive(false);
              can.SetActive(false);
            if (xun&&AVProManager.Instance.num==0)
            {
                if (bg.Control.IsPlaying() && time == 0&&xun)
                {
                    bg.Control.Rewind();
                }


                time += Time.deltaTime;
                if (time >= timer&&xun)
                {
                    bg.Control.Rewind();
                    time = 0;
                }
            }
           

        }
        else
        {
            
            if (bg.Control.IsPlaying())
                {
                    time = 0;
                    return;
                }

           
        }
	}
    IEnumerator Load(Image image)
    {
        string streamingPath = Application.streamingAssetsPath+ "/pb.jpg";
        // image.gameObject.SetActive(true);
        double startTime = (double)Time.time;
        // WWW www = new WWW("file://C:\\fast-neural-style-tensorflow-master\\generated\\res.jpg");
        WWW www = new WWW("file:///" + streamingPath);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            //获取Texture
            Texture2D texture = www.texture;
            //更多操作...
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;

            startTime = (double)Time.time - startTime;
            // UnityEngine.Debug.Log("WWW加载用时:" + startTime);

        }
    }
    private void Read(string path)
    {
        StreamReader sr = new StreamReader(path, Encoding.Default);
        string line;

        for (int i = 0; i < 1; i++)
        {
            line = sr.ReadLine();
            //Debug.Log(line + "line");
            string[] s = line.Split(':');
            switch (i)
            {
                case 0:
                    timer =int.Parse( s[1]);
                    //Debug.Log(s[1] + "s[1]");
                    break;
            }
        }
    }
}
