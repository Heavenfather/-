using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TableConfigs;
using System;

public class CardManager
{
    private static CardManager _instance;
    private CardManager()
    {

    }

    public static CardManager GetInstance()
    {
        if (_instance==null)
        {
            _instance = new CardManager();
        }
        return _instance;
    }

    /// <summary>
    /// 更换Icon图片
    /// </summary>
    /// <param name="target">目标图片</param>
    /// <param name="iconid">iconid</param>
    public void ReplaceIcon(Image target,int iconid)
    {
        if (iconid <= 0)
            return;

        string path = "UI/Icon/";

        if(iconid>=331001 && iconid <= 331050)  //成就卡牌
        {
            path += "Card/Achievement/" + iconid.ToString();
        }
        else if(iconid>=3000101 && iconid <= 3100500)   //武将卡牌
        {
            path += "Card/Role/" + iconid.ToString();
        }
        else if(iconid>=8100102 && iconid <= 8100500)   //怪物卡牌
        {
            path += "Card/Role/" + iconid.ToString();
        }
        
        target.sprite = ResourcesMgr.GetInstance().LoadResource<Sprite>(path, true);
    }

    /// <summary>
    /// 根据角色ID更换大图
    /// </summary>
    /// <param name="role"></param>
    /// <param name="id"></param>
    public void ReplaceCardRole(Image role,int id)
    {
        string path = "UI/Role/CardBig/";
        if(id>=3000101 && id <= 3000200)
        {
            path += id.ToString();
        }
        role.sprite = ResourcesMgr.GetInstance().LoadResource<Sprite>(path, true);
    }

    /// <summary>
    /// 根据属性id得到属性名字
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetAttributeNameById(int id)
    {
        string strResult = "";
        Table_AttributeBase_DefineData data = TableManager.GetInstance().GetDataById("AttributeBase", id) as Table_AttributeBase_DefineData;

        strResult = data.Name;

        return strResult;
    }

    /// <summary>
    /// 根据属性id返回值类型
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public string GetAttributeNum(int id,int num)
    {
        string strResult = "";
        Table_AttributeBase_DefineData data = TableManager.GetInstance().GetDataById("AttributeBase", id) as Table_AttributeBase_DefineData;
                if (data.Valuetype == 1)
                {
                    strResult = num.ToString();
                }else if (data.Valuetype == 2)
                {
                    strResult = num / 100 + "%";
                }
        return strResult;
    }

    /// <summary>
    /// 得到属性描述
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetAttributeDes(int id)
    {
        string strResult = "";
        
        Table_AttributeBase_DefineData data = TableManager.GetInstance().GetDataById("AttributeBase", id) as Table_AttributeBase_DefineData;
        strResult = data.AttributeDes;

        return strResult;
    }

    public string GetAttribute(int cardId)
    {
        Table_WujiangCarBase_DefineData data = TableManager.GetInstance().GetDataById("WujiangCarBase", cardId) as Table_WujiangCarBase_DefineData;

        return data.Attribute;
    }

    /// <summary>
    /// 根据品质更换名字栏背景框
    /// </summary>
    /// <param name="quanlity"></param>
    public void ReplaceNameFrameByQuanlity(Image frame,int quanlity)
    {
        string path = "UI/Frame/CardFrame/";
        switch (quanlity)
        {
            case 1:
                path += "nameframe1";
                break;
            case 2:
                path += "nameframe3";
                break;
            case 3:
                path += "nameframe5";
                break;
            case 4:
                path += "nameframe7";
                break;
            case 5:
                path += "nameframe9";
                break;
            default:
                break;
        }
        frame.sprite = ResourcesMgr.GetInstance().LoadResource<Sprite>(path, true);
    }

    /// <summary>
    /// 根据品质更换背景框
    /// </summary>
    /// <param name="quanlity"></param>
    public void ReplaceBgFrameByQuanlity(Image frame,int quanlity)
    {
        string path = "UI/Common/";
        switch (quanlity)
        {
            case 1:
                path += "itemframewhile";
                break;
            case 2:
                path += "blue";
                break;
            case 3:
                path += "itemframe";
                break;
            case 4:
                path += "purple";
                break;
            case 5:
                path += "itemframeorige";
                break;
            default:
                break;
        }
        frame.sprite = ResourcesMgr.GetInstance().LoadResource<Sprite>(path, true);
    }

