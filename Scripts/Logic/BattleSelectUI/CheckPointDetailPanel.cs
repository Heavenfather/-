using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckPointDetailPanel : BaseUIForms
{
    public Text Title;
    public List<Image> Stars = new List<Image>();
    public Image RoleBg;
    public Text Description;
    public GameObject Passed;
    private int CheckPointTag;
    private bool bHasPassed;   //已经成功通关
    private int sceneid = 0;

    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.ImPeneterabla;

        base.RegisterButtonEvent("Button-Close", ( object args) => {
            PlayClickCloseSound();
            base.CloseUIForm(SysDefine.SYS_CHECKPOINTDETAI_UIFORM);
        });

        base.RegisterButtonEvent("Button-Challenge", onChallengeClick);
        Passed.SetActive(false);

        ReceiveMessage(SysDefine.BattleSelectData, onShowCheckPointDetail);

    }

    //显示关卡面板详情
    public void onShowCheckPointDetail(KeyValueUpdate kv)
    {
        object[] objs = kv.Values as object[];

        BattleCellData data = objs[0] as BattleCellData;

        Title.text = data.Title;
        for(int i = 0; i < Stars.Count; i++)
        {
            Stars[i].gameObject.SetActive(data.Difficulty > i);
        }
        CardManager.GetInstance().ReplaceCardRole(RoleBg, data.RoleID);
        Description.text = data.Description;
        CheckPointTag = data.ID;
        sceneid = data.SceneID;

        //是否已通关
        int passedId = BattleManager.GetInstance().GetPassedCheckpoint();
        Passed.SetActive(passedId >= data.ID);
        bHasPassed = passedId >= data.ID;
    }

    public void onChallengeClick(object args)
    {
        int cardcount = BattleManager.GetInstance().GetWujiangIntoBattleCardNum();
        if (cardcount <= 0)
        {
            UIManager.GetInstance().ShowTip("暂时未上阵任何武将，不得挑战!");
            return;
        }

        //判断等级
        if (CheckPointManager.GetInstance().IsEnoughChanllenge(CheckPointTag) == false)
        {
            AddTips("等级不足");
            return;
        }

        //判断前置关卡
        if (CheckPointTag > BattleManager.GetInstance().GetPassedCheckpoint()+1)
        {
            AddTips("请先挑战前置关卡");
            return;
        }

        //是否已经通关
        if (bHasPassed)
        {
            BattleManager.EntryCheckPointTag = CheckPointTag;
            BattleManager.EntrySceneid = sceneid;
            OpenUIForm(SysDefine.SYS_ISPASSED_UIFORM);
            return;
        }

        AudioManager.GetInstance().PlayEffectSound(106);

        ScenesEnum scenes = ConvertEnumToString.GetInstance().GetSceneEnumById(sceneid);
        string jumpSceneName = ConvertEnumToString.GetInstance().GetStrByEnumScenes(scenes);
        SceneManager.LoadSceneAsync(jumpSceneName);
        BattleManager.GetInstance().HideOtherPanelInBattle();
        BattleManager.EntryCheckPointTag = CheckPointTag;
        BattleManager.EntrySceneid = sceneid;

    }

}
