using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WujiangCardDetailPanel : BaseUIForms
{
    public Image CardRole;
    public Text Name;
    public Text Description;
    public Text AttributeDes;
    public GameObject StarPanel;
    public Button ToBattleButton;
    public Button RemoveBattleButton;
    public List<GameObject> starsGo = new List<GameObject>();
    private int starNum;
    private int id;
    private WujiangData data;
    private int m_selectCardId = 0;



    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.ImPeneterabla;
        base.RegisterButtonEvent("Button-Close", (object args) =>
        {
            PlayClickCloseSound();
            base.CloseUIForm(SysDefine.SYS_WUJIANGDETAIL_UIFORM);
        });
        base.RegisterButtonEvent("IntoBattle", onIntoBattleClick);
        base.RegisterButtonEvent("RemoveBattle", onRemoveBattleClick);
        
        ReceiveMessage(SysDefine.CardDetail, onShowCardDetail);
    }
    

    public void onShowCardDetail(KeyValueUpdate kv)
    {
        object[] objs = kv.Values as object[];
        data = objs[0] as WujiangData;
        ShowCardDetailEume type = (ShowCardDetailEume)Convert.ToInt32(objs[1]); 
        
        CardManager.GetInstance().ReplaceCardRole(CardRole, data.ID);
        Name.text = data.Name;
        Description.text = data.Description;
        
        var starNum = data.Star;
        if (starNum > 0)
        {
            for (int i = 0; i < starNum; i++)
            {
                starsGo[i].SetActive(true);
            }
            for(int i = starNum; i < 5; i++)
            {
                starsGo[i].SetActive(false);
            }
        }

        m_selectCardId = data.ID;

        //属性转换
        string name = CardManager.GetInstance().GetAttributeNameById(Convert.ToInt32(data.Attribute.Split(':')[0]));
        string num = CardManager.GetInstance().GetAttributeNum(Convert.ToInt32(data.Attribute.Split(':')[0]), Convert.ToInt32(data.Attribute.Split(':')[1]));
        string des = CardManager.GetInstance().GetAttributeDes(Convert.ToInt32(data.Attribute.Split(':')[0]));
        string newdes = des.Replace("{parameter}", data.Attribute.Split(':')[1]);
        AttributeDes.text = name + "   +" + num + '\n' + newdes;

        if (type == ShowCardDetailEume.IntoBattle)
        {
            ToBattleButton.gameObject.SetActive(true);
            RemoveBattleButton.gameObject.SetActive(false);
        }
        else if (type == ShowCardDetailEume.DownBattle)
        {
            ToBattleButton.gameObject.SetActive(false);
            RemoveBattleButton.gameObject.SetActive(true);
        }else if (type == ShowCardDetailEume.HideButton)
        {
            ToBattleButton.gameObject.SetActive(false);
            RemoveBattleButton.gameObject.SetActive(false);
        }

    }

    private void onIntoBattleClick(object args)
    {
            BattleManager.GetInstance().InsertWujiangBattleCard(m_selectCardId, data);

            AddTips(data.Name + "上阵成功");
            AudioManager.GetInstance().PlayEffectSound(106);
        
    }
    //下阵
    private void onRemoveBattleClick(object args)
    {
        
            //先从参战的数组中移除
            BattleManager.GetInstance().RemoveWujiangIntoBattleCard(m_selectCardId);
            //刷新界面
            SendUIFormMessage(SysDefine.UpdateIntoBattleCardPanel, "Update", data);
            //关闭当前界面
            CloseUIForm(SysDefine.SYS_WUJIANGDETAIL_UIFORM);
            AddTips(data.Name + "下阵成功");
            AudioManager.GetInstance().PlayEffectSound(106);
        
    }

}
