using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCellDetailPanel : BaseUIForms
{
    public Image ImgMask;
    public ItemCell cell;
    public Text TxtDes;

    private ItemCellData celldata;

    void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.ImPeneterabla;

        RegisterButtonEvent(ImgMask.gameObject, (object args) =>
        {
            //点击遮罩会关闭自己
            CloseUIForm(SysDefine.SYS_ITEMCELLDETAIL_UIFORM);
        });

        ReceiveMessage(SysDefine.ShowItemDetail, (KeyValueUpdate kv) =>
         {
             celldata = kv.Values as ItemCellData;
             cell.GetComponent<ItemCell>().celldata = celldata;
             cell.GetComponent<ItemCell>().celldata.showType = ItemCell.Type_HideNum;
             cell.GetComponent<ItemCell>().celldata.isCanClick = false;
             ItemManager.GetInstance().ReplaceItemIcon(cell.GetComponent<ItemCell>().icon, celldata.Iconid);
             ItemManager.GetInstance().ReplaceQuanlityFrame(cell.GetComponent<ItemCell>().bgframe, celldata.quanlity);
             TxtDes.text = celldata.Describe;
         });

    }
    
}
