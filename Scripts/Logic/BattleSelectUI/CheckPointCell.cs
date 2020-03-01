using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointCell : BaseUIForms
{
    public Image ImgBase;   //底座图片
    public Image ImgRole;
    public GameObject TouchArea;

    [HideInInspector]
    public BattleCellData celldata;

    private void Start()
    {
        RegisterButtonEvent(TouchArea, ( object args) =>
        {
            int pointID = celldata.ID;
            OpenUIForm(SysDefine.SYS_CHECKPOINTDETAI_UIFORM);
            object[] objs = new object[1];
            objs[0] = celldata;
            SendUIFormMessage(SysDefine.BattleSelectData, "data", objs);
        });

        InitCell();

    }

    void InitCell()
    {
        CardManager.GetInstance().ReplaceCardRole(ImgRole, celldata.RoleID);
        if (celldata.index % 2 == 0)
        {
            //相邻的图片资源和位置是不一样的
            ImgBase.sprite = ResourcesMgr.GetInstance().LoadResource<Sprite>("UI/BattleSelect/tongtiantatizi1", true);
            ImgRole.transform.localPosition = new Vector3(45, 45, 0);
            ImgBase.transform.localPosition = new Vector3(0, 0,0);
        }
        else
        {
            ImgBase.sprite = ResourcesMgr.GetInstance().LoadResource<Sprite>("UI/BattleSelect/tongtiantatizi2", true);
            ImgBase.transform.localPosition = new Vector3(-30, 0,0);
            ImgRole.transform.localPosition = new Vector3(-33, 42, 0);
        }
    }

}
