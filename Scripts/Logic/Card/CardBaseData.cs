using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CardBaseData
{
    public int BaseID;      //ID 
    public int Level;       //等级

    public CardBaseData()
    {
        BaseID = 0;
        Level = 1;
    }
}
