using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFailPanel : BaseUIForms
{
    public GameObject BtnConfirm;

    void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.ImPeneterabla;

        RegisterButtonEvent(BtnConfirm.gameObject, (object args) =>
        {
            BattleManager.GetInstance().LeaveBattle();
            PlayClickCloseSound();
            CloseUIForm(SysDefine.SYS_BATTLEFAIL_UIFORM);
        });
    }

    private void OnEnable()
    {
        AudioManager.GetInstance().PlayEffectSound(103);
    }
}
