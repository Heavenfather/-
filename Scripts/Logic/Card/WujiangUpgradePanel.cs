﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TableConfigs;

public class WujiangUpgradePanel : BaseUIForms
{
    public GameObject cell;
    public GameObject GridLayout;
    public GameObject BtnClose;
    public Text CardName;
    public Text CardLevel;
    public GameObject StuffGridLayout;
    public GameObject BtnUpgrade;
    public GameObject AttributeGridLayout;
    public GameObject AttributeCell;
    public GameObject StuffItemCell;
    
    private WujiangData celldata;
    private WujiangData CurSelectData; //自动选中第一个

    private List<GameObject> attGos = new List<GameObject>();
    private List<GameObject> itemsGo = new List<GameObject>();

    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        RegisterButtonEvent(BtnClose, p =>
        {
            CloseUIForm(SysDefine.SYS_WUJIANGUPGRADE_UIFORM);
        });

        RegisterButtonEvent(BtnUpgrade, OnUpgradeClick);

        ReceiveMessage(SysDefine.CardUpgrade, onCardUpgradeSuccess);
    }

    void onCardUpgradeSuccess(KeyValueUpdate kv)
    {
        object[] value = kv.Values as object[];
        int id = (int)value[0];
        int level = (int)value[1];
        CardLevel.text = "Lv." + level;

        //更新属性行
        string[] allAtt = CardManager.GetInstance().GetAttribute(CurSelectData.ID).Split(';');
        for (int i = 0; i < allAtt.Length; i++)
        {
            int attID = Convert.ToInt32(allAtt[i].Split(':')[0]);
            int attNum = Convert.ToInt32(allAtt[i].Split(':')[1]);
            Table_WujiangUpgrade_DefineData record = TableManager.GetInstance().GetDataById("WujiangUpgrade", CurSelectData.ID) as Table_WujiangUpgrade_DefineData;

            AttributeChangeCell cell = attGos[i].GetComponent<AttributeChangeCell>();

            int oldNum = attNum + (level - 1) * record.UpgradeCoe + record.BasicNum;
            int newNum = attNum + level * record.UpgradeCoe + record.BasicNum;

            cell.ChangeAttribute(oldNum, newNum);
        }

        //更新材料
        string[] stuffs = CardManager.GetInstance().GetComsumeStuffs(CurSelectData.ID).Split(';');
        for (int i = 0; i < stuffs.Length; i++)
        {
            int itemID = Convert.ToInt32(stuffs[i].Split(',')[0]);
            int itemNum = Convert.ToInt32(stuffs[i].Split(',')[1]);

            int comsumeNum = itemNum * CurSelectData.Quantity + level;

            itemsGo[i].GetComponent<ItemCell>().UpdateShowNum(itemID, comsumeNum);
        }
    }

    private void OnUpgradeClick(object args)
    {
        List<int> itemids = new List<int>();
        List<int> itemnum = new List<int>();
        string[] stuffs = CardManager.GetInstance().GetComsumeStuffs(CurSelectData.ID).Split(';');
        for (int i = 0; i < stuffs.Length; i++)
        {
            int itemID = Convert.ToInt32(stuffs[i].Split(',')[0]);
            int itemNum = Convert.ToInt32(stuffs[i].Split(',')[1]);
            if (!PackManager.GetInstance().ItemNumEnouthg(itemID, itemNum))
            {
                AddTips("道具不足");
                return;
            }

            itemids.Add(itemID);
            itemnum.Add(itemNum);
        }

        //减少材料
        for (int i = 0; i < itemids.Count; i++)
        {
            PackManager.GetInstance().DecreaseItemNum(itemids[i], itemnum[i]);
        }
        int oldLevel = CardManager.GetInstance().GetSaveCardLevel(CurSelectData.ID);
        int newLevel = oldLevel + 1;
        CardManager.GetInstance().UpgradeCardLevel(CurSelectData.ID, newLevel);

        AudioManager.GetInstance().PlayEffectSound(107);
    }

    private void OnEnable()
    {
        InitCardCell();
        OnCellClick(CurSelectData);
    }
    
    //显示拥有的卡牌
    void InitCardCell()
    {
        for (int i = 0; i < GridLayout.transform.childCount; i++)
        {
            Transform child = GridLayout.transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }

        List<int> ids = CardManager.GetInstance().HaveCardIds();
        for(int i = 0; i < ids.Count; i++)
        {
            WujiangData data = CardManager.GetInstance().FetchWujiangCardDataById(ids[i]);
            data.isShowTip = false; //不加点击显示介绍的事件
            GameObject go = GetComponent<GameObject>();
            go = Instantiate(cell);
            
            celldata = data;
            if (i == 0)
                CurSelectData = celldata;
            data.Level = CardManager.GetInstance().GetSaveCardLevel(data.IconId);
            EventTriggerListener.Get(go.GetComponent<WujiangCardCell>().icon.gameObject, data).onClick = OnCellClick;
            go.transform.SetParent(GridLayout.transform);
            go.transform.localScale = Vector3.one;
            go.GetComponent<WujiangCardCell>().data = data;
            go.GetComponent<WujiangCardCell>().style = CardShowStyle.HideName;

            
        }
    }

    void OnCellClick(object args)
    {
        WujiangData data = args as WujiangData;
        CurSelectData = data;
        //Debug.Log(data.Name);
        CardName.text = data.Name;
        int level = CardManager.GetInstance().GetSaveCardLevel(data.ID);
        CardLevel.text = "Lv."+ level;

        attGos = new List<GameObject>();
        itemsGo = new List<GameObject>();

        for (int i = 0; i < AttributeGridLayout.transform.childCount; i++)
        {
            Transform child = AttributeGridLayout.transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < StuffGridLayout.transform.childCount; i++)
        {
            Transform child = StuffGridLayout.transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }

        //属性行
        string[] allAttribute = CardManager.GetInstance().GetAttribute(data.ID).Split(';');
        for (int i = 0; i < allAttribute.Length; i++)
        {
            int attID = Convert.ToInt32(allAttribute[i].Split(':')[0]);
            int attNum = Convert.ToInt32(allAttribute[i].Split(':')[1]);
            string strAttName = CardManager.GetInstance().GetAttributeNameById(attID);
            GameObject go = GetComponent<GameObject>();
            go = Instantiate(AttributeCell);
            AttributeChangeCellData attCellData = new AttributeChangeCellData();
            Table_WujiangUpgrade_DefineData record = TableManager.GetInstance().GetDataById("WujiangUpgrade", CurSelectData.ID) as Table_WujiangUpgrade_DefineData;
            attCellData.Name = strAttName;
            attCellData.CurrentAttNum = attNum + level * record.UpgradeCoe + record.BasicNum;
            attCellData.NextAttNum = attNum + (level+1) * record.UpgradeCoe + record.BasicNum;
            go.GetComponent<AttributeChangeCell>().celldata = attCellData;
            go.transform.SetParent(AttributeGridLayout.transform);
            go.transform.localScale = Vector3.one;
            attGos.Add(go);
        }

        //消耗材料
        string[] stuffs = CardManager.GetInstance().GetComsumeStuffs(data.ID).Split(';');
        for (int i = 0; i < stuffs.Length; i++)
        {
            int itemID = Convert.ToInt32(stuffs[i].Split(',')[0]);
            int itemNum = Convert.ToInt32(stuffs[i].Split(',')[1]);
            ItemCellData itemdata = ItemManager.GetInstance().createItemCellData(itemID);
            GameObject go = GetComponent<GameObject>();
            go = Instantiate(StuffItemCell);
            int comsumeNum = itemNum * data.Quantity + level;
            itemdata.isCanClick = true;
            itemdata.showType = ItemCell.Type_ShowNum;
            itemdata.showNum = comsumeNum;
            go.GetComponent<ItemCell>().celldata = itemdata;
            go.transform.SetParent(StuffGridLayout.transform);
            go.transform.localScale = Vector3.one;
            itemsGo.Add(go);
        }
    }



}
