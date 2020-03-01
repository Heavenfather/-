using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeChangeCell : BaseUIForms
{
    public Text Name;
    public Text CurNum;
    public Text NextNum;
    
    [HideInInspector]
    public AttributeChangeCellData celldata;
    void Start()
    {
        Name.text = celldata.Name;
        CurNum.text = celldata.CurrentAttNum + "";
        NextNum.text = celldata.NextAttNum + "";
    }

    /// <summary>
    /// 换值
    /// </summary>
    /// <param name="oldNum"></param>
    /// <param name="newNum"></param>
    public void ChangeAttribute(int oldNum,int newNum)
    {
        CurNum.text = oldNum + "";
        NextNum.text = newNum + "";
    }

}
