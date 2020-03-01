using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基于JSON配置文件的配置管理器
/// </summary>
public class ConfigManagerByJson : IConfigManager
{
    //保存键值对集合
    private static Dictionary<string, string> _AppSetting;
    /// <summary>
    /// 只读属性：得到应用设置（键值对集合）
    /// </summary>
    public Dictionary<string, string> AppSetting
    {
        get
        {
            return _AppSetting;
        }
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="jsonPath">需要加载的json文件名</param>
    public ConfigManagerByJson(string jsonPath)
    {
        _AppSetting=new Dictionary<string, string>();
        //初始化解析JSON数据，加载到集合中
        InitAndAnalyJson(jsonPath);
    }

    /// <summary>
    /// 得到APPSetting的最大数值
    /// </summary>
    /// <returns></returns>
    public int GetAppSettingMaxNumber()
    {
        if (_AppSetting != null&&_AppSetting.Count>=1)
        {
            return _AppSetting.Count;
        }
        else
        {
            return 0;
        }
        
    }

    /// <summary>
    /// 初始化解析JSON数据，加载到集合中
    /// </summary>
    /// <param name="jsonPath">Json文件路径</param>
    private void InitAndAnalyJson(string jsonPath)
    {
        TextAsset configInfo = null;
        KeyValuesInfo keyValuesInfo = null;

        //参数检查
        if (string.IsNullOrEmpty(jsonPath))
            return;

        //解析JSON配置文件
        try
        {
            configInfo = Resources.Load<TextAsset>(jsonPath);
            keyValuesInfo = JsonUtility.FromJson<KeyValuesInfo>(configInfo.text);

        }
        catch
        {
            //抛出异常
            throw new JsonAnalysisException(GetType()+ "InitAndAnalyJson/Json解析异常，请检查!JsonPath="+jsonPath);

        }
        //把数据加载到AppSetting集合中
        foreach (KeyValuesNode nodeInfo in keyValuesInfo.ConfigInfo)
        {
            _AppSetting.Add(nodeInfo.Key,nodeInfo.Value);
            
        }

    }
}
