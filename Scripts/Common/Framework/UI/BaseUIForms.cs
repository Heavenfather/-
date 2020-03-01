using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

/*UI窗体的父类
 * 功能：定义所有UI窗体的父类
 * 定义四个生命周期
 * 1.Display 显示状态
 * 2.Hiding 隐藏状态
 * 3.ReDisplay 再显示状态
 * 4.Freeze 冻结状态
 *
 */

public class BaseUIForms : MonoBehaviour {

    //字段
    private UIType _CurrentUIType=new UIType();  //当前的UI类型

    protected UIManager uiMgr;
    private string uiFormName;        //需要关闭的UI窗体名称
    //private string m_btnClose;         //关闭按钮名称
    

    #region 窗口的四种状态
    /// <summary>
    /// 属性  当前UI窗体类型
    /// </summary>
    public UIType CurrentUIType
    {
        get{ return _CurrentUIType;}
        set{_CurrentUIType = value; }
    }

    protected string UiFormName { get => uiFormName; set => uiFormName = value; }
    //public string BtnClose { get => m_btnClose; set => m_btnClose = value; }

    /// <summary>
    /// 显示状态
    /// </summary>
    public virtual void Display()
    {
        this.gameObject.SetActive(true);
        //设置UI遮罩
        if (_CurrentUIType.UIForm_Type == UIFormType.PopUp)
        {
            UIMaskManager.GetInstance().SetMaskWindow(this.gameObject,CurrentUIType.UIForm_Luceny);
        }

    }

    /// <summary>
    /// 隐藏状态
    /// </summary>
    public virtual void Hiding()
    {
        this.gameObject.SetActive(false);

        //取消UI遮罩
        if (_CurrentUIType.UIForm_Type == UIFormType.PopUp)
        {
            UIMaskManager.GetInstance().CancelMaskWindow();
        }
    }

    /// <summary>
    /// 再显示状态
    /// </summary>
    public virtual void ReDisplay()
    {
        this.gameObject.SetActive(true);
        //设置UI遮罩
        if (_CurrentUIType.UIForm_Type == UIFormType.PopUp)
        {
            UIMaskManager.GetInstance().SetMaskWindow(this.gameObject,CurrentUIType.UIForm_Luceny);
        }
    }

    /// <summary>
    /// 冻结状态
    /// </summary>
    public virtual void Freeze()
    {
        this.gameObject.SetActive(true);
    }


    #endregion

    #region 封装子类常用方法
    /// <summary>
    /// 给按钮注册方法
    /// </summary>
    /// <param name="btnName">按钮名称</param>
    /// <param name="dele">委托事件</param>
    protected void RegisterButtonEvent(string btnName, EventTriggerListener.VoidDelegate dele,object args=null)
    {
        Transform traLoginUISysButton = UnityHelper.FindTheChildNode(this.gameObject, btnName);

        //给按钮注册方法
        if (traLoginUISysButton != null)
        {
            //通过自己写的脚本把事件动态注册到这个按钮上
            EventTriggerListener.Get(traLoginUISysButton.gameObject,args).onClick = dele;
        }
    }

    /// <summary>
    /// 给按钮注册方法 不使用递归
    /// </summary>
    /// <param name="go">注册对象</param>
    /// <param name="dele">委托事件</param>
    protected void RegisterButtonEvent(GameObject go, EventTriggerListener.VoidDelegate dele,object args=null)
    {
        //给按钮注册方法
        if (go != null)
        {
            //通过自己写的脚本把事件动态注册到这个按钮上
            EventTriggerListener.Get(go.gameObject,args).onClick = dele;
        }
    }

    /// <summary>
    /// 打开一个UI窗体
    /// </summary>
    /// <param name="UIFormName"></param>
    protected void OpenUIForm(string UIFormName)
    {
        UIManager.GetInstance().ShowUIForm(UIFormName);
    }


    /// <summary>
    /// 关闭UI窗体
    /// </summary>
    protected void CloseUIForm(string UIFormName)
    {
        UIManager.GetInstance().CloseUIForm(UIFormName);
    }

    /// <summary>
    /// 弹提示
    /// </summary>
    /// <param name="value"></param>
    protected void AddTips(string value)
    {
        UIManager.GetInstance().ShowTip(value);
    }

    /// <summary>
    /// 发送UI窗体间数据传递的消息（允许传多条信息）
    /// </summary>
    /// <param name="msgType">消息区分的大类</param>
    /// <param name="msgName">消息的小类</param>
    /// <param name="msgDetail">传的数据</param>
    protected void SendUIFormMessage(string msgType, string msgName, object[] msgDetail)
    {
        KeyValueUpdate kv = new KeyValueUpdate(msgName, msgDetail);
        MessageCenter.SendMessage(msgType, kv);
    }
    /// <summary>
    /// 发送UI窗体间数据传递的消息 单条数据
    /// </summary>
    /// <param name="msgType">消息区分的大类</param>
    /// <param name="msgName">消息的小类</param>
    /// <param name="msgDetail">传的数据</param>
    protected void SendUIFormMessage(string msgType, string msgName, object msgDetail)
    {
        KeyValueUpdate kv = new KeyValueUpdate(msgName, msgDetail);
        MessageCenter.SendMessage(msgType, kv);
    }

    /// <summary>
    /// 播放点击关闭界面时的音效
    /// </summary>
    protected void PlayClickCloseSound()
    {
        AudioManager.GetInstance().PlayEffectSound(101);
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="messagType">消息分类</param>
    /// <param name="handler">消息委托</param>
    public void ReceiveMessage(string messagType, MessageCenter.DelMessageDelivery handler)
    {
        MessageCenter.AddMessageListener(messagType, handler);
    }


    #endregion

}
