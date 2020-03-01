using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BaseUIForms
{
    public InputField inputField;
    private bool _isAllNumber = true;   //是否全部都为数字

    public void Start()
    {
        //设置窗体性质
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.HideOther;
        base.CurrentUIType.UIForm_Type = UIFormType.Norlmal;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        //注册输入的字符监听
        inputField.onValueChanged.AddListener(onInputChange);
        inputField.onEndEdit.AddListener(onEditEnd);

        RegisterButtonEvent("BtnCancel", ( object args) =>
        {
            //关闭当前界面，打开账号界面
            CloseUIForm(SysDefine.SYS_REGISTER_UIFORM);
            OpenUIForm(SysDefine.SYS_ACCOUNT_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(106);
        });
        RegisterButtonEvent("BtnOK", onOKClick);
    }

    void onOKClick(object args)
    {
        if (_isAllNumber == false)
        {
            UIManager.GetInstance().ShowTip("账号只能为数字格式");
            return;
        }

        //账号已存在
        int inputAccount = Convert.ToInt32(inputField.text);
        if (GameDataManager.GetInstance().IsHaveAccount(inputAccount))
        {
            UIManager.GetInstance().ShowTip("账号已存在");
            return;
        }
        //将新的账号保存到本地
        GameDataManager.GetInstance().AddNewAccount(Convert.ToInt32(inputAccount));
        UIManager.GetInstance().ShowTip("注册成功");
        AudioManager.GetInstance().PlayEffectSound(106);

        //关闭当前界面，打开账号界面
        CloseUIForm(SysDefine.SYS_REGISTER_UIFORM);
        OpenUIForm(SysDefine.SYS_ACCOUNT_UIFORM);

        inputField.onValueChanged.RemoveAllListeners();
        inputField.onEndEdit.RemoveAllListeners();
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
}
