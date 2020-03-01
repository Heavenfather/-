using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WujiangCardShopCell : BaseUIForms
{
    public GameObject GoWujiangCardCell;
    public GameObject GoItemGoods;
    public ItemCell itemCell;
    public Image ImgMoneyType;
    public Text TxtPrice;
    public Text TxtItemGoodsName;

    [HideInInspector]
    public WujiangCardShopCellData cellCardData;
    [HideInInspector]
    public ShopBaseData baseData;

    private void Start()
    {
        ShopManager.GetInstance().UpdateCurrencyTypeImg(ImgMoneyType, baseData.CurrencyType);
        TxtPrice.text = baseData.Price + "";

        RegisterButtonEvent(this.gameObject, ( object args) =>
        {
            //打开是否购买的界面
            OpenUIForm(SysDefine.SYS_CONFIRMBUY_UIFORM);
            SendUIFormMessage(SysDefine.BuyGoods, "Shop", baseData);
        });

        if (baseData.ShopType == 1)
        {
            GoWujiangCardCell.SetActive(true);
            GoItemGoods.SetActive(false);

            InitWujiangCardCell();
        }else if (baseData.ShopType == 2)
        {
            GoWujiangCardCell.SetActive(false);
            GoItemGoods.SetActive(true);
            TxtItemGoodsName.text = baseData.Name;
            InitItemGoodsCell();
        }

    }

    void InitWujiangCardCell()
    {
        cellCardData.cardId = baseData.IconID;
        WujiangCardCell cardCell = GoWujiangCardCell.GetComponent<WujiangCardCell>();
        cardCell.data = CardManager.GetInstance().FetchWujiangCardDataById(cellCardData.cardId);
        cardCell.type = ShowCardDetailEume.HideButton;
        cardCell.style = CardShowStyle.HideLevel;
    }
    void InitItemGoodsCell()
    {
        itemCell.GetComponent<ItemCell>().celldata = ItemManager.GetInstance().createItemCellData(baseData.ID);
        itemCell.GetComponent<ItemCell>().celldata.showType = ItemCell.Type_ShowNum;
        itemCell.GetComponent<ItemCell>().celldata.showNum = 2000;
    }
}
