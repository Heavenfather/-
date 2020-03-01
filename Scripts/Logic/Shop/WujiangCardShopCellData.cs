using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WujiangCardShopCellData : ShopBaseData
{
    public int cardId;

    public WujiangCardShopCellData()
    {
        cardId = IconID;
    }
}
