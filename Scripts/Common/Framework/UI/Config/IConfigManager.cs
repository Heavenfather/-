using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用配置管理器接口
///   作用：基于“键值对”配置文件的通用解析
/// </summary>
public interface IConfigManager{
    /// <summary>
    /// 只读属性：应用设置
    /// 功能：得到键值对集合数据
    /// </summary>
    Dictionary<string ,string> AppSetting { get; }

    /// <summary>
    /// 得到配置文件（APPSetting）最大的数量
    /// </summary>
    /// <returns>数量</returns>
    int GetAppSettingMaxNumber();
}


[Serializable]
internal class KeyValuesInfo
{ 
    //配置信息     ConfigInfo这个名字必须和JSON配置文件里面的字段一样
    public List<KeyValuesNode> ConfigInfo = null;
}

[Serializable]
internal class KeyValuesNode
{
    /// <summary>
    /// 下面的字段必须要和JSON配置文件里面的字段保持一致，大小字母要区分
    /// </summary>
    //键
    public string Key = null;
    //值
    public string Value = null;
}
