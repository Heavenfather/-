using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WujiangCardCell : BaseUIForms
{
    public Image icon;
    public Image bgframe;
    public Image nameFrame;
    public Text cellname;
    public Text Level;

    [HideInInspector]
    public WujiangData data;
    [HideInInspector]
    public ShowCardDetailEume type;
    [HideInInspector]
    public CardShowStyle style = CardShowStyle.HideLevel;

    private void Start()
    {
        if (data.isShowTip)
        {
            //武将卡牌的点击事件，发送data数据给详情界面做显示
            RegisterButtonEvent("Icon", (object args) =>
            {
                //在战斗外还是战斗内
                if (type == ShowCardDetailEume.InBattle)
                    {
                        OpenUIForm(SysDefine.SYS_INBATTLECARDDETAIL_UIFORM);    //打开的只是简单的介绍
                }
                else
                {
                    OpenUIForm(SysDefine.SYS_WUJIANGDETAIL_UIFORM);
                }

                object[] obj = new object[2];
                obj[0] = data;
                obj[1] = type;
                SendUIFormMessage(SysDefine.CardDetail, "Card", obj);
            });
        }

        this.name = data.Name;
        CardManager.GetInstance().ReplaceIcon(icon, data.IconId);
        CardManager.GetInstance().ReplaceBgFrameByQuanlity(bgframe, data.Quantity);

        if (style == CardShowStyle.HideLevel)
        {
            CardManager.GetInstance().ReplaceNameFrameByQuanlity(nameFrame, data.Quantity);
            cellname.text = data.Name;

            Level.gameObject.SetActive(false);
        }
        else if (style == CardShowStyle.HideName)
        {
            nameFrame.gameObject.SetActive(false);

            Level.text = "Lv." + CardManager.GetInstance().GetSaveCardLevel(data.IconId);
        }
        else if (style == CardShowStyle.Normal)
        {
            CardManager.GetInstance().ReplaceNameFrameByQuanlity(nameFrame, data.Quantity);
            cellname.text = data.Name;
            Level.text = "Lv." + CardManager.GetInstance().GetSaveCardLevel(data.IconId);

        }

        ReceiveMessage(SysDefine.CardUpgrade, onCardLevelChange);

    }

    private void onCardLevelChange(KeyValueUpdate kv)
    {
        Dictionary<int, int> value = kv.Values as Dictionary<int, int>;
        //int level = (int)kv.Values;
        if(style == CardShowStyle.Normal || style == CardShowStyle.HideName)
        {
            if (Level)
            {
                if (value.ContainsKey(data.ID))
                {
                    Level.text = "Lv." + value.TryGet(data.ID); ;
                }
            }
        }
    }



}
