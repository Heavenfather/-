using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*游戏核心参数
 *功能：
 *  1.系统常量
 *  2.全局性方法
 *  3.系统枚举类型
 *  4.委托定义
 */

#region 系统枚举类型

//UI窗体类型
public enum UIFormType
{
    Norlmal,        //普通窗体
    Fixed,          //固定窗体
    PopUp           //弹出窗体
}

//UI窗体的显示类型
public enum UIFormShowMode
{
    Normal,         //普通
    ReverseChange,  //反向切换窗体
    HideOther       //隐藏其他窗口
}

//UI窗体的透明度类型
public enum UIFormLucencyType
{
    Lucency,        //完全透明 不能穿透
    Translucency,    //半透明  不能穿透
    ImPeneterabla,  //低透明度 不能穿透
    Pentrate        //能穿透
}

//场景名称的枚举
public enum ScenesEnum
{
    None=1000,
    StartScenes,
    LoadingScenes,
    MainCity,
    BattleScene01,
    BattleScene02,
    BattleScene03
}

/// <summary>
/// 武将卡牌详情界面是否需要显示上阵或下阵按钮
/// </summary>
public enum ShowCardDetailEume
{
    DownBattle=1,   //下阵
    IntoBattle=2,   //上阵
    InBattle=3,     //处于战斗场景点击查看详情的
    HideButton=4     //隐藏上阵和下阵两个按钮，战斗里面用
}

/// <summary>
/// 武将卡显示类型
/// </summary>
public enum CardShowStyle
{
    Normal,         //正常 显示等级、名字
    HideLevel,      //隐藏等级
    HideName        //隐藏名字
}

/// <summary>
/// 小兵状态
/// </summary>
public enum SoldierState
{
    Idle,
    Chase,
    Attack1,
    Attack2,
    Attack3,
    Damage,
    Death
}

/// <summary>
/// 士兵类型
/// </summary>
public enum SoldierType
{
    Elite,      //精英
    BatMan      //小兵
}

/// <summary>
/// 属性类型
/// </summary>
public enum AttributeType
{
    Attack=130,         //攻击
    Defence=140,        //防御
    HP=160,             //血量
    SkillAttack=220     //技能攻击系数
}

#endregion

#region 委托
//事件的委托标识，根据委托类型的参数数量使用其中一个
public delegate void GameAction();
public delegate void GameAction<in T>(T arg);
public delegate void GameAction<in T1, in T2>(T1 arg1, T2 arg2);
public delegate void GameAction<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);
#endregion

public class SysDefine{
    #region 系统常量
    //路径常量
    public const string SYS_PATH_CANVAS = "Canvas";
    public const string SYS_PATH_UIFORMJSONINFO = "UIFormsConfigInfo";
    public const string SYS_PATH_SYSCONFIGINFO = "SysConfigInfo";
    //标签常量
    public const string SYS_TAG_CANVAS = "_TagCanvas";
    public const string SYS_TAG_UICAMERA = "_TagUICamera";
    public const string SYS_TAG_PLAYER = "Player";
    public const string SYS_TAG_ENEMY = "Enemy";

    //节点常量
    public const string SYS_NODE_NORMAL = "NormalPanel";
    public const string SYS_NODE_FIXED = "FixedPanel";
    public const string SYS_NODE_POPUP = "PopUpPanel";
    public const string SYS_NODE_SCRIPTSMANAGER = "_UIScriptsHolder";

    //UI窗体预设名称
    public const string SYS_CHECKPOINTDETAI_UIFORM = "CheckPointDetailPanel";
    public const string SYS_LOADING_UIFORM = "LoadingPanel";
    public const string SYS_BATTLESELECT_UIFORM = "BattleSelectPanel";
    public const string SYS_WUJIANGDETAIL_UIFORM = "WujiangCardDetailPanel";
    public const string SYS_LOGIN_UIFORM = "LoginPanel";
    public const string SYS_MASTERMANSION_UIFORM = "MasterMansionPanel";
    public const string SYS_MAINCITY_UIFORM = "MainCityPanel";
    public const string SYS_INTOBATTLECARD_UIFORM = "IntoBattleCardPanel";
    public const string SYS_ADDSODILERTIPS_UIFORM = "AddSodilerTipsPanel";
    public const string SYS_INBATTLECARDDETAIL_UIFORM = "InBattleCardDetailPanel";
    public const string SYS_ACCOUNT_UIFORM = "AccountPanel";
    public const string SYS_REGISTER_UIFORM = "RegisterPanel";
    public const string SYS_ROLEDETAIL_UIFORM = "RoleDetailPanel";
    public const string SYS_DELETEDATA_UIFORM = "DeleteDataPanel";
    public const string SYS_SHOP_UIFORM = "ShopPanel";
    public const string SYS_CONFIRMBUY_UIFORM = "ConfirmBuyPanel";
    public const string SYS_ITEMCELLDETAIL_UIFORM = "ItemCellDetailPanel";
    public const string SYS_BATTLEWIN_UIFORM = "BattleWinPanel";
    public const string SYS_BATTLEFAIL_UIFORM = "BattleFailPanel";
    public const string SYS_ISPASSED_UIFORM = "IsPassedPanel";
    public const string SYS_WUJIANGUPGRADE_UIFORM = "WujiangUpgradePanel";
    public const string SYS_PACK_UIFORM = "PackPanel";

    /* 消息传递定义常量区 */
    public const string CardDetail = "CardDetail";
    public const string BattleSelectData = "BattleSelectData";
    public const string InitSodlier = "InitSodlier";
    public const string MeleeSoldier = "MeleeSoldier";
    public const string FarSoldier = "FarSoldier";
    public const string CardUseTime = "CardUseTime";
    public const string TipsEvent = "TipsEvent";
    public const string ChangeName = "ChangeName";
    public const string GoldNumChanged = "GoldNumChanged";
    public const string CoinNumChanged = "CoinNumChanged";
    public const string LingpaiNumChanged = "LingpaiNumChanged";
    public const string BuyGoods = "BuyGoods";
    public const string DestroyWujiangCardCell = "DestroyWujiangCardCell";
    public const string UpdateIntoBattleCardPanel = "UpdateIntoBattleCardPanel";
    public const string ShowItemDetail = "ShowItemDetail";
    public const string RoleLevelChanged = "RoleLevelChanged";
    public const string HaveCardNumChanged = "HaveCardNumChanged";
    public const string UpdateUserCheckPoint = "UpdateUserCheckPoint";
    public const string ClearCurrency = "ClearCurrency";
    public const string ItemNumChange = "ItemNumChange";
    public const string CardUpgrade = "CardUpgrade";

    //道具ID对应 道具映射表
    public const int Item_Gold_Id = 101;
    public const int Item_Coin_Id = 102;
    public const int Item_Lingpai_Id = 103;
    public const int Item_Exp_Id = 104;

    #endregion

}
