using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 账号信息 存储所有登录过的账号信息
/// </summary>
[Serializable]
public class AccountData
{
    public Dictionary<int, UserData> accountData = new Dictionary<int, UserData>();

    public AccountData()
    {

    }
}
