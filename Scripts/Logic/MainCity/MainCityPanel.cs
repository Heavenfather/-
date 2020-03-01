using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class MainCityPanel : BaseUIForms
{
    public Button Shop;
    public Button CardUpgrade;
    public Button Pack;

    private Transform traHead;
    private Transform traName;
    private Transform traLevel;
    private Transform traGoldNum;
    private Transform traCoinNum;
    private Transform traLingpaiNum;

    private Image ImgHead;
    private Text TxtName;
    private Text TxtLevel;
    private Text TxtGoldNum;
    private Text TxtCoinNum;
    private Text TxtLingpaiNum;

    private UserData userData;

    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.Normal;
        base.CurrentUIType.UIForm_Type = UIFormType.Fixed; 
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        traHead = UnityHelper.FindTheChildNode(this.gameObject, "Head");
        traName = UnityHelper.FindTheChildNode(this.gameObject, "Name");
        traLevel = UnityHelper.FindTheChildNode(this.gameObject, "Level");
        traGoldNum = UnityHelper.FindTheChildNode(this.gameObject, "Gold");
        traCoinNum = UnityHelper.FindTheChildNode(this.gameObject, "Coin");
        traLingpaiNum = UnityHelper.FindTheChildNode(this.gameObject, "Lingpai");

        ImgHead = traHead.GetComponent<Image>();
        TxtName = traName.GetComponent<Text>();
        TxtLevel = traLevel.GetComponent<Text>();
        TxtGoldNum = traGoldNum.GetComponent<Text>();
        TxtCoinNum = traCoinNum.GetComponent<Text>();
        TxtLingpaiNum = traLingpaiNum.GetComponent<Text>();

        userData = GameDataManager.UserData;
        TxtLevel.text = "Lv." + userData.Level;
        TxtName.text = userData.Name;
        CardManager.GetInstance().ReplaceIcon(ImgHead, CardManager.GetInstance().TransitionHeadID(userData.HeadId));    //更换头像
        TxtGoldNum.text = userData.Gold + "";
        TxtCoinNum.text = userData.Coin + "";
        TxtLingpaiNum.text = userData.LingPai + "";

        base.RegisterButtonEvent(traHead.gameObject, (object args) =>
        {
            OpenUIForm(SysDefine.SYS_ROLEDETAIL_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(105);
        });
        base.RegisterButtonEvent("Button-MasterMansion", (object args) =>
        {
            base.OpenUIForm(SysDefine.SYS_MASTERMANSION_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(105);
        });
        base.RegisterButtonEvent("Button-BattleSelect", (object args) =>
        {
            OpenUIForm(SysDefine.SYS_BATTLESELECT_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(105);
        });
        base.RegisterButtonEvent("Button-ChangeCard", (object args) =>
        {
            OpenUIForm(SysDefine.SYS_INTOBATTLECARD_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(105);
        });

        base.RegisterButtonEvent(Shop.gameObject, ( object args) =>
        {
            OpenUIForm(SysDefine.SYS_SHOP_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(105);
        });
        base.RegisterButtonEvent(CardUpgrade.gameObject, (object args) =>
        {
            OpenUIForm(SysDefine.SYS_WUJIANGUPGRADE_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(105);
            return;
        });

        base.RegisterButtonEvent(Pack.gameObject, (object args) =>
        {
            OpenUIForm(SysDefine.SYS_PACK_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(105);
            return;
        });

        ReceiveMessage(SysDefine.ChangeName, p =>
        {
            TxtName.text = GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").Name;
        });

        ReceiveMessage(SysDefine.GoldNumChanged,onGoldNumChanged);
        ReceiveMessage(SysDefine.CoinNumChanged, onCoinNumChanged);
        ReceiveMessage(SysDefine.LingpaiNumChanged, onLingpaiNumChanged);
        ReceiveMessage(SysDefine.RoleLevelChanged, onRoleLevelChanged);
        ReceiveMessage(SysDefine.ClearCurrency, (KeyValueUpdate kv) =>
        {
            int num = Convert.ToInt32(kv.Values);
            TxtGoldNum.text = num + "";
            TxtCoinNum.text = num + "";
            TxtLingpaiNum.text = num + "";
        });

    }

    private void onGoldNumChanged(KeyValueUpdate kv)
    {
        int num = Convert.ToInt32(kv.Values);
        TxtGoldNum.text = num + "";
    }

    private void onCoinNumChanged(KeyValueUpdate kv)
    {
        int num = Convert.ToInt32(kv.Values);
        TxtCoinNum.text = num + "";
    }

    private void onLingpaiNumChanged(KeyValueUpdate kv)
    {
        int num = Convert.ToInt32(kv.Values);
        TxtLingpaiNum.text = num + "";

    }

    private void onRoleLevelChanged(KeyValueUpdate kv)
    {
        int level = Convert.ToInt32(kv.Values);
        TxtLevel.text = "Lv." + level;

    }
}
