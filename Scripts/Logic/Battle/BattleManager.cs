using System;
using System.Collections;
using System.Collections.Generic;
using TableConfigs;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager
{
    private static BattleManager _instance;
    private BattleManager()
    {
        Init();
        MessageCenter.AddMessageListener(SysDefine.RoleLevelChanged, onRoleLevelChange);
    }
    public static BattleManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BattleManager();
        }
        return _instance;
    }

    public Dictionary<int, WujiangData> dicIntoBattleCard = new Dictionary<int, WujiangData>();
    public List<int> cardIds = new List<int>();
    public static int EntryCheckPointTag = 0;   //当前进入的关卡标签
    public static int EntrySceneid = 0;   //当前进入的关卡场景id

    private int _UseCardTime = 3;   //使用卡牌的次数
    private int _MaxCreateSoldierCount = 10;
    private int _MaxUseCardTime = 5;
    private int _aliveEnemys;
    private int _alivePlayer;
    private bool _isStartBattle=false;        //是否开始战斗

    public int UseCardTime { get => _UseCardTime;}
    public int AliveEnemys { get => _aliveEnemys; set => _aliveEnemys = value; }
    public int AlivePlayer { get => _alivePlayer; set => _alivePlayer = value; }
    public bool IsStartBattle { get => _isStartBattle; set => _isStartBattle = value; }
    public int MaxCreateSoldierCount { get => _MaxCreateSoldierCount;}
    public int MaxUseCardTime { get => _MaxUseCardTime; }

    void Init()
    {
        //初始化使用卡牌的次数和最大生成的士兵数量
        int roleLevel = RoleInfoManager.GetInstance().GetRoleLevel();
        _MaxUseCardTime = GetMaxUseCardCount(roleLevel);
        _UseCardTime = _MaxUseCardTime; //初始的时候使用次数等于最大次数
        _MaxCreateSoldierCount = GetMaxCreateSoldierNum(roleLevel);
    }

    void onRoleLevelChange(KeyValueUpdate kv)
    {
        int roleLevel = (int)kv.Values;
        _MaxUseCardTime = GetMaxUseCardCount(roleLevel);
        _MaxCreateSoldierCount = GetMaxCreateSoldierNum(roleLevel);
    }

    //减少使用的次数 1次
    public void DecreaseUseTime()
    {
        if (_UseCardTime <= 0)
            return;
        _UseCardTime -= 1;
    }

    //是否还有使用卡的次数
    public bool IsHaveUseCardTime()
    {
        if (_UseCardTime <= 0)
            return false;
        return true;
    }

    //重置使用卡牌的次数
    public void ResetUseTime()
    {
        if (_UseCardTime >= _MaxUseCardTime)
            return;
        _UseCardTime = _MaxUseCardTime;
    }

    /// <summary>
    /// 得到最多生成的士兵数量
    /// </summary>
    /// <param name="level">角色等级</param>
    /// <returns></returns>
    public int GetMaxCreateSoldierNum(int level)
    {
        //表里的等级就是ID
        Table_BattleUseCountComparision_DefineData data = TableManager.GetInstance().GetDataById("BattleUseCountComparision", level) as Table_BattleUseCountComparision_DefineData;
        int result = 0;
        result = data.MaxCreateSoldierNum;
        return result;
    }
    /// <summary>
    /// 得到使用卡牌的次数
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int GetMaxUseCardCount(int level)
    {
        Table_BattleUseCountComparision_DefineData data = TableManager.GetInstance().GetDataById("BattleUseCountComparision", level) as Table_BattleUseCountComparision_DefineData;
        int result = 0;
        result = data.MaxUseCardCount;
        return result;
    }

    public void InsertWujiangBattleCard(int id,WujiangData data)
    {
        WujiangData hasData = null;
        
        if (dicIntoBattleCard.TryGetValue(id,out hasData))
        {
            UIManager.GetInstance().ShowTip("已经存在于上阵武将数组中!");
            return; //已经存在，不能再添加进上阵武将卡牌数组
        }

        if (dicIntoBattleCard.Count >= GameCommon.MAX_WUJIANGINTOBATTLENUM)
        {
            UIManager.GetInstance().ShowTip("上阵数量已达到最大上阵数量!");
            return; //不能超过5个
        }

        dicIntoBattleCard.Add(id, data);
        cardIds.Add(id);

    }
    
    //得到上阵武将的卡牌数量
    public int GetWujiangIntoBattleCardNum()
    {
        return dicIntoBattleCard.Count;
    }

    public void RemoveWujiangIntoBattleCard(int id)
    {
        WujiangData data = null;
        if(dicIntoBattleCard.TryGetValue(id , out data))
        {
            dicIntoBattleCard.Remove(id);
        }
        else
        {
            Debug.LogError("移除上阵武将卡牌出错，阵中无:" + id + "的武将卡牌");
        }
    }

    //获取所有上阵的武将id
    public List<int> GetAllIntoBattleIds()
    {
        return cardIds;
    }
    /// <summary>
    /// 当前的通关数
    /// </summary>
    /// <returns></returns>
    public int GetPassedCheckpoint()
    {
        return GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").CheckPoint;
    }
    /// <summary>
    /// 关卡是否已经通关
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool IsPassed(int point)
    {
        return GetPassedCheckpoint() >= point ? true : false;
    }

    //进入战斗场景隐藏一些未关闭的界面
    public void HideOtherPanelInBattle()
    {
        //关闭其他所有的UI
        UIManager.GetInstance().CloseUIForm(SysDefine.SYS_MAINCITY_UIFORM);
        UIManager.GetInstance().CloseUIForm(SysDefine.SYS_BATTLESELECT_UIFORM);
        UIManager.GetInstance().CloseUIForm(SysDefine.SYS_CHECKPOINTDETAI_UIFORM);
        UIManager.GetInstance().CloseUIForm(SysDefine.SYS_LOADING_UIFORM);
        UIManager.GetInstance().CloseUIForm(SysDefine.SYS_LOGIN_UIFORM);
        UIManager.GetInstance().CloseUIForm(SysDefine.SYS_BATTLESELECT_UIFORM);
    }
    
    //离开战斗
    public void LeaveBattle()
    {
        SceneManager.LoadSceneAsync(ConvertEnumToString.GetInstance().GetStrByEnumScenes(ScenesEnum.MainCity));
        //打开主场景的UI
        UIManager.GetInstance().ShowUIForm(SysDefine.SYS_MAINCITY_UIFORM);
        ResetUseTime();
        //播放主程音乐
        AudioManager.GetInstance().PlayBgSound(2);
        GC.Collect();
    }

}
