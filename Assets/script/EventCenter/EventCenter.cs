using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter
{
    //��ί�к��¼��������Ӧ
    private static Dictionary<EventType, Delegate> eventTable = new Dictionary<EventType, Delegate>();

    #region ��Ӷ���
    public static void OnListenerAdding(EventType eventType ,Delegate d)
    {
        //����¼�����ί���ֵ��ﲻ��������¼����ͣ��ͽ������
        if (!eventTable.ContainsKey(eventType))
        {
            eventTable.Add(eventType, d);
            //Debug.Log("����¼�");
        }

        //���������¼����Ǿ��жϴ���ĵڶ�������callback�Ƿ���һ��ί�����͵ı���
        Delegate delegate_Temp = eventTable[eventType];
        if (delegate_Temp != null && delegate_Temp.GetType() != d.GetType())
        {
            throw new Exception(string.Format("��Ӽ������󣺳���Ϊ�¼�{0}��Ӳ�ͬ���͵�ί�С���ǰ�¼���Ӧ��ί������ӦΪ{1}��",
                eventType, delegate_Temp.GetType()));
        }
    }
    //�޲���Ӷ���
    public static void AddListener(EventType eventType, Callback callback)
    {
        OnListenerAdding(eventType, callback);

        //������Խ���ί�У��Ǿ���Ӷ����¼�
        eventTable[eventType] = (Callback)eventTable[eventType] + callback;
    }
    //��һ�������Ķ���
    public static void AddListener<T>(EventType eventType, Callback<T> callback)
    {
        OnListenerAdding(eventType, callback);

        //������Խ���ί�У��Ǿ���Ӷ����¼�
        eventTable[eventType] = (Callback<T>)eventTable[eventType] + callback;
    }
    //�����������Ķ���
    public static void AddListener<T,X>(EventType eventType, Callback<T,X> callback)
    {
        OnListenerAdding(eventType, callback);

        //������Խ���ί�У��Ǿ���Ӷ����¼�
        eventTable[eventType] = (Callback<T, X>)eventTable[eventType] + callback;
    }
    //�����������Ķ���
    public static void AddListener<T, X, Y>(EventType eventType, Callback<T, X, Y> callback)
    {
        OnListenerAdding(eventType, callback);

        //������Խ���ί�У��Ǿ���Ӷ����¼�
        eventTable[eventType] = (Callback<T, X, Y>)eventTable[eventType] + callback;
    }
    //���ĸ������Ķ���
    public static void AddListener<T, X, Y, Z>(EventType eventType, Callback<T, X, Y, Z> callback)
    {
        OnListenerAdding(eventType, callback);

        //������Խ���ί�У��Ǿ���Ӷ����¼�
        eventTable[eventType] = (Callback<T, X, Y, Z>)eventTable[eventType] + callback;
    }
    #endregion

    #region �Ƴ�����
    private static void OnListenerRemoving(EventType eventType, Delegate d)
    {
        //����¼�����ί���ֵ��ﲻ��������¼����ͣ��ͽ������
        if (eventTable.ContainsKey(eventType))
        {
            Delegate delegate_Temp = eventTable[eventType];
            if (delegate_Temp == null)
            {
                throw new Exception(string.Format("�Ƴ����������¼�{0}û�ж�Ӧί��",
                    eventType));
            }
            else if (delegate_Temp.GetType() != d.GetType())
            {
                throw new Exception(string.Format("�Ƴ����������¼�{0}��Ӧ��ί������ӦΪ{1}��������{2}",
                    eventType, delegate_Temp.GetType(), d.GetType()));
            }

        }
        else
        {
            throw new Exception(string.Format("�Ƴ����������¼�{0}û�ж�Ӧ�¼���",
                    eventType));
        }
    }
    private static void OnListenerRemoved(EventType eventType)
    {
        //���¼�����ֵ����Ƴ���ʹ���´��ܹ��ɹ�����¼�
        if (eventTable[eventType] == null)
        {
            eventTable.Remove(eventType);
        }
    }
    //�޲��Ƴ�����
    public static void RemoveListener(EventType eventType, Callback callback)
    {
        OnListenerRemoving(eventType, callback);
        //�Ƴ�����
        eventTable[eventType] = (Callback)eventTable[eventType] - callback;
        OnListenerRemoved(eventType);

    }
    //��һ���������Ƴ�����
    public static void RemoveListener<T>(EventType eventType, Callback<T> callback)
    {
        OnListenerRemoving(eventType, callback);
        //�Ƴ�����
        eventTable[eventType] = (Callback<T>)eventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    //�������������Ƴ�����
    public static void RemoveListener<T, X>(EventType eventType, Callback<T, X> callback)
    {
        OnListenerRemoving(eventType, callback);
        //�Ƴ�����
        eventTable[eventType] = (Callback<T, X>)eventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    //�������������Ƴ�����
    public static void RemoveListener<T, X, Y>(EventType eventType, Callback<T, X, Y> callback)
    {
        OnListenerRemoving(eventType, callback);
        //�Ƴ�����
        eventTable[eventType] = (Callback<T, X, Y>)eventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    //���ĸ��������Ƴ�����
    public static void RemoveListener<T, X, Y, Z>(EventType eventType, Callback<T, X, Y, Z> callback)
    {
        OnListenerRemoving(eventType, callback);
        //�Ƴ�����
        eventTable[eventType] = (Callback<T, X, Y, Z>)eventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    #endregion

    #region �㲥���������¼�������Ӧ��ע�⣺��һ���¼����Ѿ��ж�Ӧ��ί�У���ҪΪ�������һ��ί��ʱ����Ҫ�����ԭ����ί�С�
    public static void Broadcast(EventType eventType)
    {
        //���ô����¼�����Ӧ��ί��
        Delegate delegate_temp;
        if (eventTable.TryGetValue(eventType, out delegate_temp))
        {
            Callback callback = delegate_temp as Callback;
            callback();
        }

    }
    //��һ�������Ĺ㲥����.
    public static void Broadcast<T>(EventType eventType, T arg)
    {
        //���ô����¼�����Ӧ��ί��
        Delegate delegate_temp;
        if (eventTable.TryGetValue(eventType, out delegate_temp))
        {
            Callback<T> callback = delegate_temp as Callback<T>;
            callback(arg);
        }
    }
    //�����������Ĺ㲥����.
    public static void Broadcast<T, X>(EventType eventType, T arg1, X arg2)
    {
        //���ô����¼�����Ӧ��ί��
        Delegate delegate_temp;
        if (eventTable.TryGetValue(eventType, out delegate_temp))
        {
            Callback<T, X> callback = delegate_temp as Callback<T, X>;
            callback(arg1,arg2);
        }
    }
    //�����������Ĺ㲥����.
    public static void Broadcast<T, X, Y>(EventType eventType, T arg1, X arg2, Y arg3)
    {
        //���ô����¼�����Ӧ��ί��
        Delegate delegate_temp;
        if (eventTable.TryGetValue(eventType, out delegate_temp))
        {
            Callback<T, X, Y> callback = delegate_temp as Callback<T, X, Y>;
            callback(arg1, arg2, arg3);
        }
    }
    //���ĸ������Ĺ㲥����.
    public static void Broadcast<T, X, Y, Z>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4)
    {
        //���ô����¼�����Ӧ��ί��
        Delegate delegate_temp;
        if (eventTable.TryGetValue(eventType, out delegate_temp))
        {
            Callback<T, X, Y, Z> callback = delegate_temp as Callback<T, X, Y, Z>;
            callback(arg1, arg2, arg3, arg4);
        }
    }
    #endregion



}
