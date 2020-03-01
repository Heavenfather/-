using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ShopBaseData
{
    //ID
    public int ID;
    //名称
    public string Name;
    //消耗货币类型
    public int CurrencyType;
    //图标ID
    public int IconID;
    //商品的类型
    public int ShopType;
    //价格
    public int Price;

    public ShopBaseData()
    {
        ID = -1;
        Name = "";
        CurrencyType = -1;
        IconID = 0;
        ShopType = -1;
        Price = -1;
    }

}
