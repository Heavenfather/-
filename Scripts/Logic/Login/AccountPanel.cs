using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AccountPanel : BaseUIForms
{
    public InputField inputField;
    private bool _isAllNumber=true;   //是否全部都为数字

    public void Start()
    {
        //设置窗体性质
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.HideOther;
        base.CurrentUIType.UIForm_Type = UIFormType.Norlmal;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        InitInputText();

        //注册输入的字符监听
        inputField.onValueChanged.AddListener(onInputChange);
        inputField.onEndEdit.AddListener(onEditEnd);

        RegisterButtonEvent("BtnCancel", (object args) =>
        {
            //关闭当前界面，打开登录界面
            CloseUIForm(SysDefine.SYS_ACCOUNT_UIFORM);
            OpenUIForm(SysDefine.SYS_LOGIN_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(106);
        });
        RegisterButtonEvent("BtnRegister", ( object args) =>
        {
            //关闭当前界面 打开注册界面
            CloseUIForm(SysDefine.SYS_ACCOUNT_UIFORM);
            OpenUIForm(SysDefine.SYS_REGISTER_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(106);
        });
        RegisterButtonEvent("BtnEntry", onEntryClick);
    }

    void InitInputText()
    {
        AccountData data = GameDataManager.GetInstance().LoadAccountData();
        //有账号 则将输入的文本设为最近一次登录的账号
        if (data.accountData.Count > 0)
        { 
            foreach (var item in data.accountData.Keys)
            {
                inputField.text = item + "";
            }

        }
    }

    void onInputChange(string args)
    {
        string strInput = args;
        if (strInput.Length > inputField.characterLimit)
        {
            UIManager.GetInstance().ShowTip("不得大于8个字符");
            inputField.interactable = false;
            return;
        }
        else
        {
            inputField.interactable = true;
        }

    }

    void onEditEnd(string args)
    {
        char[] ch = new char[args.Length];
        ch = args.ToCharArray();
        for (int i = 0; i < args.Length; i++)
        {
            if (ch[i] < 48 || ch[i] > 57)
            {
                _isAllNumber = false;
                break;
            }
        }
    }

    void onEntryClick( object args)
    {
        //有非数字类型的
        if (_isAllNumber == false)
        {
            UIManager.GetInstance().ShowTip("账号只能为数字");
            return;
        }

        //未做任何输入
        if (inputField.text.Length <= 0)
        {
            UIManager.GetInstance().ShowTip("请输入账号");
            return;
        }

        //账户是否存在
        int inputAccount = Convert.ToInt32(inputField.text);
        if (GameDataManager.GetInstance().IsHaveAccount(inputAccount) == false)
        {
            UIManager.GetInstance().ShowTip("账号不存在");
            return;
        }

        //关闭当前界面，打开Loading界面
        CloseUIForm(SysDefine.SYS_ACCOUNT_UIFORM);
        OpenUIForm(SysDefine.SYS_LOADING_UIFORM);
        AudioManager.GetInstance().PlayEffectSound(106);
        GameDataManager.UserAccount = inputAccount;
        //赋值当前登录账号的用户信息
        GameDataManager.UserData = GameDataManager.GetInstance().GetAccountUserData(GameDataManager.UserAccount);

        inputField.onValueChanged.RemoveAllListeners();
        inputField.onEndEdit.RemoveAllListeners();
    }
}
