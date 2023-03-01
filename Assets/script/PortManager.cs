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

    #region ���崮������
    public Text gui;
    //public GUIText Test;
    //���������Ϣ

    public int baudRate = 9600;//������
    public Parity parity = Parity.None;//Ч��λ
    public int dataBits = 8;//����λ
    public StopBits stopBits = StopBits.One;//ֹͣλ
    SerialPort sp = null;
    Thread dataReceiveThread;
    //���͵���Ϣ
    string message = "";
    public List<byte> listReceive = new List<byte>();
    char[] strchar = new char[100];//���յ��ַ���Ϣת��Ϊ�ַ�������Ϣ
    public string receiveStr;//���ַ�����ʽ������յ�����Ϣ

    [Header("�������")]
    public double accelerationX;//������ٶ�
    public double accelerationY;//������ٶ�
    public double lgy;
    public double lgp;
    public double bendDegree = 10;//������

    [Header("��׼����")]
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

    #region �������ڣ����򿪴���
    public void OpenPort()
    {
        string portName = "COM11";
        //Debug.Log("�˿�����" + portName);
        //��������
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

    #region �����˳�ʱ�رմ���
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

    #region ���ղ���ӡ����
    void DataReceiveFunction()
    {
        #region ��ȡһ������
        while (sp != null && sp.IsOpen)
        {
            Thread.Sleep(1);
            try
            {

                string str = sp.ReadLine();
                sp.DiscardInBuffer();
                receiveStr = str;//ԭʼ����
                //Debug.Log(receiveStr);
                StringToFloat(str);
            }
            catch
            {

            }
        }
        #endregion
    }
    //�����������ֵ��Ԥ��ı���
    public void StringToFloat(string strInput)
    {
        Regex reg = new Regex(@"[\-\d\.]+");
        MatchCollection mc = reg.Matches(strInput);//�趨Ҫ���ҵ��ַ���
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

    #region ���ݴ���

    #endregion

    #region �¼��㲥
    public void JudgeJumpOrFall(double bend)
    {
        //������ڱ�׼�����ȣ�˵��Ӧ������
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
    
    #region ��������
    public void WriteData(string dataStr)
    {
        if (sp.IsOpen)
        {
            sp.Write(dataStr);
        }
    }
    #endregion
}
