using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmBuyPanel : BaseUIForms
{
    public Image ImgCurrencyType;
    public Text CurrencyNum;
    public Text GoodsDes;

    public GameObject BtnConfirm;
    public GameObject BtnCancel;

    private ShopBaseData data;

    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        ReceiveMessage(SysDefine.BuyGoods, onBuyGoodsHandler);

    }

    private void Start()
    {
        RegisterButtonEvent(BtnCancel, (object args) =>
        {
            CloseUIForm(SysDefine.SYS_CONFIRMBUY_UIFORM);
        });

        RegisterButtonEvent(BtnConfirm, onConfirmClick);
    }

    void onBuyGoodsHandler(KeyValueUpdate kv)
    {
        data = kv.Values as ShopBaseData;

        ShopManager.GetInstance().UpdateCurrencyTypeImg(ImgCurrencyType, data.CurrencyType);
        CurrencyNum.text = data.Price+"";
        GoodsDes.text = "购买" + data.Name + "吗?";
    }   

    void onConfirmClick( object args)
    {
        string showTxt = "";
        if (data.CurrencyType == 1)
            showTxt = "元宝不足";
        else if (data.CurrencyType == 2)
            showTxt = "铜币不足";
        else if (data.CurrencyType == 3)
            showTxt = "令牌不足";
        //货币是否充足
        if (ShopManager.GetInstance().IsEnoughMoney(data.CurrencyType, data.Price) == false)
        {
            AddTips(showTxt);
            CloseUIForm(SysDefine.SYS_CONFIRMBUY_UIFORM);
            return;
        }

        //如果是武将卡牌，是否已经购买过
        List<int> haveCards = CardManager.GetInstance().HaveCardIds();
        for (int i = 0; i < haveCards.Count; i++)
        {
            //武将卡牌商品iconid就是基础的id
            if (data.IconID == haveCards[i])
            {
                AddTips("已经拥有了" + data.Name + "武将");
                CloseUIForm(SysDefine.SYS_CONFIRMBUY_UIFORM);
                return;
            }
        }

        //1-金币 2-铜币 3-令牌
        if (data.CurrencyType == 1)
        {
            ShopManager.GetInstance().UpdateGoldNum(-data.Price);
        }else if (data.CurrencyType == 2)
        {
            ShopManager.GetInstance().UpdateCoinNum(-data.Price);
        }else if (data.CurrencyType == 3)
        {
            ShopManager.GetInstance().UpdateLingpaiNum(-data.Price);
        }

        //是否是卡牌类型的商品  1-卡牌 2-杂货
        if (data.ShopType == 1)
        {
            CardManager.GetInstance().AddNewCard(data.IconID);  //武将卡牌，Iconid就是武将卡牌ID
            CardBaseData cardBaseData = new CardBaseData();
            cardBaseData.BaseID = data.IconID;
            cardBaseData.Level = 1; 
            CardManager.GetInstance().AddCardData(cardBaseData);
        }else if (data.ShopType == 2)
        {
            //对几个货币类型的道具处理 其实应该分开的
            if (data.ID == SysDefine.Item_Coin_Id)
            {
                ShopManager.GetInstance().UpdateCoinNum(1000);
            }else if (data.ID == SysDefine.Item_Lingpai_Id)
            {
                ShopManager.GetInstance().UpdateLingpaiNum(1000);
            }else if (data.ID == SysDefine.Item_Exp_Id)
            {
                RoleInfoManager.GetInstance().AddExp(1000);
            }
        }

        AddTips("购买成功");
        CloseUIForm(SysDefine.SYS_CONFIRMBUY_UIFORM);
        AudioManager.GetInstance().PlayEffectSound(106);
    }

}
