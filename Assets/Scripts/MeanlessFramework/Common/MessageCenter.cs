using UnityEngine;
using System.Collections;
using Meaningless;
using System.Collections.Generic;

/// <summary>
/// 广播中心
/// </summary>
public class MessageCenter
{
    public delegate void DelCallBack(params object[] obj);
    //存放参数监听的字典
    public static Dictionary<EMessageType, DelCallBack> dicMessageType = new Dictionary<EMessageType, DelCallBack>();


    /// <summary>
    /// 单参数-添加监听
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="handler"></param>
    public static void AddListener(EMessageType messageType, DelCallBack handler)
    {
        if (!dicMessageType.ContainsKey(messageType))
        {
            dicMessageType.Add(messageType, null);
        }
        dicMessageType[messageType] += handler;
    }

    /// <summary>
    ///  单参数-取消监听
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="handler"></param>
    public static void RemoveListener(EMessageType messageType, DelCallBack handler)
    {
        if (dicMessageType.ContainsKey(messageType))
        {
            dicMessageType[messageType] -= handler;
        }
    }

    /// <summary>
    /// 取消所有监听
    /// </summary>
    public static void RemoveAllListener()
    {
        dicMessageType.Clear();
       
    }

    /// <summary>
    /// 单参数-消息广播
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="obj"></param>
    public static void Send(EMessageType messageType,params object[] obj)
    {
        DelCallBack del;
        if (dicMessageType.TryGetValue(messageType, out del))
        {
            if (del != null)
            {
                del(obj);
            }
        }
    }

}
