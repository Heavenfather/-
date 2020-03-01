using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户数据
/// </summary>
[Serializable]
public class UserData
{
    public int Id;                  //用户ID
    public string Name;             //用户名称
    public int Level;               //等级    
    public List<int> HaveCardIds;      //拥有的卡牌id
    public int Coin;               //银币
    public int Gold;               //金币
    public int LingPai;            //令牌
    public int CheckPoint;         //通关信息
    public int HeadId;            //头像ID  暂定有三个头像可以选择修改 对应1-3000101 2-3000105 3-3000108 
    public float SoundValue;        //音量
    public int Exp;               //经验
    public Dictionary<int, CardBaseData> CardData;  //卡牌的信息
    public Dictionary<int, ItemCellData> ItemKeyValue;       //道具信息 Key-道具ID Value-道具数量

    public UserData()
    {
        initData();
    }

    void initData()
    {
        Id = -1;
        Name = "无名大侠";
        Level = 1;
        HaveCardIds = new List<int>();
        Coin = 1000;        //初始给1000铜币和令牌
        Gold = 100;         //给100元宝
        LingPai = 1000;
        CheckPoint = 0;
        HeadId = 1;
        SoundValue = 0.5f;  //初始的音量为50%
        Exp = 0;
        CardData = new Dictionary<int, CardBaseData>();
        ItemKeyValue = new Dictionary<int, ItemCellData>();
    }

}
