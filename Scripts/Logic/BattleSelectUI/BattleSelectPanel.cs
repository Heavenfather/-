using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools;

public class BattleSelectPanel : BaseUIForms
{
    public Transform TraGridLayout;
    public GameObject CheckPointcell;

    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.ImPeneterabla;

        base.RegisterButtonEvent("Button-Close", (object args) => {
            base.CloseUIForm(SysDefine.SYS_BATTLESELECT_UIFORM);
            PlayClickCloseSound();
        });
        Transform traBaseImg = UnityHelper.FindTheChildNode(CheckPointcell, "Base");
        int count = CheckPointManager.GetInstance().GetCheckPointCount();
        float rectHeight = count * (traBaseImg.GetComponent<RectTransform>().rect.height + TraGridLayout.GetComponent<GridLayoutGroup>().spacing.y);
        TraGridLayout.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectHeight);
        List<int> ids = CheckPointManager.GetInstance().GetCheckPointIds();
        for (int i = 0; i < count; i++)
        {
            GameObject go = GetComponent<GameObject>();
            go = Instantiate(CheckPointcell);
            CheckPointCell cell = go.GetComponent<CheckPointCell>();
            go.transform.SetParent(TraGridLayout);
            go.transform.localScale = Vector3.one;
            cell.celldata = CheckPointManager.GetInstance().createCheckPointData(ids[i]);
            cell.celldata.index = i + 1;
        }
    }    


}
