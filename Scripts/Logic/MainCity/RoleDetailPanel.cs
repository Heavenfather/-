using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleDetailPanel : BaseUIForms
{
    public Button BtnClose;
    public InputField inputFieldName;
    public Image BtnWriteName;
    public Button BtnReturnLogin;
    public Button BtnDeleteData;
    public Button BtnApply;
    public Image ImgHead;
    public Text TxtCardNum;
    public Text TxtPointInfo;
    public Text TxtLevel;
    public Text TxtSoundNum;
    public Button BtnAddSound;
    public Button BtnDecreaseSound;
    public GameObject NameMaskGo;
    public Text TxtRoleID;
    public GameObject MaskButton;

    private UserData userData;
    private string changedName;
    private float soundVolume;

    public void Awake()
    {
        //设置窗体性质
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        RegisterButtonEvent(BtnReturnLogin.gameObject, (object args) =>
        {
            //退出登录
            OpenUIForm(SysDefine.SYS_LOGIN_UIFORM);
            CloseUIForm(SysDefine.SYS_MAINCITY_UIFORM);
            CloseUIForm(SysDefine.SYS_ROLEDETAIL_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(106);
        });

        RegisterButtonEvent(BtnDeleteData.gameObject, ( object args) =>
        {
            //清除数据
            OpenUIForm(SysDefine.SYS_DELETEDATA_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(106);
        });

        RegisterButtonEvent(BtnClose.gameObject, ( object args) =>
        {
            PlayClickCloseSound();
            CloseUIForm(SysDefine.SYS_ROLEDETAIL_UIFORM);
            inputFieldName.onEndEdit.RemoveAllListeners();
            AudioManager.GetInstance().PlayEffectSound(101);
        });

        RegisterButtonEvent(BtnWriteName.gameObject, ( object args) =>
        {
            //将挡住不能点击的遮罩隐藏
            NameMaskGo.SetActive(false);
            MaskButton.SetActive(true); //将挡住按钮的遮罩打开
            inputFieldName.Select();        //点击的时候立刻就是选择编辑
            AudioManager.GetInstance().PlayEffectSound(106);
        });        

        //设置一次初始的音量值
        soundVolume = AudioManager.GetInstance().GetLocalSoundVolume();
        SetShowSoundValue();
        RegisterButtonEvent(BtnAddSound.gameObject, (object args) =>
        {
            //加大音量
            if (soundVolume >= 1)
            {
                return;
            }
            soundVolume += 0.1f;
            TxtSoundNum.text = Convert.ToDouble(soundVolume).ToString("P");     //转换成保留两位数的百分比
            AudioManager.GetInstance().PlayEffectSound(106);
        });

        RegisterButtonEvent(BtnDecreaseSound.gameObject, ( object args) =>
        {
            //减少音量
            if (soundVolume <= 0)
            {
                return;
            }
            soundVolume -= 0.1f;
            TxtSoundNum.text = Convert.ToDouble(soundVolume).ToString("P");
            AudioManager.GetInstance().PlayEffectSound(106);
        });

        MaskButton.SetActive(false);
        inputFieldName.onEndEdit.AddListener(onInputEditEnd);

        RegisterButtonEvent(BtnApply.gameObject, onApplyChange);

        userData = GameDataManager.UserData;
        inputFieldName.text = userData.Name;
        TxtLevel.text = "Lv."+userData.Level;
        CardManager.GetInstance().ReplaceIcon(ImgHead, CardManager.GetInstance().TransitionHeadID(userData.HeadId));
        TxtPointInfo.text = "通关"+userData.CheckPoint + "";
        TxtCardNum.text = CardManager.GetInstance().HaveCardIds().Count + "";
        TxtRoleID.text = "ID:" + GameDataManager.GetInstance().LoadUserData(GameDataManager.UserAccount + "").Id + "";

        ReceiveMessage(SysDefine.RoleLevelChanged, onRoleLevelChanged);
        ReceiveMessage(SysDefine.HaveCardNumChanged, (KeyValueUpdate kv) =>
         {
             int num = Convert.ToInt32(kv.Values);
             TxtCardNum.text = num + "";
         });
        ReceiveMessage(SysDefine.UpdateUserCheckPoint, (KeyValueUpdate kv) =>
         {
             int point = Convert.ToInt32(kv.Values);
             TxtPointInfo.text = "通关" + point;
         });

    }

    public void SetShowSoundValue()
    {
        TxtSoundNum.text = Convert.ToDouble(soundVolume).ToString("P");
    }

    void onInputEditEnd(string args)
    {
        NameMaskGo.SetActive(true); //输入完成后再遮住
        changedName = args;
        MaskButton.SetActive(false);
    }

    void onApplyChange(object args)
    {
        //音量
        AudioManager.GetInstance().ChangeBgSoundVolume(soundVolume);
        AudioManager.GetInstance().ChangeEffectSoundVolume(soundVolume);
        //名称
        if (!string.IsNullOrEmpty(changedName))
        {
            GameDataManager.GetInstance().SaveUserName(userData.Id + "", changedName);
            SendUIFormMessage(SysDefine.ChangeName, "Name", null);
        }
        AddTips("应用成功");
        AudioManager.GetInstance().PlayEffectSound(106);
    }

    private void onRoleLevelChanged(KeyValueUpdate kv)
    {
        int level = Convert.ToInt32(kv.Values);
        TxtLevel.text = "Lv." + level;

    }

}
