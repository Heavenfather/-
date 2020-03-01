using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 项目里面的数据操作 与LocalManager区分开来，LocalManager是通用的一个框架 GameDataManager是与项目相关的数据操作
/// </summary>
public class GameDataManager
{
    private GameDataManager()
    {
    }
    private static GameDataManager _instance;

    public static GameDataManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameDataManager();
        }
        return _instance;
    }

    public static int UserAccount = 0;   //当前登录的账号  只需要在登录的时候赋值一次就行了
    public static UserData UserData;    //用户信息

    public void Init()
    {

    }

    /// <summary>
    /// 加载本地的账户信息
    /// </summary>
    /// <returns></returns>
    public AccountData LoadAccountData()
    {
        AccountData data = LocalManager.Load<AccountData>("Account");
        if (data != null)
        {
            return data;
        }
        else
        {
            return new AccountData();
        }
    }

    /// <summary>
    /// 得到本地所有的账号
    /// </summary>
    /// <returns></returns>
    public List<int> GetAllAccount()
    {
        AccountData data = LoadAccountData();
        if (data.accountData.Count <= 0)
        {
            return new List<int>();
        }
        List<int> result = new List<int>();
        foreach (var item in data.accountData)
        {
            result.Add(item.Key);
        }
        return result;
    }

    /// <summary>
    /// 添加新的账号
    /// </summary>
    /// <param name="account"></param>
    public void AddNewAccount(int account)
    {
        AccountData data = LoadAccountData();
        if (IsHaveAccount(account))
        {
            UIManager.GetInstance().ShowTip("账号已存在!");
            return;
        }
        //是一个新账号
        UserData userData = new UserData();
        userData.Id = account;
        SaveUserId(account + "", account);
        data.accountData.Add(account, userData);
        LocalManager.Save<AccountData>("Account", data);
    }

    /// <summary>
    /// 得到指定账号的角色信息
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public UserData GetAccountUserData(int account)
    {
        UserData user;
        LoadAccountData().accountData.TryGetValue(account, out user);
        if (user == null)
        {
            return null;
        }
        user = LoadUserData(account + "");
        return user;
    }

    /// <summary>
    /// 是否已经存在该账号
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public bool IsHaveAccount(int account)
    {
        AccountData data = LoadAccountData();
        bool bResult = false;
        for (int i = 0; i < data.accountData.Count; i++)
        {
            if (data.accountData.ContainsKey(account))
            {
                bResult = true;
            }
        }
        return bResult;
    }

    /// <summary>
    /// 加载用户信息
    /// </summary>
    /// <param name="dataName">用户数据唯一标识，可用用户账号标记</param>
    /// <returns></returns>
    public UserData LoadUserData(string dataName)
    {
        UserData data = LocalManager.Load<UserData>(dataName);
        if (data != null)
        {
            return data;
        }
        else
        {
            return new UserData();
        }

    }

    /// <summary>
    /// 保存用户的Id  这里用账号标识为id
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserId(string dataName, int value)
    {
        UserData data = LoadUserData(dataName);
        data.Id = value;
        LocalManager.Save<UserData>(dataName, data);
    }

    /// <summary>
    /// 保存用户的银币
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserCoin(string dataName, int value)
    {
        UserData data = LoadUserData(dataName);
        data.Coin = value;
        LocalManager.Save<UserData>(dataName, data);
    }

    /// <summary>
    /// 保存用户的名称
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserName(string dataName, string value)
    {
        UserData data = LoadUserData(dataName);
        data.Name = value;
        LocalManager.Save<UserData>(dataName, data);
    }

    /// <summary>
    /// 保存用户的等级
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserLevel(string dataName, int value)
    {
        UserData data = LoadUserData(dataName);
        data.Level = value;
        LocalManager.Save<UserData>(dataName, data);
    }
    /// <summary>
    /// 保存用户的拥有的卡牌id
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserHaveCardIds(string dataName, int value)
    {
        UserData data = LoadUserData(dataName);
        data.HaveCardIds.Add(value);
        LocalManager.Save<UserData>(dataName, data);
    }
    /// <summary>
    /// 保存用户的金币
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserGold(string dataName, int value)
    {
        UserData data = LoadUserData(dataName);
        data.Gold = value;
        LocalManager.Save<UserData>(dataName, data);
    }
    /// <summary>
    /// 保存用户的令牌
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserLingPai(string dataName, int value)
    {
        UserData data = LoadUserData(dataName);
        data.LingPai = value;
        LocalManager.Save<UserData>(dataName, data);
    }
    /// <summary>
    /// 保存用户的通关信息
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserCheckPoint(string dataName, int value)
    {
        UserData data = LoadUserData(dataName);
        data.CheckPoint = value;
        LocalManager.Save<UserData>(dataName, data);
    }
    /// <summary>
    /// 保存用户的头像ID
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserHeadID(string dataName, int value)
    {
        UserData data = LoadUserData(dataName);
        data.HeadId = value;
        LocalManager.Save<UserData>(dataName, data);
    }
    /// <summary>
    /// 保存用户设置的音量
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserSoundValue(string dataName, float value)
    {
        UserData data = LoadUserData(dataName);
        data.SoundValue = value;
        LocalManager.Save<UserData>(dataName, data);
    }
    /// <summary>
    /// 保存用户经验
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="value"></param>
    public void SaveUserExp(string dataName, int value)
    {
        UserData data = LoadUserData(dataName);
        data.Exp = value;
        LocalManager.Save<UserData>(dataName, data);
    }

    /// <summary>
    /// 根据卡的ID返回存储的数据
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="cardid"></param>
    public CardBaseData GetCardData(string dataName,int cardid)
    {
        CardBaseData data;

        if (cardid <= 0)
        {
            return null;
        }

        UserData user = LoadUserData(dataName);
        user.CardData.TryGetValue(cardid, out data);

        if (data != null)
        {
            return data;
        }
        
        return new CardBaseData();
    }

    /// <summary>
    /// 保存卡牌的信息
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="cardid"></param>
    /// <param name="level">等级</param>
    public void SaveCardDataLevel(string dataName,int cardid,int level)
    {
        UserData userData = LoadUserData(dataName);
        CardBaseData baseCardData = GetCardData(dataName, cardid);

        if (baseCardData != null)
        {
            //之前有没有添加过
            if (baseCardData.BaseID > 0)
            {
                foreach (var data in userData.CardData)
                {
                   if (data.Key == cardid)
                   {
                      data.Value.Level = level;
                   }
                }
                
            }
            else
            {
                baseCardData.BaseID = cardid;
                baseCardData.Level = level;
                userData.CardData.Add(cardid, baseCardData);
            }
        }

        LocalManager.Save<UserData>(dataName, userData);
    }

    /// <summary>
    /// 保存用户的道具信息
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="item"></param>
    public void SaveUserItem(string dataName,ItemCellData item)
    {
        UserData userData = LoadUserData(dataName);
        ItemCellData itemKeyValue;
        userData.ItemKeyValue.TryGetValue(item.ID, out itemKeyValue);
        if (itemKeyValue != null)
        {
            //之前已经有存该ID的道具
            //itemKeyValue = item;
            foreach (var cellItem in userData.ItemKeyValue)
            {
                if (cellItem.Key == item.ID)
                {
                    cellItem.Value.showNum = item.showNum;
                    break;
                }
            }
        }
        else
        {
            //添加新的道具
            userData.ItemKeyValue.Add(item.ID, item);
        }
        LocalManager.Save<UserData>(dataName, userData);
    }
}
