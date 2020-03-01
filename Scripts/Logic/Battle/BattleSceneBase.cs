using UnityEngine;
using TableConfigs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class BattleSceneBase : MonoBehaviour
{
    [Header("关卡标签第几关")]
    public int PointTag = 1;
    public Transform SpawPoint;
    
    private int m_initNum=0;
    private int m_soldierId;
    private int m_maxHp;
    private int m_attackRange;
    private int m_moveSpeed;
    private int m_attackPower;
    private int m_defence;
    private int m_type;
    private int m_skillPower;
    private string m_attackTag;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("InitSodilerInfo");
    }
    

    //初始化士兵信息
    IEnumerator InitSodilerInfo()
    {
        yield return new WaitForSeconds(0.3f);

        Table_PointInfo_DefineData data = TableManager.GetInstance().GetDataById("PointInfo", PointTag) as Table_PointInfo_DefineData;

        //设置初始的士兵数量
        BattleManager.GetInstance().AliveEnemys = data.SoldierInfo.Split(';').Length;

        //按照格式：守卫id+最大血量+攻击范围+移动速度+攻击力+防御力+类型+技能攻击系数;
        //foreach (var data in datas.mDataMap)
        //{
        //    if(data.Value.Id == PointTag)
        //    {
                m_initNum = data.SoldierInfo.Split(';').Length;
                for (int i = 0; i < m_initNum; i++)
                {
                    string info = data.SoldierInfo.Split(';')[i];
                    m_soldierId = Convert.ToInt32(info.Split(',')[0]);
                    m_maxHp = Convert.ToInt32(info.Split(',')[1]);
                    m_attackRange = Convert.ToInt32(info.Split(',')[2]);
                    m_moveSpeed = Convert.ToInt32(info.Split(',')[3]);
                    m_attackPower = Convert.ToInt32(info.Split(',')[4]);
                    m_defence = Convert.ToInt32(info.Split(',')[5]);
                    m_type = Convert.ToInt32(info.Split(',')[6]);
                    m_skillPower = Convert.ToInt32(info.Split(',')[7]);
                    m_attackTag = data.AttackTarget;

                    InitSoldierProperty(i);
                    //Debug.Log("id=" + m_soldierId + ";HP=" + m_maxHp + ";attackRange=" + m_attackRange + ";moveSpeed=" + m_moveSpeed + ";attackPower=" + m_attackPower + ";defence=" + m_defence + ";type=" + m_type + ";skillPower=" + m_skillPower);
                }
            //}
        //}
    }

    //初始化士兵的属性
    private void InitSoldierProperty(int index)
    {
        GameObject go = ResourcesMgr.GetInstance().LoadResource<GameObject>("Prefabs/3DModel/Role/" + m_soldierId, true);
        go.transform.localPosition = new Vector3(SpawPoint.position.x+ index* 2, SpawPoint.position.y, SpawPoint.position.z);      //这里修改生成的位置

        go.tag = SysDefine.SYS_TAG_ENEMY;
        SoldierBaseAI ai = go.GetComponent<SoldierBaseAI>();
        ai.FloCurrentHealth = m_maxHp;
        ai.IntMaxHealth = m_maxHp;
        ai.SoldierAttack = m_attackPower;
        ai.SoldierDefence = m_defence;
        ai.attackRange = m_attackRange;
        ai.walkSpeed = 3.5f;
        ai.turnSpeed = 0.1f;
        ai.targetTagName = m_attackTag;
        ai.SkillHurtPower = m_skillPower;
        SoldierType type=SoldierType.BatMan;
        if (m_type == 1)
        {
            type = SoldierType.BatMan;
        }else if (m_type == 2)
        {
            type = SoldierType.Elite;
        }
        ai.soldierType = type;

        Instantiate(go);
    }
    
}
