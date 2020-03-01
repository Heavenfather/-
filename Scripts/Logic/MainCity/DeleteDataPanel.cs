using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteDataPanel : BaseUIForms
{
    public GameObject Confirm;
    public GameObject Cancel;
    public void Awake()
    {
        //设置窗体性质
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        RegisterButtonEvent(Confirm, (object args) =>
        {
            //清除货币
            RoleInfoManager.GetInstance().ClearCurrency();
            //清除通关信息
            RoleInfoManager.GetInstance().UpdateCheckPoint(0);
            //清除经验和等级
            RoleInfoManager.GetInstance().ClearExpAndLevel();
            //清除卡牌
            RoleInfoManager.GetInstance().ClearCardIds();
            AddTips("清除成功");
            CloseUIForm(SysDefine.SYS_DELETEDATA_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(106);
        });

        RegisterButtonEvent(Cancel, (object args) =>
        {
            CloseUIForm(SysDefine.SYS_DELETEDATA_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(106);
        });
    }
}
