using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class BatttleCardPanel : BaseUIForms
{
    public Image MeleeCard;
    public Image FarCard;
    public GameObject CardCell;
    public GameObject parent;
    public Text RetainTime;
    public Button BtnReturn;
    
    public GameObject itemCell;

    private void Awake()
    {
        InitWuJiangCardCell();
    }

    void Start()
    {
        if (MeleeCard)
        {
            EventTriggerListener.Get(MeleeCard.gameObject).onClick = onInitMeleeSoldierClick;
        }

        if (FarCard)
        {
            EventTriggerListener.Get(FarCard.gameObject).onClick = onInitFarSoldierClick;
        }

        if (BtnReturn)
        {
            EventTriggerListener.Get(BtnReturn.gameObject).onClick = onReturnMainCity;
        }

        RetainTime.text = "剩余使用次数：" + BattleManager.GetInstance().UseCardTime +"/"+ BattleManager.GetInstance().MaxUseCardTime;
        ReceiveMessage(SysDefine.CardUseTime, onCardUseTimeHandler);

        AudioManager.GetInstance().PlayBgSound(1);

    }

    private void Update()
    {
        if (BattleManager.GetInstance().IsStartBattle)
        {
            if (BattleManager.GetInstance().AliveEnemys <= 0)
            {
                //战斗胜利
                OpenUIForm(SysDefine.SYS_BATTLEWIN_UIFORM);
                BattleManager.GetInstance().IsStartBattle = false;
                this.gameObject.SetActive(false);
            }

            if (BattleManager.GetInstance().AlivePlayer <= 0)
            {
                //失败
                OpenUIForm(SysDefine.SYS_BATTLEFAIL_UIFORM);
                BattleManager.GetInstance().IsStartBattle = false;
                this.gameObject.SetActive(false);
            }
        }
    }


    private void onReturnMainCity(object args)
    {
        BattleManager.GetInstance().LeaveBattle();
        AudioManager.GetInstance().PlayEffectSound(101);
    }

    private void onCardUseTimeHandler(KeyValueUpdate kv)
    {
        int useTime = 0;
        object[] objs = kv.Values as object[];
        useTime = Convert.ToInt32(objs[0]);
        RetainTime.text = "剩余使用次数：" + useTime  +"/" + BattleManager.GetInstance().MaxUseCardTime;
    }        

    private void onInitMeleeSoldierClick(object args)
    {
        //Debug.Log(go.name);
        UIManager.GetInstance().ShowUIForm(SysDefine.SYS_ADDSODILERTIPS_UIFORM);
        //发送需要生成的是近战士兵的信息给另外一个界面
        object[] obj = new object[1];
        obj[0] = SysDefine.MeleeSoldier;   //就发送一个名称过去应该就可以识别了
        SendUIFormMessage(SysDefine.InitSodlier, "data", obj);
    }

    private void onInitFarSoldierClick(object args)
    {
        //Debug.Log(go.name);
        UIManager.GetInstance().ShowUIForm(SysDefine.SYS_ADDSODILERTIPS_UIFORM);
        //发送需要生成的是近战士兵的信息给另外一个界面
        object[] obj = new object[1];
        obj[0] = SysDefine.FarSoldier;   //就发送一个名称过去应该就可以识别了
        SendUIFormMessage(SysDefine.InitSodlier, "data", obj);
        AudioManager.GetInstance().PlayEffectSound(105);
    }

    private void InitWuJiangCardCell()
    {
        int count = BattleManager.GetInstance().GetWujiangIntoBattleCardNum();
        List<int> ids = BattleManager.GetInstance().GetAllIntoBattleIds();

        for (int i = 0; i < count; i++)
        {
            WujiangData data = CardManager.GetInstance().FetchWujiangCardDataById(ids[i]);
            GameObject go = GetComponent<GameObject>();
            go = Instantiate(CardCell);

            go.transform.SetParent(parent.transform);
            go.transform.localScale = Vector3.one;  //设置父节点后scale会莫名其妙变成60 在这里转换回来

            go.GetComponent<WujiangCardCell>().data = data;
            go.GetComponent<WujiangCardCell>().type = ShowCardDetailEume.InBattle;
            go.GetComponent<WujiangCardCell>().style = CardShowStyle.Normal;

        }
    }

}
