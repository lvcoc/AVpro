/** 
 *Copyright(C) 2017 by MMHD 
 *All rights reserved. 
 *FileName:     AVProManager.cs
 *Author:       Joel
 *Date:         2018.2.2 
 *Description:  AVProVideo 当前视频播放完毕自动播放下一视频
 *History: By307035570
*/
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine.SceneManagement;
using System.IO;

public class AVProManager : MonoBehaviour
{
    private static AVProManager instance;
    // public SerialController serial;
    public GameObject rst;
    public GameObject pianzi;
    public GameObject can;
    //public message message;
    public int num=0;
    private string sName;
    private string path;

    public static AVProManager Instance
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
    //private string rtsp;
    private void Start()
    {

      //  transform.GetComponent<MediaPlayer>().m_VideoPath = rtsp;
        //  Cursor.visible = false;

    }
    public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode er)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.ReadyToPlay:
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                {
                    message.Instance.peopletime = 0;
                }

                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                {
                    if (num==0)
                    {
                        num++;
                        transform.GetComponent<MediaPlayer>().m_VideoPath = "D:\\城市生长V3.mp4";
                        MediaPlayer media = (transform.GetComponent<MediaPlayer>()) as MediaPlayer;
                        media.OpenVideoFromFile(media.m_VideoLocation, media.m_VideoPath, true);
                    }
                    else if (num==1)
                    {
                        num = 0;
                        transform.GetComponent<MediaPlayer>().m_VideoPath = "D:\\cc2.mov";
                        MediaPlayer media = (transform.GetComponent<MediaPlayer>()) as MediaPlayer;
                        media.OpenVideoFromFile(media.m_VideoLocation, media.m_VideoPath, false);
                        // pianzi.SetActive(false);
                        // rst.SetActive(true);
                        can.SetActive(true);
                       // rst.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                        // message.Instance.color = Color.white;
                        message.Instance.peopletime = 0;
                        message.Instance.f = false;
                        message.Instance.xun = false;
                        message.Instance.s = false;
                        message.Instance.leave = false;
                    }
                    
                }
                break;
            default:
                break;
        }
    }

}
