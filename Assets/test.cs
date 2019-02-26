using UnityEngine;
using System.Collections;
using Microsoft.Win32;
using UnityEngine.UI;

public class test : MonoBehaviour
{

   // public Button Btn1;
   // public Button Btn2;
   // public Text text;

    private void Start()
    {
        OnBtn1Click();
    }
    void OnEnable()
    {
       // Btn1.onClick.AddListener(OnBtn1Click);
       // Btn2.onClick.AddListener(OnBtn2Click);
    }

    void OnDisable()
    {
       // Btn1.onClick.RemoveListener(OnBtn1Click);
       // Btn2.onClick.RemoveListener(OnBtn2Click);
    }
    private void OnBtn1Click()
    {
        //MessageBox.Show("设置开机自启动，需要修改注册表", "提示");
        try
        {
            string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            RegistryKey rgkRun = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rgkRun == null)
            {
                rgkRun = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            }
            rgkRun.SetValue("dhstest", path);
        }
        catch
        {
            Debug.Log(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
       
        }
         finally
        {
           // regeditkey();
          //  Debug.Log(123);
        }
    }

    private void OnBtn2Click()
    {
        //MessageBox.Show("取消开机自启动，需要修改注册表", "提示");
        try
        {
            string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            RegistryKey rgkRun = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
                true);
            if (rgkRun == null)
            {
                rgkRun = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            }
            rgkRun.DeleteValue("dhstest", false);
        }
        catch
        {
            Debug.Log("error");
        }
        finally
        {
            regeditkey();
            Debug.Log(123);
        }

    }




    public void regeditkey()
    {
        Debug.Log(123);
        RegistryKey rgkRun = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        if (rgkRun.GetValue("dhstest") == null)
        {
          //  text.text = "自启动为关闭";
        }
        else
        {
           // text.text = "自启动为打开";
            
        }
    }


}
 
