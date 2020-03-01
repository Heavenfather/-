using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 提示飘字文本
/// </summary>
public class TipText : MonoBehaviour
{
    //public string _showTipStr;

    //初始的位置
    private Vector3 _originalPos;
    //移动至消失所需要的时间
    private float _moveTime=1.5f;    
    void Start()
    {
        _originalPos = this.transform.localPosition;
        this.gameObject.SetActive(false);   //默认是不显示的

        MessageCenter.AddMessageListener(SysDefine.TipsEvent, onShowTip);
        //Text txt = this.GetComponent<Text>();
        //txt.text = _showTipStr;

        //this.transform.DOLocalMoveY(180, _moveTime).OnComplete(() =>
        //{
        //    //this.gameObject.SetActive(false);
        //    //this.transform.localPosition = _originalPos;
        //    //回收到池子里
        //    Destroy(this.gameObject);

        //});
    }

    void onShowTip(KeyValueUpdate kv)
    {
        string txt = kv.Values + "";
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<Text>().text = txt;
        //还不是很通用，如果其他项目用到的话还每次都要引入DOTween挺麻烦的
        this.transform.DOLocalMoveY(180, _moveTime).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
            this.transform.localPosition = _originalPos;
        });
        Debug.Log("提示的内容:" + kv.Values);
    }



}
