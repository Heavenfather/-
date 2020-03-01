using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCell : BaseUIForms
{
    public static int Type_ShowNum=1;
    public static int Type_HideNum=2;
    
    public Image bgframe;
    public Image icon;
    public Text num;
    [HideInInspector]
    public ItemCellData celldata;

    private void Start()
    {
        if (celldata!=null && celldata.ID>0)
        {
            if (celldata.showType == Type_HideNum)
            {
                num.gameObject.SetActive(false);
            }
            else if (celldata.showType == Type_ShowNum)
            {
                num.gameObject.SetActive(true);
                num.text = celldata.showNum + "";
            }
            ItemManager.GetInstance().ReplaceItemIcon(icon, celldata.Iconid);
            ItemManager.GetInstance().ReplaceQuanlityFrame(bgframe, celldata.quanlity);

            if (celldata.isCanClick)
            {
                RegisterButtonEvent(this.gameObject, (object args) =>
                {
                    OpenUIForm(SysDefine.SYS_ITEMCELLDETAIL_UIFORM);
                    SendUIFormMessage(SysDefine.ShowItemDetail, "ItemCell", celldata);
                });
            }
        }
        else
        {
            icon.gameObject.SetActive(false);
            num.gameObject.SetActive(false);
        }
    }



}
