using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class InBattleCardDetailPanel : BaseUIForms
{
    public WujiangCardCell cell;
    public Text InBattleDescribe;
    private WujiangData data;

    private GameObject[] Soldiers;
    private bool _haveSelfSoldier=false;

    void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        base.RegisterButtonEvent("Cancel", (object args) =>
        {
            base.CloseUIForm(SysDefine.SYS_INBATTLECARDDETAIL_UIFORM);
            PlayClickCloseSound();
        });

        base.RegisterButtonEvent("OK", onOkClick);

        ReceiveMessage(SysDefine.CardDetail, onShowCardDetail);
    }

    private void Update()
    {
        //查询场景中的己方士兵
        Soldiers = GameObject.FindGameObjectsWithTag(SysDefine.SYS_TAG_PLAYER);
        if (Soldiers.Length > 0)
        {
            _haveSelfSoldier = true;
        }
        else
        {
            _haveSelfSoldier = false;
        }
    }

    private void onOkClick(object args)
    {
        //是否有使用次数
        if (BattleManager.GetInstance().IsHaveUseCardTime() == false)
        {
            //Debug.Log("没有使用次数!!");
            UIManager.GetInstance().ShowTip("没有使用次数!!");
            return;
        }
        //是否有己方士兵在场景里
        if (_haveSelfSoldier == false)
        {
            //Debug.Log("没有己方士兵在场景中!!");
            UIManager.GetInstance().ShowTip("没有己方士兵在场景中!!");
            return;
        }
        
        //UI处理
        CloseUIForm(SysDefine.SYS_INBATTLECARDDETAIL_UIFORM);
        BattleManager.GetInstance().DecreaseUseTime();  //减少一次使用次数
        object[] obj = new object[1];
        obj[0] = BattleManager.GetInstance().UseCardTime;
        SendUIFormMessage(SysDefine.CardUseTime, "UseTime", obj);   //刷新还剩余多少次的界面显示

        AudioManager.GetInstance().PlayEffectSound(106);

        //提示加成的属性
        string showText=CardManager.GetInstance().GetAttributeNameById(Convert.ToInt32(data.Attribute.Split(':')[0]))+"+"+ data.Attribute.Split(':')[1];
        UIManager.GetInstance().ShowTip(showText);

        //关于战斗的处理
        for (int i = 0; i < Soldiers.Length; i++)
        {
            if (Soldiers[i])
            {
                SoldierBaseAI ai = Soldiers[i].GetComponent<SoldierBaseAI>();
                //根据属性的类型增加不同的属性
                if (Convert.ToInt32(data.Attribute.Split(':')[0]) == Convert.ToInt32(AttributeType.Attack))
                {
                    //攻击
                    ai.SoldierAttack += Convert.ToInt32(data.Attribute.Split(':')[1]);
                }else if(Convert.ToInt32(data.Attribute.Split(':')[0]) == Convert.ToInt32(AttributeType.Defence))
                {
                    //防御
                    ai.SoldierDefence += Convert.ToInt32(data.Attribute.Split(':')[1]);
                }else if(Convert.ToInt32(data.Attribute.Split(':')[0]) == Convert.ToInt32(AttributeType.HP))
                {
                    //血量
                    ai.FloCurrentHealth += Convert.ToInt32(data.Attribute.Split(':')[1]);
                    if (ai.FloCurrentHealth > ai.IntMaxHealth)
                    {
                        ai.FloCurrentHealth = ai.IntMaxHealth;
                    }
                }
                else if(Convert.ToInt32(data.Attribute.Split(':')[0]) == Convert.ToInt32(AttributeType.SkillAttack))
                {
                    //技能攻击系数
                    ai.SkillHurtPower += Convert.ToInt32(data.Attribute.Split(':')[1]);                    
                }
            }
        }
    }

    private void onShowCardDetail(KeyValueUpdate kv)
    {
        object[] objs = kv.Values as object[];
        data = objs[0] as WujiangData;
        
        cell.GetComponent<WujiangCardCell>().data = data;
        cell.GetComponent<WujiangCardCell>().type = ShowCardDetailEume.HideButton;
        cell.GetComponent<WujiangCardCell>().style = CardShowStyle.Normal;

        string attributeName = CardManager.GetInstance().GetAttributeNameById(Convert.ToInt32(data.Attribute.Split(':')[0]));
        string num = CardManager.GetInstance().GetAttributeNum(Convert.ToInt32(data.Attribute.Split(':')[0]), Convert.ToInt32(data.Attribute.Split(':')[1]));
        string des = CardManager.GetInstance().GetAttributeDes(Convert.ToInt32(data.Attribute.Split(':')[0]));
        string newdes = des.Replace("{parameter}", data.Attribute.Split(':')[1]);
        InBattleDescribe.text= attributeName + "   +" + num + '\n' + newdes;
    }
}
