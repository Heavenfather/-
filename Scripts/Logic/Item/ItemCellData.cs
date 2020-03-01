using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemCellData
{
    //ID
    public int ID;
    //名称
    public string Name;
    //IconID
    public int Iconid;
    //描述
    public string Describe;
    //品质
    public int quanlity;
    //索引
    public int index;
    //显示类型
    public int showType;
    //显示数量
    public int showNum;
    //是否注册点击事件
    public bool isCanClick=true;

    public ItemCellData()
    {
        Init();
    }

    public void Init()
    {
        ID = -1;
        Name = "";
        Iconid = -1;
        Describe = "";
        quanlity = -1;
        isCanClick=true;
    }

}
