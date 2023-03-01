using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter
{
    //将委托和事件类型相对应
    private static Dictionary<EventType, Delegate> eventTable = new Dictionary<EventType, Delegate>();

    #region 添加订阅
    public static void OnListenerAdding(EventType eventType ,Delegate d)
    {
        //如果事件——委托字典里不存在这个事件类型，就将他添加
        if (!eventTable.ContainsKey(eventType))
        {
            eventTable.Add(eventType, d);
            //Debug.Log("添加事件");
        }

        //如果有这个事件，那就判断传入的第二个参数callback是否是一个委托类型的变量
        Delegate delegate_Temp = eventTable[eventType];
        if (delegate_Temp != null && delegate_Temp.GetType() != d.GetType())
        {
            throw new Exception(string.Format("添加监听错误：尝试为事件{0}添加不同类型的委托。当前事件对应的委托类型应为{1}。",
                eventType, delegate_Temp.GetType()));
        }
    }
    //无参添加订阅
    public static void AddListener(EventType eventType, Callback callback)
    {
        OnListenerAdding(eventType, callback);

        //如果可以建立委托，那就添加订阅事件
        eventTable[eventType] = (Callback)eventTable[eventType] + callback;
    }
    //有一个参数的订阅
    public static void AddListener<T>(EventType eventType, Callback<T> callback)
    {
        OnListenerAdding(eventType, callback);

        //如果可以建立委托，那就添加订阅事件
        eventTable[eventType] = (Callback<T>)eventTable[eventType] + callback;
    }
    //有两个参数的订阅
    public static void AddListener<T,X>(EventType eventType, Callback<T,X> callback)
    {
        OnListenerAdding(eventType, callback);

        //如果可以建立委托，那就添加订阅事件
        eventTable[eventType] = (Callback<T, X>)eventTable[eventType] + callback;
    }
    //有三个参数的订阅
    public static void AddListener<T, X, Y>(EventType eventType, Callback<T, X, Y> callback)
    {
        OnListenerAdding(eventType, callback);

        //如果可以建立委托，那就添加订阅事件
        eventTable[eventType] = (Callback<T, X, Y>)eventTable[eventType] + callback;
    }
    //有四个参数的订阅
    public static void AddListener<T, X, Y, Z>(EventType eventType, Callback<T, X, Y, Z> callback)
    {
        OnListenerAdding(eventType, callback);

        //如果可以建立委托，那就添加订阅事件
        eventTable[eventType] = (Callback<T, X, Y, Z>)eventTable[eventType] + callback;
    }
    #endregion

    #region 移除订阅
    private static void OnListenerRemoving(EventType eventType, Delegate d)
    {
        //如果事件——委托字典里不存在这个事件类型，就将他添加
        if (eventTable.ContainsKey(eventType))
        {
            Delegate delegate_Temp = eventTable[eventType];
            if (delegate_Temp == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应委托",
                    eventType));
            }
            else if (delegate_Temp.GetType() != d.GetType())
            {
                throw new Exception(string.Format("移除监听错误：事件{0}对应的委托类型应为{1}，而不是{2}",
                    eventType, delegate_Temp.GetType(), d.GetType()));
            }

        }
        else
        {
            throw new Exception(string.Format("移除监听错误：事件{0}没有对应事件码",
                    eventType));
        }
    }
    private static void OnListenerRemoved(EventType eventType)
    {
        //将事件码从字典中移除，使得下次能够成功添加事件
        if (eventTable[eventType] == null)
        {
            eventTable.Remove(eventType);
        }
    }
    //无参移除订阅
    public static void RemoveListener(EventType eventType, Callback callback)
    {
        OnListenerRemoving(eventType, callback);
        //移除监听
        eventTable[eventType] = (Callback)eventTable[eventType] - callback;
        OnListenerRemoved(eventType);

    }
    //有一个参数的移除监听
    public static void RemoveListener<T>(EventType eventType, Callback<T> callback)
    {
        OnListenerRemoving(eventType, callback);
        //移除监听
        eventTable[eventType] = (Callback<T>)eventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    //有两个参数的移除监听
    public static void RemoveListener<T, X>(EventType eventType, Callback<T, X> callback)
    {
        OnListenerRemoving(eventType, callback);
        //移除监听
        eventTable[eventType] = (Callback<T, X>)eventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    //有三个参数的移除监听
    public static void RemoveListener<T, X, Y>(EventType eventType, Callback<T, X, Y> callback)
    {
        OnListenerRemoving(eventType, callback);
        //移除监听
        eventTable[eventType] = (Callback<T, X, Y>)eventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    //有四个参数的移除监听
    public static void RemoveListener<T, X, Y, Z>(EventType eventType, Callback<T, X, Y, Z> callback)
    {
        OnListenerRemoving(eventType, callback);
        //移除监听
        eventTable[eventType] = (Callback<T, X, Y, Z>)eventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }
    #endregion

    #region 广播监听，对事件进行相应。注意：当一个事件码已经有对应的委托，想要为它添加另一个委托时，就要先清除原来的委托。
    public static void Broadcast(EventType eventType)
    {
        //调用传入事件所对应的委托
        Delegate delegate_temp;
        if (eventTable.TryGetValue(eventType, out delegate_temp))
        {
            Callback callback = delegate_temp as Callback;
            callback();
        }

    }
    //有一个参数的广播监听.
    public static void Broadcast<T>(EventType eventType, T arg)
    {
        //调用传入事件所对应的委托
        Delegate delegate_temp;
        if (eventTable.TryGetValue(eventType, out delegate_temp))
        {
            Callback<T> callback = delegate_temp as Callback<T>;
            callback(arg);
        }
    }
    //有两个参数的广播监听.
    public static void Broadcast<T, X>(EventType eventType, T arg1, X arg2)
    {
        //调用传入事件所对应的委托
        Delegate delegate_temp;
        if (eventTable.TryGetValue(eventType, out delegate_temp))
        {
            Callback<T, X> callback = delegate_temp as Callback<T, X>;
            callback(arg1,arg2);
        }
    }
    //有三个参数的广播监听.
    public static void Broadcast<T, X, Y>(EventType eventType, T arg1, X arg2, Y arg3)
    {
        //调用传入事件所对应的委托
        Delegate delegate_temp;
        if (eventTable.TryGetValue(eventType, out delegate_temp))
        {
            Callback<T, X, Y> callback = delegate_temp as Callback<T, X, Y>;
            callback(arg1, arg2, arg3);
        }
    }
    //有四个参数的广播监听.
    public static void Broadcast<T, X, Y, Z>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4)
    {
        //调用传入事件所对应的委托
        Delegate delegate_temp;
        if (eventTable.TryGetValue(eventType, out delegate_temp))
        {
            Callback<T, X, Y, Z> callback = delegate_temp as Callback<T, X, Y, Z>;
            callback(arg1, arg2, arg3, arg4);
        }
    }
    #endregion



}
