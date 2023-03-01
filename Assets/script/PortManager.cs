using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PortManager : MonoBehaviour
{
    public static PortManager instance;

    #region 定义串口属性
    public Text gui;
    //public GUIText Test;
    //定义基本信息

    public int baudRate = 9600;//波特率
    public Parity parity = Parity.None;//效验位
    public int dataBits = 8;//数据位
    public StopBits stopBits = StopBits.One;//停止位
    SerialPort sp = null;
    Thread dataReceiveThread;
    //发送的消息
    string message = "";
    public List<byte> listReceive = new List<byte>();
    char[] strchar = new char[100];//接收的字符信息转换为字符数组信息
    public string receiveStr;//以字符串形式保存接收到的消息

    [Header("传入参数")]
    public double accelerationX;//横向加速度
    public double accelerationY;//纵向加速度
    public double lgy;
    public double lgp;
    public double bendDegree = 10;//弯曲度

    [Header("标准参数")]
    public double defaultBendDegree;
    #endregion
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }


        OpenPort();
        dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
        dataReceiveThread.Start();

    }
    void Update()
    {
        JudgeJumpOrFall(bendDegree);
    }

    #region 创建串口，并打开串口
    public void OpenPort()
    {
        string portName = "COM11";
        //Debug.Log("端口名：" + portName);
        //创建串口
        sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        sp.ReadTimeout = 40;

        try
        {
            sp.Open();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion

    #region 程序退出时关闭串口
    void OnApplicationQuit()
    {
        ClosePort();
    }
    public void ClosePort()
    {
        try
        {
            sp.Close();
            dataReceiveThread.Abort();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion

    #region 接收并打印数据
    void DataReceiveFunction()
    {
        #region 读取一行数据
        while (sp != null && sp.IsOpen)
        {
            Thread.Sleep(1);
            try
            {

                string str = sp.ReadLine();
                sp.DiscardInBuffer();
                receiveStr = str;//原始读入
                //Debug.Log(receiveStr);
                StringToFloat(str);
            }
            catch
            {

            }
        }
        #endregion
    }
    //将传入参数赋值给预设的变量
    public void StringToFloat(string strInput)
    {
        Regex reg = new Regex(@"[\-\d\.]+");
        MatchCollection mc = reg.Matches(strInput);//设定要查找的字符串
        for (int i = 0; i < mc.Count; i++)
        {
            double d = double.Parse(mc[i].Value);
            switch (i)
            {
                case 0:
                    accelerationX = d;
                    break;
                case 1:
                    accelerationY = d;
                    break;
                case 2:
                    lgy = d;
                    break;
                case 3:
                    lgp = d;
                    break;
                case 4:
                    bendDegree = d;
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region 数据处理

    #endregion

    #region 事件广播
    public void JudgeJumpOrFall(double bend)
    {
        //如果大于标准弯曲度，说明应该上跳
        if (bend > defaultBendDegree + 20)
        {
            EventCenter.Broadcast(EventType.jump);
        }

        if (bend < defaultBendDegree - 20)
        {
            EventCenter.Broadcast(EventType.down);
        }
    }
    #endregion
    
    #region 发送数据
    public void WriteData(string dataStr)
    {
        if (sp.IsOpen)
        {
            sp.Write(dataStr);
        }
    }
    #endregion
}
