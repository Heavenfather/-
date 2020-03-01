using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCellData
{
    //关卡ID
    public int ID;
    //关卡人物ID
    public int RoleID;
    //名称
    public string Title;
    //描述
    public string Description;
    //难度
    public int Difficulty;
    //奖励
    public string itemsID;
    //挑战所需等级
    public int NeedLevel;
    public int SceneID;
    public int index;

    public BattleCellData()
    {
        ID = 0;
        RoleID = 0;
        Title = "";
        Description = "";
        Difficulty = 0;
        itemsID = "";
        NeedLevel = 0;
        SceneID = 0;
        index = 0;
    }

}
