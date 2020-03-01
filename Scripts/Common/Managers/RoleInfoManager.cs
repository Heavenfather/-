using System.Collections;
using System.Collections.Generic;
using TableConfigs;
using UnityEngine;

public class RoleInfoManager
{
    private RoleInfoManager()
    {

    }
    private static RoleInfoManager _instance;
    public static RoleInfoManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new RoleInfoManager();
        }
        return _instance;
    }

    /// <summary>
    /// 添加经验
    /// </summary>
    /// <param name="value"></param>
    public void AddExp(int value)
    {
        int oldExp = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").Exp;
        int oldLevel = GetLevel(oldExp);
        int newNum = oldExp + Mathf.Abs(value);
        int newLevel = GetLevel(newNum);
        GameDataManager.GetInstance().SaveUserExp(GameDataManager.UserAccount + "", newNum);
        //如果等级有发生变化发送信息给界面刷新
        if (newLevel != oldLevel)
        {
            //发送消息给界面更新数值
            KeyValueUpdate kv = new KeyValueUpdate(SysDefine.RoleLevelChanged, newLevel);
            MessageCenter.SendMessage(SysDefine.RoleLevelChanged, kv);
            //保存新等级到本地
            GameDataManager.GetInstance().SaveUserLevel(GameDataManager.UserAccount + "", newLevel);

            UIManager.GetInstance().ShowTip("角色升级");
        }
    }

    /// <summary>
    /// 得到用户的金币数量
    /// </summary>
    /// <returns></returns>
    public int GetRoleGoldNum()
    {
        return GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").Gold;
    }
    /// <summary>
    /// 得到用户的金币数量
    /// </summary>
    /// <returns></returns>
    public int GetRoleCoinNum()
    {
        return GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").Coin;
    }
    /// <summary>
    /// 得到用户的金币数量
    /// </summary>
    /// <returns></returns>
    public int GetRoleLingpaiNum()
    {
        return GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").LingPai;
    }

    /// <summary>
    /// 得到当前的经验
    /// </summary>
    /// <returns></returns>
    public int GetExp()
    {
        return GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").Exp;
    }

    /// <summary>
    /// 得到角色等级
    /// </summary>
    /// <returns></returns>
    public int GetRoleLevel()
    {
        return GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").Level;
    }

    /// <summary>
    /// 根据传进来的经验查表返回等级
    /// </summary>
    /// <param name="expNum"></param>
    /// <returns></returns>
    public int GetLevel(int expNum)
    {
        int result = 1;
        List<int> ids = TableManager.GetInstance().GetAllIds("RoleUpgradeExp");
        for (int i = 0; i < ids.Count; i++)
        {
            Table_RoleUpgradeExp_DefineData data = TableManager.GetInstance().GetDataById("RoleUpgradeExp", ids[i]) as Table_RoleUpgradeExp_DefineData;
            if (expNum >= data.NeedExp)
            {
                result = data.ID;   //表里面的ID就是等级
            }
        }
        return result;
    }

    /// <summary>
    /// 更新通关关卡
    /// </summary>
    public void UpdateCheckPoint(int point)
    {
        //保存通关信息
        GameDataManager.GetInstance().SaveUserCheckPoint(GameDataManager.UserAccount + "", point);
        //通知界面刷新
        MessageCenter.SendMessage(SysDefine.UpdateUserCheckPoint, new KeyValueUpdate(SysDefine.UpdateUserCheckPoint, point));
    }

    /// <summary>
    /// 清除等级和经验
    /// </summary>
    public void ClearExpAndLevel()
    {
        GameDataManager.GetInstance().SaveUserLevel(GameDataManager.UserAccount + "", 1);
        GameDataManager.GetInstance().SaveUserExp(GameDataManager.UserAccount + "", 0);
        //更新界面
        KeyValueUpdate kv = new KeyValueUpdate(SysDefine.RoleLevelChanged, 1);
        MessageCenter.SendMessage(SysDefine.RoleLevelChanged, kv);
    }

    /// <summary>
    /// 清除货币
    /// </summary>
    public void ClearCurrency()
    {
        GameDataManager.GetInstance().SaveUserGold(GameDataManager.UserAccount + "", 0);
        GameDataManager.GetInstance().SaveUserCoin(GameDataManager.UserAccount + "", 0);
        GameDataManager.GetInstance().SaveUserLingPai(GameDataManager.UserAccount + "", 0);

        //通知界面刷新
        MessageCenter.SendMessage(SysDefine.ClearCurrency, new KeyValueUpdate(SysDefine.ClearCurrency, 0));
    }
    /// <summary>
    /// 清除拥有的武将
    /// </summary>
    public void ClearCardIds()
    {
        List<int> reset = new List<int>();
        UserData data = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "");
        data.HaveCardIds = reset;
        LocalManager.Save<UserData>(GameDataManager.UserAccount + "", data);

        //通知界面刷新
        MessageCenter.SendMessage(SysDefine.HaveCardNumChanged, new KeyValueUpdate(SysDefine.HaveCardNumChanged,0));
    }
}
