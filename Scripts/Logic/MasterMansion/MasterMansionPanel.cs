using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class MasterMansionPanel : BaseUIForms
{
    public GameObject CardCell;
    public GameObject WujiangCardGrid;
    private List<WujiangData> datas = new List<WujiangData>();
    private int cardNum;

    void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        base.RegisterButtonEvent("Button-Close", OnCloseClick);

    }

    private void OnEnable()
    {
        InitializeWujiangCard();
        //float cellWidth = CardCell.GetComponent<RectTransform>().rect.width;
        //float gridSpace = WujiangCardGrid.GetComponent<GridLayoutGroup>().spacing.y;
        //float gridWidth = (cardNum / 4) * (cellWidth + gridSpace);

        //if (cardNum % 4 != 0)
        //    gridWidth += cellWidth + gridSpace;

        //this.WujiangCardGrid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, gridWidth);
        UnityHelper.SetScrollArea(RectTransform.Axis.Vertical, WujiangCardGrid, cardNum, 4);
    }

    public void OnCloseClick(object args)
    {
        PlayClickCloseSound();
        base.CloseUIForm(SysDefine.SYS_MASTERMANSION_UIFORM);
    }

    //初始化武将卡
    void InitializeWujiangCard()
    {
        //先将之前所有的子节点移除
        for (int i = 0; i < WujiangCardGrid.transform.childCount; i++)
        {
            Transform child = WujiangCardGrid.transform.GetChild(i);
            GameObject.Destroy(child.gameObject);

        }

        List<int> haveCardIds = CardManager.GetInstance().HaveCardIds();
        int count = haveCardIds.Count;
        cardNum = count;
        for (int i = 0; i < count; i++)
        {
            WujiangData data = CardManager.GetInstance().FetchWujiangCardDataById(haveCardIds[i]);
            GameObject go = GetComponent<GameObject>();
            go = Instantiate(CardCell);

            go.transform.SetParent(WujiangCardGrid.transform);
            go.transform.localScale = Vector3.one;  //设置父节点后scale会莫名其妙变成60 在这里转换回来
            go.GetComponent<WujiangCardCell>().data = data;
            go.GetComponent<WujiangCardCell>().type = ShowCardDetailEume.IntoBattle;
            go.GetComponent<WujiangCardCell>().style = CardShowStyle.Normal;
        }
    }

}
