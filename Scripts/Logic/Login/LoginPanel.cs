using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TableConfigs;
using Tools;

public class LoginPanel : BaseUIForms
{

    public void Start()
    {
        //设置窗体性质
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.HideOther; 
        base.CurrentUIType.UIForm_Type = UIFormType.Norlmal;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

        AudioManager.GetInstance().PlayBgSound(2);   //进入游戏的时候播一个背景音  其实应该做一个状态机来管理不同的状态的，声音也应该放在那里面播放管理

        base.RegisterButtonEvent("Button-EnterGame", OnStartClick,transform.name);
        
        base.RegisterButtonEvent("UserAccount", (object args) =>
        {
            CloseUIForm(SysDefine.SYS_LOGIN_UIFORM);
            OpenUIForm(SysDefine.SYS_ACCOUNT_UIFORM);
        });
    }


    public void OnStartClick(object args)
    {
        //如果有账号登录过，可以直接跳过账户那个界面，如果需要切换账号再点击账号那个按钮
        List<int> allAcount = GameDataManager.GetInstance().GetAllAccount();
        if (allAcount.Count <= 0)
        {
            //本地没有账号，打开注册账号界面
            base.OpenUIForm(SysDefine.SYS_REGISTER_UIFORM);
        }
        else
        {
            //本地有账号，取最近的一次登录的账号 然后直接进入游戏
            GameDataManager.UserAccount = allAcount[allAcount.Count - 1];   //赋值当前登录的账号
            GameDataManager.UserData = GameDataManager.GetInstance().GetAccountUserData(GameDataManager.UserAccount); //赋值当前登录账号的用户信息            

            base.OpenUIForm(SysDefine.SYS_LOADING_UIFORM);
        }

    }
}
