using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using UnityEngine.UI;

public class PortManager : MonoBehaviour
{
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
    string receiveStr;//���ַ�����ʽ������յ�����Ϣ
    #endregion
    void Start()
    {
        OpenPort();
        dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
        dataReceiveThread.Start();

    }
    void Update()
    {        

    }

    #region �������ڣ����򿪴���
    public void OpenPort()
    {
        string portName = "COM111";
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


    /// <summary>
    /// ��ӡ���յ���Ϣ
    /// </summary>
    void PrintData()
    {
        for (int i = 0; i < listReceive.Count; i++)
        {
            strchar[i] = (char)(listReceive[i]);
            receiveStr = new string(strchar);
        }
        Debug.Log(receiveStr);
    }

    #region ���ղ���ӡ����
    void DataReceiveFunction()
    {
        #region �������ֽڷ��ʹ�����Ϣ�����ܽ�������
        /*        while (sp != null && sp.IsOpen)
                {
                    Thread.Sleep(1);
                    try
                    {

                        byte addr = Convert.ToByte(sp.ReadByte());
                        sp.DiscardInBuffer();
                        listReceive.Add(addr);
                        PrintData();

                        WriteData("successfully receive data\n");
                    }
                    catch
                    {
                        listReceive.Clear();
                    }
                }*/
        #endregion

        #region ���ֽ����鷢�ʹ�����Ϣ����Ϣȱʧ
        /*        byte[] buffer = new byte[1024];
                int bytes = 0;
                while (true)
                {
                    if (sp != null && sp.IsOpen)
                    {
                        try
                        {
                            bytes = sp.Read(buffer, 0, buffer.Length);//�����ֽ�
                            if (bytes == 0)
                            {
                                continue;
                            }
                            else
                            {
                                string strbytes = Encoding.Default.GetString(buffer);
                                Debug.Log(strbytes);

                                WriteData("successfully receive data\n");
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.GetType() != typeof(ThreadAbortException))
                            {
                            }
                        }
                    }
                    Thread.Sleep(10);
                }*/
        #endregion

        #region ��ȡһ������
        while (sp!=null && sp.IsOpen)
        {
            Thread.Sleep(1);
            try
            {

                string str = sp.ReadLine();
                sp.DiscardInBuffer();
                receiveStr = str;
                PrintData();

                WriteData("successfully receive data\n");
            }
            catch
            {
                
            }
        }
        #endregion
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
