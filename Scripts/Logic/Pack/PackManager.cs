using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackManager
{
    private PackManager() { }

    private static PackManager _instance;
    public static PackManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new PackManager();
        }
        return _instance;
    }


    /// <summary>
    /// 添加道具数量
    /// </summary>
    /// <param name="itemid">道具id</param>
    /// <param name="itemNum">道具数量</param>
    public void AddItemNum(int itemid, int itemNum)
    {
        if (itemNum < 0)
        {
            Debug.LogError("添加道具数量不允许为负数");
            return;
        }

        ItemCellData item;
        Dictionary<int, ItemCellData> itemKeyValue;
        itemKeyValue = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").ItemKeyValue;
        itemKeyValue.TryGetValue(itemid, out item);

        if (item != null)
        {
            item.showNum += itemNum;
        }
        else
        {
            //这是一个新道具
            item = new ItemCellData();
            item.ID = itemid;   //其它的信息应该用不着赋值，只要拿到id就可以查表那这个道具其它的值了
            item.showNum = itemNum;
            itemKeyValue.Add(itemid, item);
        }
        //保存到本地
        GameDataManager.GetInstance().SaveUserItem(GameDataManager.UserAccount + "", item);
        //可能需要发送一个事件
        MessageCenter.SendMessage(SysDefine.ItemNumChange, new KeyValueUpdate(SysDefine.ItemNumChange, item));
    }

    /// <summary>
    /// 减少道具数量
    /// </summary>
    /// <param name="itemid"></param>
    /// <param name="itemNum"></param>
    public void DecreaseItemNum(int itemid, int itemNum)
    {
        ItemCellData item;
        Dictionary<int, ItemCellData> itemKeyValue;
        itemKeyValue = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").ItemKeyValue;
        itemKeyValue.TryGetValue(itemid, out item);

        if (item == null)
        {
            Debug.LogError("没有该道具");
            return;
        }
        item.showNum -= itemNum;
        GameDataManager.GetInstance().SaveUserItem(GameDataManager.UserAccount + "", item);
        MessageCenter.SendMessage(SysDefine.ItemNumChange, new KeyValueUpdate(SysDefine.ItemNumChange, item));
    }
    /// <summary>
    /// 得到道具数量
    /// </summary>
    /// <param name="itemid"></param>
    /// <returns></returns>
    public int GetItemNum(int itemid)
    {
        ItemCellData item;
        Dictionary<int, ItemCellData> itemKeyValue;
        itemKeyValue = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").ItemKeyValue;
        itemKeyValue.TryGetValue(itemid, out item);
        if (item != null)
        {
            return item.showNum;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 得到背包中所有的道具ID
    /// </summary>
    /// <returns></returns>
    public List<int> GetAllPackItemId()
    {
        List<int> result = new List<int>();

        Dictionary<int, ItemCellData> items = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").ItemKeyValue;
        foreach (var item in items)
        {
            result.Add(item.Key);
        }

        return result;
    }
    /// <summary>
    /// 背包中是否有该道具
    /// </summary>
    /// <param name="itemid"></param>
    /// <returns></returns>
    public bool IsHaveItem(int itemid)
    {
        bool bResult = false;

        for (int i = 0; i < GetAllPackItemId().Count; i++)
        {
            if (GetAllPackItemId()[i] == itemid)
            {
                bResult = true;
                break;
            }
        }

        return bResult;
    }

    /// <summary>
    /// 判断单个道具是否充足
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="needNum"></param>
    /// <returns></returns>
    public bool ItemNumEnouthg(int itemId,int needNum)
    {
        int haveNum = GetItemNum(itemId);
        if (haveNum < needNum)
        {
            return false;
        }

        return true;
    }
}
