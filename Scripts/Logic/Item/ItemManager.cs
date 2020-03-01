using System.Collections;
using System.Collections.Generic;
using TableConfigs;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager
{
    private ItemManager()
    {

    }

    private static ItemManager _instance;
    public static ItemManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ItemManager();
        }
        return _instance;
    }

    public ItemCellData createItemCellData(int itemid)
    {
        ItemCellData celldata = new ItemCellData();
        Table_ItemBase_DefineData data = TableManager.GetInstance().GetDataById("ItemBase", itemid) as Table_ItemBase_DefineData;
        celldata.ID = data.ID;
        celldata.Iconid = data.Iconid;
        celldata.Name = data.Name;
        celldata.quanlity = data.quanlity;
        celldata.Describe = data.Describe;           

        return celldata;
    }

    public void ReplaceQuanlityFrame(Image target,int quanlity)
    {
        string path = "UI/Common/";
        switch (quanlity)
        {
            case 1:
                path += "itemframewhile";
                break;
            case 2:
                path += "green";
                break;
            case 3: 
                path += "purple";
                break;
            default:
                break;
        }
        target.sprite = ResourcesMgr.GetInstance().LoadResource<Sprite>(path, true);

    }

    /// <summary>
    /// 更换Icon图片
    /// </summary>
    /// <param name="target"></param>
    /// <param name="iconid"></param>
    public void ReplaceItemIcon(Image target,int iconid)
    {
        string path = GetIconPath(iconid);

        target.sprite = ResourcesMgr.GetInstance().LoadResource<Sprite>(path, true);
    }

    string GetIconPath(int iconid)
    {
        string path = "UI/Icon/Item/";
        if (iconid >= 1001 && iconid <= 1005)       //1001-1005 货币
            path += "Money/" + iconid;
        else if (iconid >= 1010 && iconid <= 1019)      //1010-1019 宝石
            path += "Stone/" + iconid;

        return path;
    }

}
