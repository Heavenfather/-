using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 初始化战斗中的人物
/// </summary>
public class BattleRoleInit : MonoBehaviour
{
    private int m_initNum = 10;

    // Start is called before the first frame update
    void Start()
    {
        Initialized();
    }

    public void Initialized()
    {
        for (int i = 0; i < m_initNum; i++)
        {
            GameObject go = ResourcesMgr.GetInstance().LoadResource<GameObject>("Prefabs/3DModel/Role/3000106", true);

            go.transform.localPosition = new Vector3(i * 2, 0, 0);
            
            go.tag = "Player";
            SoldierBaseAI ai = go.GetComponent<SoldierBaseAI>();
            ai.FloCurrentHealth = 1000;
            ai.IntMaxHealth = 1000;
            ai.SoldierAttack = 10;
            ai.SoldierDefence = 100;
            ai.attackRange = 5f;
            ai.walkSpeed = 2f;
            ai.turnSpeed = 0.2f;
            ai.targetTagName = "Enemy";
            ai.soldierType = SoldierType.Elite;

            Instantiate(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