    /// <summary>
    /// 根据id返回武将卡的数据
    /// </summary>
    /// <param name="id"></param>
    public WujiangData FetchWujiangCardDataById(int id)
    {
        Table_WujiangCarBase_DefineData data = TableManager.GetInstance().GetDataById("WujiangCarBase", id) as Table_WujiangCarBase_DefineData;
                var wujiangData = new WujiangData();
                wujiangData.ID = data.ID;
                wujiangData.IconId = data.IconId;
                wujiangData.Attribute = data.Attribute;
                wujiangData.Description = data.Description;
                wujiangData.Name = data.Name;
                wujiangData.Star = data.Star;
                wujiangData.Quantity = data.Quantity;

                return wujiangData;
    }
    /// <summary>
    /// 得到表里的总数
    /// </summary>
    /// <returns></returns>
    public int GetWujiangCardCount()
    {
        return TableManager.GetInstance().GetAllIds("WujiangCarBase").Count;
    }

    public List<int> GetWujiangCardIds()
    {
        List<int> ids = new List<int>();
        ids = TableManager.GetInstance().GetAllIds("WujiangCarBase");

        return ids;

    }

    /// <summary>
    /// 根据id得到武将的品质
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetCardQuanlity(int id)
    {
        int quanlity = 0;
        Table_WujiangCarBase_DefineData data = TableManager.GetInstance().GetDataById("WujiangCarBase", id) as Table_WujiangCarBase_DefineData;
        quanlity = data.Quantity;

        return quanlity;
    }

    //转换头像ID 1-3000101 2-3000105 3-3000108 
    public int TransitionHeadID(int headid)
    {
        int result = 0;
        switch (headid)
        {
            case 1:
                result = 3000101;
                break;
            case 2:
                result = 3000105;
                break;
            case 3:
                result = 3000108;
                break;
            default:
                break;
        }
        return result;
    }    

    /// <summary>
    /// 拥有的卡牌集合
    /// </summary>
    /// <returns></returns>
    public List<int> HaveCardIds()
    {
        List<int> ids = new List<int>();
        ids = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").HaveCardIds;
        
        return ids;
    }

    /// <summary>
    /// 添加新拥有的卡牌保存到本地
    /// </summary>
    /// <param name="cardid"></param>
    public void AddNewCard(int cardid)
    {
        GameDataManager.GetInstance().SaveUserHaveCardIds(GameDataManager.UserAccount + "", cardid);
        MessageCenter.SendMessage(SysDefine.HaveCardNumChanged, new KeyValueUpdate(SysDefine.HaveCardNumChanged, HaveCardIds().Count));
    }

    /// <summary>
    /// 添加卡牌数据
    /// </summary>
    /// <param name="dataName"></param>
    /// <param name="data"></param>
    public void AddCardData(CardBaseData data)
    {
        UserData userData = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount+"");
        userData.CardData.Add(data.BaseID, data);
        LocalManager.Save<UserData>(GameDataManager.UserAccount+"", userData);
    }

    /// <summary>
    /// 得到保存在本地卡的等级
    /// </summary>
    /// <param name="cardid"></param>
    /// <returns></returns>
    public int GetSaveCardLevel(int cardid)
    {
        CardBaseData data = GameDataManager.GetInstance().GetCardData(GameDataManager.UserAccount + "", cardid);

        return data.Level;
    }

    /// <summary>
    /// 升级某张指定卡牌
    /// </summary>
    /// <param name="cardId"></param>
    /// <param name="upLevel"></param>
    public void UpgradeCardLevel(int cardId,int upLevel)
    {
        GameDataManager.GetInstance().SaveCardDataLevel(GameDataManager.UserAccount + "", cardId, upLevel);
        object[] objs = new object[2];
        objs[0] = cardId;
        objs[1] = upLevel;
        //Dictionary<int, int> value = new Dictionary<int, int>();
        //value.Add(cardId, upLevel);
        MessageCenter.SendMessage(SysDefine.CardUpgrade, new KeyValueUpdate(SysDefine.CardUpgrade, objs));

    }

    /// <summary>
    /// 根据卡牌ID得到升级所需要的材料
    /// </summary>
    /// <param name="cardid"></param>
    /// <returns>字符串</returns>
    public string GetComsumeStuffs(int cardid)
    {
        Table_WujiangUpgrade_DefineData data = TableManager.GetInstance().GetDataById("WujiangUpgrade", cardid) as Table_WujiangUpgrade_DefineData;
        return data.Stuff;
    }

}
