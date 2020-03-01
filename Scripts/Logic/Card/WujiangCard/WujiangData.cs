using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WujiangData
{
    //ID
    public int ID;
    //名称
    public string Name;
    //属性
    public string Attribute;
    //描述
    public string Description;
    //图标ID
    public int IconId;
    //星级
    public int Star;
    //品质
    public int Quantity;
    //等级
    public int Level=1;
    //是否显示介绍
    public bool isShowTip = true;

    public WujiangData()
    {
        init();
    }

    public void init()
    {
        ID = -1;
        Name = "张三";
        Attribute = "属性";
        Description = "张三的描述";
        IconId = -1;
        Star = -1;
        Quantity = -1;
    }


}
