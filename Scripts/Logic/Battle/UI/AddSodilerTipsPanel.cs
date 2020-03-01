using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSodilerTipsPanel : BaseUIForms
{
    public Text TxtAddNum;
    public Text TxtMaxNum;
    public Button BtnDecreaseNum;
    public Button BtnAddNum;
    public Button BtnOk;
    public Button BtnCancel;

    private int InitSoldierNum = 1;
    private int MaxSoldierNum = 10;
    private string type ;

    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.Normal;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.ImPeneterabla;

        base.RegisterButtonEvent("Cancel", (object args) =>
        {
            base.CloseUIForm(SysDefine.SYS_ADDSODILERTIPS_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(106);
        });
        RegisterButtonEvent("Decrease", onDecreaseNumClick);
        RegisterButtonEvent("Add", onAddNumClick);
        RegisterButtonEvent("OK", onOkClick);

        ReceiveMessage(SysDefine.InitSodlier, InitPanelType);

    }

    private void OnEnable()
    {
        MaxSoldierNum = BattleManager.GetInstance().GetMaxCreateSoldierNum(RoleInfoManager.GetInstance().GetRoleLevel());  
        //给个初始值
        TxtAddNum.text = InitSoldierNum + "";
        TxtMaxNum.text = MaxSoldierNum+"";  //这个之后可以根据角色的能力进行改变      
    }

    private void InitPanelType(KeyValueUpdate kv)
    {
        object[] objs = kv.Values as object[];
        type = Convert.ToString(objs[0]);        
    }

    //减少生成数量
    private void onDecreaseNumClick(object args)
    {
        //最少生成一个
        if (InitSoldierNum <= 1)
            return;
        InitSoldierNum -= 1;
        BattleManager.GetInstance().AlivePlayer = InitSoldierNum;
        TxtAddNum.text = InitSoldierNum + "";
        AudioManager.GetInstance().PlayEffectSound(106);
    }
    
    //增加生成数量
    private void onAddNumClick( object args)
    {
        //数量不能大于最大的生成数
        if (InitSoldierNum >= MaxSoldierNum)
            return;

        InitSoldierNum += 1;
        TxtAddNum.text = InitSoldierNum + "";
        AudioManager.GetInstance().PlayEffectSound(106);
        //统计数量
        BattleManager.GetInstance().AlivePlayer = InitSoldierNum;
        //开始了战斗
        BattleManager.GetInstance().IsStartBattle = true;
    }
    
    private void onOkClick( object args)
    {
        //生成的是近战士兵还是远战士兵
        if (type == SysDefine.MeleeSoldier)
        {
            CreateMeleeSoldier();
        }
        else if (type == SysDefine.FarSoldier)
        {
            CreateFarSoldier();

        }
        MaxSoldierNum -= InitSoldierNum;
        UIManager.GetInstance().CloseUIForm(SysDefine.SYS_ADDSODILERTIPS_UIFORM);
        //Debug.Log("生成了" + InitSoldierNum + "个士兵，剩余还可以生成" + MaxSoldierNum + "个士兵!!!");
        if (MaxSoldierNum == 0)
            InitSoldierNum = 0;
        else
            InitSoldierNum = 1;
        TxtAddNum.text = InitSoldierNum + "";
        TxtMaxNum.text = MaxSoldierNum + "";

        AudioManager.GetInstance().PlayEffectSound(104);
    }

    private void CreateMeleeSoldier()
    {
        for (int i = 0; i < InitSoldierNum; i++)
        {
            //先给固定的属性
            GameObject go = ResourcesMgr.GetInstance().LoadResource<GameObject>("Prefabs/3DModel/Role/3000106", true);      //目前生成固定的一个对象
            go.transform.localPosition = new Vector3(i * 4, 0, i*2);
            go.tag = SysDefine.SYS_TAG_PLAYER;
            SoldierBaseAI ai = go.GetComponent<SoldierBaseAI>();
            ai.FloCurrentHealth = 1000;
            ai.IntMaxHealth = 1000;
            ai.SoldierAttack = 20;
            ai.SoldierDefence = 50;
            ai.soldierType = SoldierType.BatMan;
            ai.attackRange = 2;
            ai.walkSpeed = 2;
            ai.turnSpeed = 0.1f;
            ai.targetTagName = SysDefine.SYS_TAG_ENEMY;
            ai.SkillHurtPower = 5;

            Instantiate(go);
        }
    }

    //远战士兵 TODO.....
    private void CreateFarSoldier()
    {

    }
}
