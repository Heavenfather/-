using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class PackPanel : BaseUIForms
{
    public GameObject BtnClose;
    public GameObject ItemGridLayout;
    public GameObject itemCell;
    
    [Header("给定的格子数量")]
    public int gridNum = 60;    //暂时是给定数量，可以根据需求变化

    void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;
        
        UnityHelper.SetScrollArea(RectTransform.Axis.Vertical, ItemGridLayout, gridNum, 6);

        RegisterButtonEvent(BtnClose, p =>
        {
            PlayClickCloseSound();
            CloseUIForm(SysDefine.SYS_PACK_UIFORM);
        });
        InitItemCell(null);

        ReceiveMessage(SysDefine.ItemNumChange, InitItemCell);
    }
    
    void InitItemCell(KeyValueUpdate kv)
    {
        for (int i = 0; i < ItemGridLayout.transform.childCount; i++)
        {
            GameObject.Destroy(ItemGridLayout.transform.GetChild(i).gameObject);
        }

        //取得所有拥有的道具的id
        List<int> haveItemids = PackManager.GetInstance().GetAllPackItemId();

        //暂时给定背包格的数量
        for (int i = 0; i < gridNum; i++)
        {
            GameObject go = GetComponent<GameObject>();
            go = Instantiate(itemCell);

            if (i < haveItemids.Count)
            {
                ItemCellData celldata = ItemManager.GetInstance().createItemCellData(haveItemids[i]);
                celldata.isCanClick = true;
                celldata.index = i;
                celldata.showType = ItemCell.Type_ShowNum;
                if (kv != null)
                {
                    ItemCellData changeCellData = kv.Values as ItemCellData;
                    if (haveItemids[i] == changeCellData.ID)
                    {
                        celldata.showNum = changeCellData.showNum;
                    }
                }
                else
                    celldata.showNum = PackManager.GetInstance().GetItemNum(haveItemids[i]);
                go.GetComponent<ItemCell>().celldata = celldata;                
            }
            else
            {
                go.GetComponent<ItemCell>().celldata = null;
            }
            
            UnityHelper.AddChildNodeToParentNode(ItemGridLayout.transform, go.transform);
        }
    } 

}
