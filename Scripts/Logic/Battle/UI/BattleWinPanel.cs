using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleWinPanel : BaseUIForms
{
    public GameObject BtnConfirm;
    public GameObject RewardContainerGrid;
    public GameObject itemCell;

    private List<int> itemsid = new List<int>();
    private List<int> itemsnum = new List<int>();
    void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.ImPeneterabla;

        RegisterButtonEvent(BtnConfirm.gameObject, (object args) =>
        {
            //已经通关过 不会再有奖励
            if (BattleManager.GetInstance().IsPassed(BattleManager.EntryCheckPointTag) == false)
            {
                //添加奖励
                for (int i = 0; i < itemsid.Count; i++)
                {
                    //区分货币和其它的道具
                    if (itemsid[i] == SysDefine.Item_Coin_Id)
                    {
                        ShopManager.GetInstance().UpdateCoinNum(itemsnum[i]);
                    }
                    else if (itemsid[i] == SysDefine.Item_Lingpai_Id)
                    {
                        ShopManager.GetInstance().UpdateLingpaiNum(itemsnum[i]);
                    }
                    else if (itemsid[i] == SysDefine.Item_Exp_Id)
                    {
                        RoleInfoManager.GetInstance().AddExp(itemsnum[i]);
                    }
                    else
                    {
                        PackManager.GetInstance().AddItemNum(itemsid[i], itemsnum[i]);
                    }
                }

                //更新通关信息
                RoleInfoManager.GetInstance().UpdateCheckPoint(BattleManager.EntryCheckPointTag);
            }
            CloseUIForm(SysDefine.SYS_BATTLEWIN_UIFORM);
            BattleManager.GetInstance().LeaveBattle();
            AudioManager.GetInstance().PlayEffectSound(106);
        });

        if (BattleManager.GetInstance().IsPassed(BattleManager.EntryCheckPointTag) == false)
        {
            InitRewardItems();
        }
    }

    private void OnEnable()
    {
        AudioManager.GetInstance().PlayEffectSound(102);
    }

    //初始化关卡的奖励
    void InitRewardItems()
    {
        string[] items = CheckPointManager.GetInstance().GetCheckPointReward(BattleManager.EntryCheckPointTag).Split(';');
        int count = items.Length;
        foreach (var item in items)
        {
            int id = Convert.ToInt32(item.Split(':')[0]);
            int num = Convert.ToInt32(item.Split(':')[1]);
            itemsid.Add(id);
            itemsnum.Add(num);
        }
        //初始化奖励
        for (int i = 0; i < count; i++)
        {
            ItemCellData celldata = ItemManager.GetInstance().createItemCellData(itemsid[i]);
            celldata.index = i;
            celldata.showNum = itemsnum[i];
            celldata.showType = ItemCell.Type_ShowNum;
            GameObject go = GetComponent<GameObject>();
            go = Instantiate(itemCell);
            go.transform.SetParent(RewardContainerGrid.transform);
            go.transform.localScale = Vector3.one;
            go.GetComponent<ItemCell>().celldata = celldata;
        }
    }
}
