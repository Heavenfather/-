using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IsPassedPanel : BaseUIForms
{
    public Button BtnClose;
    public Button BtnContinute;

    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.ImPeneterabla;

        RegisterButtonEvent(BtnClose.gameObject, (object args) =>
        {
            CloseUIForm(SysDefine.SYS_ISPASSED_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(105);
        });

        RegisterButtonEvent(BtnContinute.gameObject, (object args) =>
        {
            ScenesEnum scenes = ConvertEnumToString.GetInstance().GetSceneEnumById(BattleManager.EntrySceneid);
            string jumpSceneName = ConvertEnumToString.GetInstance().GetStrByEnumScenes(scenes);
            SceneManager.LoadSceneAsync(jumpSceneName);
            BattleManager.GetInstance().HideOtherPanelInBattle();
            AudioManager.GetInstance().PlayEffectSound(106);
        });

    }
}
