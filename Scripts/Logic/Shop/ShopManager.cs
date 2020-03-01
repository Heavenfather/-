using System;
using System.Collections;
using System.Collections.Generic;
using TableConfigs;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager
{
    private ShopManager() { }

    private static ShopManager _instance;
    public static ShopManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ShopManager();
        }
        return _instance;
    }

    /// <summary>
    /// 传入道具的id返回道具信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ShopBaseData FetchShopCellData(int id)
    {
        Table_ShopBase_DefineData data = TableManager.GetInstance().GetDataById("ShopBase", id) as Table_ShopBase_DefineData;
        
                ShopBaseData shopData = new ShopBaseData();
                shopData.ID = data.ID;
                shopData.CurrencyType = data.CurrencyType;
                shopData.IconID = data.IconID;
                shopData.Name = data.Name;
                shopData.Price = data.Price;
                shopData.ShopType = data.ShopType;

                return shopData;
    }

    /// <summary>
    /// 得到所有的商品数量
    /// </summary>
    /// <returns></returns>
    public int GetGoodsCount()
    {
        return GetAllGoodsIds().Count;
    }
    
    public List<int> GetAllGoodsIds()
    {
        List<int> ids = new List<int>();
        ids = TableManager.GetInstance().GetAllIds("ShopBase");
        return ids;
    }
    
    /// <summary>
    /// 是否拥有足够的货币
    /// </summary>
    /// <param name="type">货币类型</param>
    /// <param name="num">数量</param>
    /// <returns></returns>
    public bool IsEnoughMoney(int type,int num)
    {
        bool bResult = false;

        //1-金币 2-银币 3-令牌
        switch (type)
        {
            case 1:
                bResult = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount+"").Gold >= num;
                break;
            case 2:
                bResult = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").Coin >= num;
                break;
            case 3:
                bResult = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").LingPai >= num;
                break;
            default:
                break;
        }

        return bResult;
    }

    /// <summary>
    /// 更具类型更换货币的图片
    /// </summary>
    /// <param name="target"></param>
    /// <param name="type"></param>
    public void UpdateCurrencyTypeImg(Image target,int type)
    {
        string path = "UI/MainCity/";
        //1-金币  2-银币 3-令牌
        switch (type)
        {
            case 1:
                path += "yuanbao";
                break;
            case 2:
                path += "tongqian";
                break;
            case 3:
                path += "lingpai";
                break;
            default:
                break;
        }
        target.sprite = ResourcesMgr.GetInstance().LoadResource<Sprite>(path, true);
    }

    /// <summary>
    /// 更新金币的数量
    /// </summary>
    /// <param name="value">带上加减符号传数值进来</param>
    public void UpdateGoldNum(int value)
    {
        int oldNum = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").Gold;
        int newNum = 0;
        newNum = oldNum + value;
        //保存更改后的数值到本地
        GameDataManager.GetInstance().SaveUserGold(GameDataManager.UserAccount + "", newNum);
        //发送消息给界面更新数值
        KeyValueUpdate kv = new KeyValueUpdate(SysDefine.GoldNumChanged, newNum);
        MessageCenter.SendMessage(SysDefine.GoldNumChanged,kv);
    }
    /// <summary>
    /// 更新铜币的数量
    /// </summary>
    /// <param name="value">带上加减符号传数值进来</param>
    public void UpdateCoinNum(int value)
    {
        int oldNum = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").Coin;
        int newNum = 0;
        newNum = oldNum + value;
        //保存更改后的数值到本地
        GameDataManager.GetInstance().SaveUserCoin(GameDataManager.UserAccount + "", newNum);
        //发送消息给界面更新数值
        KeyValueUpdate kv = new KeyValueUpdate(SysDefine.CoinNumChanged, newNum);
        MessageCenter.SendMessage(SysDefine.CoinNumChanged, kv);
    }
    /// <summary>
    /// 更新令牌的数量
    /// </summary>
    /// <param name="value">带上加减符号传数值进来</param>
    public void UpdateLingpaiNum(int value)
    {
        int oldNum = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").LingPai;
        int newNum = 0;
        newNum = oldNum + value;
        //保存更改后的数值到本地
        GameDataManager.GetInstance().SaveUserLingPai(GameDataManager.UserAccount + "", newNum);
        //发送消息给界面更新数值
        KeyValueUpdate kv = new KeyValueUpdate(SysDefine.LingpaiNumChanged, newNum);
        MessageCenter.SendMessage(SysDefine.LingpaiNumChanged, kv);
    }

}
