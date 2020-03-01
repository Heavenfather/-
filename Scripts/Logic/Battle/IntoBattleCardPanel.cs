using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class IntoBattleCardPanel : BaseUIForms
{
    public GameObject CardCell;
    public GameObject WujiangCardLayout;
    public GameObject CelueCardLyaout;

    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.ImPeneterabla;
        base.RegisterButtonEvent("Button-Close", ( object args) => {
            CloseUIForm(SysDefine.SYS_INTOBATTLECARD_UIFORM);
            AudioManager.GetInstance().PlayEffectSound(105);
        });


        ReceiveMessage(SysDefine.UpdateIntoBattleCardPanel, RemoveTargetWujiangCard);
    }

    /// <summary>
    /// 移除指定的武将卡牌
    /// </summary>
    /// <param name="kv"></param>
    void RemoveTargetWujiangCard(KeyValueUpdate kv)
    {
        WujiangData data = kv.Values as WujiangData;
        for (int i = 0; i < WujiangCardLayout.transform.childCount; i++)
        {
            Transform child = WujiangCardLayout.transform.GetChild(i);
            if (child.name == data.Name)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    private void OnEnable()
    {
        InitializeToBattleCard();        
    }

    private void InitializeToBattleCard() 
    {
        for (int i = 0; i < WujiangCardLayout.transform.childCount; i++)
        {
            Transform child = WujiangCardLayout.transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }

        int count = BattleManager.GetInstance().GetWujiangIntoBattleCardNum();
        List<int> ids = BattleManager.GetInstance().GetAllIntoBattleIds();
        
        for(int i = 0; i < count; i++)
        {
            WujiangData data = CardManager.GetInstance().FetchWujiangCardDataById(ids[i]);
            GameObject go = GetComponent<GameObject>();
            go = Instantiate(CardCell);

            go.transform.SetParent(WujiangCardLayout.transform);
            go.transform.localScale = Vector3.one;  //设置父节点后scale会莫名其妙变成60 在这里转换回来

            go.GetComponent<WujiangCardCell>().data = data;
            go.GetComponent<WujiangCardCell>().type = ShowCardDetailEume.DownBattle;
            go.GetComponent<WujiangCardCell>().style = CardShowStyle.Normal;

        }
    }

}
