using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TableConfigs;
using System;

public class CheckPointManager
{
    private static CheckPointManager _instance;
    private CheckPointManager()
    {

    }
    public static CheckPointManager GetInstance()
    {
        if (_instance==null)
        {
            _instance = new CheckPointManager();            
        }
        return _instance;
    }

    public BattleCellData createCheckPointData(int pointid)
    {
        Table_CheckPoints_DefineData tabledata = TableManager.GetInstance().GetDataById("CheckPoints", pointid) as Table_CheckPoints_DefineData;
        BattleCellData data = new BattleCellData();
        //foreach (var item in datas.mDataMap)
        //{
        //    if (item.Key == pointid.ToString())
        //    {
                data.ID = tabledata.ID;
                data.itemsID = tabledata.itemsID;
                data.NeedLevel = tabledata.NeedLevel;
                data.RoleID = tabledata.RoleID;
                data.Title = tabledata.Title;
                data.Description = tabledata.Description;
                data.Difficulty = tabledata.Difficulty;
                data.SceneID = tabledata.LevelID;
                //break;
        //    }
        //}

        return data;
    }

    //得到关卡总id
    public List<int> GetCheckPointIds()
    {
        List<int> ids = new List<int>();

        ids = TableManager.GetInstance().GetAllIds("CheckPoints");

        return ids;
    }

    /// <summary>
    /// 得到关卡的总数量
    /// </summary>
    /// <returns></returns>
    public int GetCheckPointCount()
    {
        return GetCheckPointIds().Count;
    }

    /// <summary>
    /// 得到通关奖励的道具或货币 格式：道具id1:道具1数量;道具id2:道具2数量;....
    /// </summary>
    /// <param name="checkpointId"></param>
    /// <returns></returns>
    public string GetCheckPointReward(int checkpointId)
    {
        string result = createCheckPointData(checkpointId).itemsID;
        return result;
    }

    /// <summary>
    /// 等级是否达到挑战要求
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public bool IsEnoughChanllenge(int checkpoint)
    {
        bool bResult = false;

        int needLevel = createCheckPointData(checkpoint).NeedLevel;
        bResult = RoleInfoManager.GetInstance().GetRoleLevel() >= needLevel;

        return bResult;
    }
}
