using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : BaseUIForms
{
    public GameObject GoodsCell;
    public GameObject BtnClose;

    //不同的商品设置不同的父节点
    public GameObject ItemGoodsLayout;      //杂货
    public GameObject EpicCardLayout;       //史诗卡牌
    public GameObject EliteLayout;          //精英卡牌
    public GameObject OrdinaryCardLayout;   //普通卡牌

    private int ItemNum;
    private int EpicCardNum;
    private int EliteCardNum;
    private int OrdinaryCardNum;


    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        RegisterButtonEvent(BtnClose, (object args) =>
        {
            PlayClickCloseSound();
            CloseUIForm(SysDefine.SYS_SHOP_UIFORM);
            
        });

        InitShopCell();
        
        UnityHelper.SetScrollArea(RectTransform.Axis.Horizontal, ItemGoodsLayout, ItemNum, 2);
        UnityHelper.SetScrollArea(RectTransform.Axis.Horizontal, EpicCardLayout, EpicCardNum, 2);
        UnityHelper.SetScrollArea(RectTransform.Axis.Horizontal, EliteLayout, EliteCardNum, 2);
        UnityHelper.SetScrollArea(RectTransform.Axis.Horizontal, OrdinaryCardLayout, OrdinaryCardNum, 2);
        
    }

    //初始化杂货列表
    void InitShopCell()
    {
        int count = ShopManager.GetInstance().GetGoodsCount();
        List<int> goodsIds = ShopManager.GetInstance().GetAllGoodsIds();
        for (int i = 0; i < count; i++)
        {
            ShopBaseData celldata = ShopManager.GetInstance().FetchShopCellData(goodsIds[i]);
            GameObject go = GetComponent<GameObject>();
            go = Instantiate(GoodsCell);

            //目前的商品有两大类 1.卡牌 2.杂货
            //卡牌有三类 1.史诗 2.精英 3.普通

            //先以大类区分
            //1.卡牌 2.杂货
            if (celldata.ShopType == 1)
            {
                //卡牌类型 表里面的配置是 iconID就是卡牌的id
                //再按卡牌区分  将按照品质决定 5-史诗 3和4-精英 1和2-普通
                int quanlity = CardManager.GetInstance().GetCardQuanlity(celldata.IconID);
                if (quanlity == 5)
                {
                    go.transform.SetParent(EpicCardLayout.transform);
                    EpicCardNum++;
                }else if (quanlity == 3 || quanlity == 4)
                {
                    go.transform.SetParent(EliteLayout.transform);
                    EliteCardNum++;
                }else if (quanlity == 1 || quanlity == 2)
                {
                    go.transform.SetParent(OrdinaryCardLayout.transform);
                    OrdinaryCardNum++;
                }
                else
                {
                    Debug.LogError("读取卡牌的品质出错，没有该卡牌ID:" + celldata.IconID);
                }
            }
            else if (celldata.ShopType == 2)
            {
                //杂货
                go.transform.SetParent(ItemGoodsLayout.transform);
                ItemNum++;
            }

            go.transform.localScale = Vector3.one;
            go.GetComponent<WujiangCardShopCell>().baseData = celldata;
        }
    }
}