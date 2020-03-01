using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息传递中心
///   用于UI窗体之间的数据传值
/// </summary>
public class MessageCenter
{
    public delegate void DelMessageDelivery(KeyValueUpdate kv);
    public delegate void DelMessageNoneParam(object[] args);

    /// <summary>
    /// 消息中心缓存集合   键：消息的类型  值：数据执行的委托
    /// </summary>
    public static Dictionary<string , DelMessageDelivery> DicMessage=new Dictionary<string, DelMessageDelivery>();

    public static Dictionary<string, DelMessageNoneParam> Messages = new Dictionary<string, DelMessageNoneParam>();

    /// <summary>
    /// 添加消息的监听
    /// </summary>
    /// <param name="messageType">消息类型</param>
    /// <param name="handler">消息委托</param>
    public static void AddMessageListener(string messageType, DelMessageDelivery handler)
    {
        if (!DicMessage.ContainsKey(messageType))
        {
            DicMessage.Add(messageType,null);
        }

        DicMessage[messageType] += handler;
    }

    public static void AddMessageListenerNoneParam(string messageType,DelMessageNoneParam handler)
    {
        if (!Messages.ContainsKey(messageType))
        {
            Messages.Add(messageType, null);
        }
        Messages[messageType] += handler;
    }

    /// <summary>
    /// 取消消息监听
    /// </summary>
    /// <param name="messageType">消息类型</param>
    /// <param name="handler">消息委托</param>
    public static void RemoveMessageListener(string messageType, DelMessageDelivery handler)
    {
        if (DicMessage.ContainsKey(messageType))
        {
            DicMessage[messageType] -= handler;
        }
    }

    public static void RemoveMessageListener(string messageType,DelMessageNoneParam handler)
    {
        if (Messages.ContainsKey(messageType))
        {
            Messages[messageType] -= handler;
        }
    }

    /// <summary>
    /// 清除所有的消息
    /// </summary>
    public static void ClearAllMessage()
    {
        if (DicMessage != null)
        {
            DicMessage.Clear();
            Messages.Clear();
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="messageType">消息类型</param>
    /// <param name="kv">键值对</param>
    public static void SendMessage(string messageType, KeyValueUpdate kv)
    {
        DelMessageDelivery del;

        DicMessage.TryGetValue(messageType, out del);
        if (del != null)
        {
            //调用委托
            del(kv);
        }
    }

    public static void SendMessage(string messageType,object[] args=null)
    {
        DelMessageNoneParam del;
        Messages.TryGetValue(messageType, out del);
        if (del != null)
        {
            del(args);
        }
    }
    
}
/// <summary>
/// 键值更新对
///     功能：配合委托，实现数据传递
/// </summary>
public class KeyValueUpdate
{
    private string _Key;        //键
    private object _Values;     //值


    public string Key
    {
        get { return _Key; }
    }

    public object Values
    {
        get { return _Values; }
    }

    public KeyValueUpdate(string key, object valuesObj)
    {
        _Key = key;
        _Values = valuesObj;
    }
}
