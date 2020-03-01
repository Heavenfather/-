using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


//UI管理器  作用：是整个UI框架的核心，用户通过这个脚本，来实现框架绝大多数的功能实现
public class UIManager : MonoBehaviour
{
    //UI窗体预设路径  参数1：窗体预设名称 参数2：预设的资源路径
    private Dictionary<string, string> _DictionaryPaths;
    //缓存所有UI窗体
    private Dictionary<string, BaseUIForms> _DicAllUiFormses;
    //当前显示的UI窗体
    private Dictionary<string, BaseUIForms> _DicCurrentUIForm;
    //利用栈的形式来处理当前显示的弹出框（即具备方向切换属性）
    private Stack<BaseUIForms> _StaCurrentUIForm;
    
    //UI根节点
    private Transform _TraCanvasTranform = null;

    //全屏幕显示的节点
    private Transform _TraNormal = null;

    //固定显示的节点
    private Transform _TraFixed = null;

    //弹出节点
    private Transform _TraPopUp = null;

    //UI管理脚本的节点
    private Transform _UIScripts = null;

    private static UIManager _instance = null;

    //得到实例
    public static UIManager GetInstance()
    {
        if (_instance == null)
        {
            _instance=new GameObject("_UIManager").AddComponent<UIManager>();   //为了保证这个脚本是挂载在一个对象上
        }

        return _instance;
    }
    
    //初始化核心数据，加载“UI窗体路径”到集合中
    void Awake()
    {
        //字段初始化
        _DicAllUiFormses=new Dictionary<string, BaseUIForms>();
        _DicCurrentUIForm=new Dictionary<string, BaseUIForms>();
        _DictionaryPaths=new Dictionary<string, string>();
        _StaCurrentUIForm=new Stack<BaseUIForms>();

        //初始化加载（根UI窗体） canvas预设
        InitRootCanvasLoading();

        //得到各个节点  根节点、全屏节点、固定节点、弹出节点
        _TraCanvasTranform = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS).transform;

        _TraNormal = UnityHelper.FindTheChildNode(_TraCanvasTranform.gameObject, SysDefine.SYS_NODE_NORMAL);
        _TraFixed = UnityHelper.FindTheChildNode(_TraCanvasTranform.gameObject, SysDefine.SYS_NODE_FIXED);
        _TraPopUp = UnityHelper.FindTheChildNode(_TraCanvasTranform.gameObject, SysDefine.SYS_NODE_POPUP);
        _UIScripts = UnityHelper.FindTheChildNode(_TraCanvasTranform.gameObject, SysDefine.SYS_NODE_SCRIPTSMANAGER);

        //把本脚本作为根UI窗体的子节点
        this.gameObject.transform.SetParent(_UIScripts,false);

        //根UI窗体 在场景转换的时候不允许销毁
        DontDestroyOnLoad(_TraCanvasTranform);

        //初始化“UI窗体的预设”路径数据
        if (_DictionaryPaths != null)
        {
            InitUIFormPath();

        }
       
    }

    /// <summary>
    /// 初始化UI窗体的路径数据
    /// </summary>
    private void InitUIFormPath()
    {
        IConfigManager configManager=new ConfigManagerByJson(SysDefine.SYS_PATH_UIFORMJSONINFO);
        if (configManager != null)
        {
            _DictionaryPaths = configManager.AppSetting;
        }
    }

    /// <summary>
    /// 显示UI窗体
    ///     功能：
    ///         1.加载与判断指定的UI窗体的名称，加载到“所有UI窗体”缓存集合中
    ///         2.根据不同的UI窗体的显示模式，分别做不同 的加载处理
    /// </summary>
    /// <param name="UIFormName">参数是UI窗体预设名称</param>
    public void ShowUIForm(string UIFormName)
    {
        BaseUIForms baseUiForms = null;        //UI窗体基类

        //参数的检查
        if(string.IsNullOrEmpty(UIFormName))
            return;

        //根据窗体名称，加载到窗体缓存集合中
        baseUiForms = LoadFormToAllUIFormCatch(UIFormName);
        if (baseUiForms == null)
        {
            Debug.LogError("窗体不在缓存集合中：" + UIFormName+"   请检查窗体路径");
            return;
        }

        //是否清空“栈集合”中的数据
        if (baseUiForms.CurrentUIType.IsClearStack)
        {
            ClearStackArray();
        }

        //根据不同的UI窗体的显示模式，分别做不同的加载处理
        switch (baseUiForms.CurrentUIType.UIForm_ShowMode)
        {
            case UIFormShowMode.Normal:     //普通显示UI窗口模式
                //把当前窗口加载到“当前窗体”集合中
                LoadUIToCurrentForm(UIFormName);
                break;
            case UIFormShowMode.ReverseChange:  //需要反向切换模式
                PushUIFormToStack(UIFormName);
                break;
            case UIFormShowMode.HideOther:      //隐藏其它模式
                EnterUIFormAndHideOther(UIFormName);
                break;
            default:
                break;  
        }
    }

    /// <summary>
    /// 关闭（返回上一个）窗体
    /// </summary>
    /// <param name="UIFormName">窗体名称</param>
    public void CloseUIForm(string UIFormName)
    {
        BaseUIForms baseUiForms;
        //参数检查
        if (string.IsNullOrEmpty(UIFormName))
        {
            Debug.LogError("窗体不存在" + UIFormName);
            return;
        }
        //“所有窗体”集合中，如果没有则返回
        _DicAllUiFormses.TryGetValue(UIFormName, out baseUiForms);
        if (baseUiForms == null)
        {
            Debug.LogError("窗体未添加到集合中" + UIFormName);
            return;
        }

        //根据不同的窗体显示模式，做不同的关闭处理
        switch (baseUiForms.CurrentUIType.UIForm_ShowMode)
        {   
            case UIFormShowMode.Normal:             //普通窗体关闭处理
                ExitUIForm(UIFormName);
                break;
            case UIFormShowMode.ReverseChange:      //反向切换关闭处理
                PopUIForm();
                break;
            case UIFormShowMode.HideOther:          //隐藏其它模式关闭处理
                ExitUIFormAndDisplayOther(UIFormName);
                break;
            default:
                break;              
        }

    }
    
    
    /// <summary>
    /// 显示文本  还需要优化 应该用一个队列把需要弹的提示存起来 弹提示的是否应该从这个队列里面拿 显示完成后再pop出去这个队列 TODO...
    /// </summary>
    /// <param name="obj"></param>
    public void ShowTip(string value)
    {
        object obj = new object();
        obj = value;
        KeyValueUpdate kv = new KeyValueUpdate("Tips", obj);
        MessageCenter.SendMessage(SysDefine.TipsEvent, kv);
        //GameObject go = ResourcesMgr.GetInstance().LoadAsset("TipText", true);
        //go.transform.SetParent(_TraPopUp);
        //go.transform.localScale = Vector3.one;
        //go.transform.localPosition = new Vector3(0, -8, 0);
        //TipText txt = go.GetComponent<TipText>();
        //txt._showTipStr = value;
    }

    #region 显示UI管理器内部核心数据，测试用
    /// <summary>
    /// 显示所有UI窗体的数量
    /// </summary>
    /// <returns>“所有UI窗体”集合数量</returns>
    public int ShowAllUIFormCount()
    {
        if (_DicAllUiFormses != null)
        {
            return _DicAllUiFormses.Count;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 显示“正在显示UI窗体”集合数量
    /// </summary>
    /// <returns>“正在显示UI窗体”集合数量</returns>
    public int ShowCurrentUIFormCount()
    {
        if (_DicCurrentUIForm != null)
        {
            return _DicCurrentUIForm.Count;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 显示“栈集合”中的数量
    /// </summary>
    /// <returns>“栈集合”中的数量</returns>
    public int ShowCurrentStackUIFormCount()
    {
        if (_StaCurrentUIForm != null)
        {
            return _StaCurrentUIForm.Count;
        }
        else
        {
            return 0;
        }
    }
    
    #endregion

    #region 私有方法
    //初始化加载（根UI窗体）canvas预设
    private void InitRootCanvasLoading()
    {
        //Resources.Load();
        ResourcesMgr.GetInstance().LoadAsset(SysDefine.SYS_PATH_CANVAS,false);

    }

    /// <summary>
    /// 加载与判断指定的UI窗体的名称，加载到“所有UI窗体”缓存集合中
    /// </summary>
    /// <param name="UIFormsName">UI窗体预设名称</param>
    /// <returns></returns>
    private BaseUIForms LoadFormToAllUIFormCatch(string UIFormsName)
    {
        BaseUIForms baseUiFormsResult = null;       //加载的返回UI窗体基类

        //先尝试查询里面有没有值.没有再做判断向里面加载
        _DicAllUiFormses.TryGetValue(UIFormsName, out baseUiFormsResult);
        if (baseUiFormsResult == null)
        {
            //加载指定路径的UI窗体
            baseUiFormsResult= LoadUIForm(UIFormsName);

        }

        return baseUiFormsResult;
    }

    /// <summary>
    /// 加载指定名称UI窗体
    /// 功能：
    ///     1.根据“UI窗体名称”，加载预设克隆体
    ///     2.根据不同预设克隆体中带的脚本中不同的“位置信息”，加载到根窗体下不同的节点
    ///     3.隐藏刚创建的UI克隆体
    ///     4.把克隆体加入到所有UI窗体的缓存集合中
    /// </summary>
    /// <param name="UIFormName">UI窗体名称</param>
    /// <returns></returns>
    private BaseUIForms LoadUIForm(string UIFormName)
    {
        string strUIFormPath = null;    //UI窗体的查询路径
        GameObject goCloneUIPrefab = null;  //创建的UI克隆体
        BaseUIForms baseUiForms = null;  //窗体基类

        //根据UI窗体名称，得到对应的加载路径
        _DictionaryPaths.TryGetValue(UIFormName, out strUIFormPath);


        //根据UI窗体名称，加载预设克隆体
        if (!string.IsNullOrEmpty(strUIFormPath))
        {
            goCloneUIPrefab = ResourcesMgr.GetInstance().LoadAsset(strUIFormPath, false);
        }

        //设置UI克隆体的父节点  根据克隆体中带的脚本中不同的“位置信息”
        if (_TraCanvasTranform != null && goCloneUIPrefab != null)
        {
            baseUiForms = goCloneUIPrefab.GetComponent<BaseUIForms>();        //得到这个克隆体里面的BaseUIForms脚本

            if (baseUiForms == null)
            {
                Debug.LogError("baseUIForms为空，请先确认窗体预设对象是否已经加载了子类脚本,UIFormName="+UIFormName);
                return null;
            }

            switch (baseUiForms.CurrentUIType.UIForm_Type)
            {
                case UIFormType.Norlmal:        //普通窗体节点
                    goCloneUIPrefab.transform.SetParent(_TraNormal,false);
                    break;
                case UIFormType.Fixed:          //固定窗体节点
                    goCloneUIPrefab.transform.SetParent(_TraFixed, false);
                    break;
                case UIFormType.PopUp:          //弹出窗体节点
                    goCloneUIPrefab.transform.SetParent(_TraPopUp, false);
                    break;
            }

            //设置隐藏   初始时应该是隐藏的，因为不知道需不需要一开始就显示
            goCloneUIPrefab.SetActive(false);

            //把克隆体加入到所有UI窗体缓存集合中
            _DicAllUiFormses.Add(UIFormName,baseUiForms);

            return baseUiForms;
        }

        else
        {
            Debug.LogError("_TraCanvasTranform != null && goCloneUIPrefab != null，请检查,参数，UIFormName="+UIFormName);
        }

        Debug.LogError(GetType()+"其他错误");
        return null;

    }

    /// <summary>
    /// 把当前窗口加载到“当前窗体”集合中
    /// </summary>
    /// <param name="UIFormName">UI窗体预设名称</param>
    private void LoadUIToCurrentForm(string UIFormName)
    {
        BaseUIForms baseUiForm;                 //UI窗体基类
        BaseUIForms baseUiFormsByAllUIForm;     //从“所有窗体”集合中得到的窗体

        //首先先检查当前的显示窗口中是否已经有这个预设，如果已经有了就直接返回
        _DicCurrentUIForm.TryGetValue(UIFormName, out baseUiForm);
        if(baseUiForm!=null) return;

        //把当前的窗体加载到“正在显示”的集合中来
        _DicAllUiFormses.TryGetValue(UIFormName, out baseUiFormsByAllUIForm); //如果_DicCurrentUIForm里面没有，就从_DicAllUiFormses里面拿出来，这个里面肯定是有的
        if (baseUiFormsByAllUIForm != null)
        {
            _DicCurrentUIForm.Add(UIFormName,baseUiFormsByAllUIForm);       //从“所有窗体”集合中拿到之后，就添加到“当前显示”窗体集合中
            baseUiFormsByAllUIForm.Display();       //显示窗口
        }

    }

    /// <summary>
    /// 弹出的所有UI窗体作进栈处理
    /// </summary>
    /// <param name="UIFormName">UI窗体名称</param>
    private void PushUIFormToStack(string UIFormName)
    {
        BaseUIForms baseUiForms;
        //先检查栈中有没有其他UI窗体，如果有则冻结其它窗体
        if (_StaCurrentUIForm.Count > 0)
        {
            BaseUIForms topUiForms = _StaCurrentUIForm.Peek();
            topUiForms.Freeze();
        }

        //检查“所有UI窗体”集合中是否有指定弹出的UI窗体，有则让它显示出来
        _DicAllUiFormses.TryGetValue(UIFormName, out baseUiForms);
        if (baseUiForms != null)
        {
            //显示当前窗口
            baseUiForms.Display();
            //将指定的UI窗体作进栈处理
            _StaCurrentUIForm.Push(baseUiForms);
        }
        else
        {
            Debug.LogError("baseUiForms == null.请检查.参数UIFormName="+UIFormName);
        }
    }

    /// <summary>
    /// 普通窗体关闭
    /// </summary>
    /// <param name="UIFormName">窗体名称</param>
    private void ExitUIForm(string UIFormName)
    {
        BaseUIForms baseUiForms;
        //检查“当前显示窗体”集合中是否存在，如果不存在则直接返回
        _DicCurrentUIForm.TryGetValue(UIFormName, out baseUiForms);
        if(baseUiForms==null)
            return;

        //把当前指定显示的UI窗体作关闭处理，且从“当前显示窗体”集合中移除
        baseUiForms.Hiding();
        _DicCurrentUIForm.Remove(UIFormName);
    }

    /// <summary>
    /// UI窗体出栈逻辑  用于具有方向切换属性的窗体
    /// </summary>
    private void PopUIForm()
    {
        if (_StaCurrentUIForm.Count >= 2)
        {
            //出栈处理
            BaseUIForms topUiForms = _StaCurrentUIForm.Pop();
            //UI窗体出栈后做隐藏处理
            topUiForms.Hiding();

            //取得栈顶元素  让栈顶的UI窗体作再显示处理
            BaseUIForms nextUiForms = _StaCurrentUIForm.Peek();
            nextUiForms.ReDisplay();

        }
        else if (_StaCurrentUIForm.Count == 1)
        {
            //出栈处理
            BaseUIForms topUiForms = _StaCurrentUIForm.Pop();
            //UI窗体出栈后做隐藏处理 
            topUiForms.Hiding();
        }
    }

    /// <summary>
    /// 打开窗体，且隐藏其它窗体  如场景切换时
    /// </summary>
    /// <param name="UIFormName">打开指定UI窗体的名称</param>
    private void EnterUIFormAndHideOther(string UIFormName)
    {
        BaseUIForms baseUiForms;
        BaseUIForms UiFormByAllDic;

        //参数检查
        if(string.IsNullOrEmpty(UIFormName))
            return;
        _DicCurrentUIForm.TryGetValue(UIFormName, out baseUiForms);     //检查当前显示UI窗体集合中是否有元素
        if(baseUiForms!=null)
            return;

        //把“正在显示UI窗体”集合和“栈集合”中所有的元素都做隐藏处理
        foreach (BaseUIForms baseUi in _DicCurrentUIForm.Values)
        {
            baseUi.Hiding();
        }

        foreach (BaseUIForms baseUi in _StaCurrentUIForm)
        {
            baseUi.Hiding();
        }

        //把指定打开的UI窗体加入到“正在显示UI窗体”集合中，并且做显示处理
        _DicAllUiFormses.TryGetValue(UIFormName, out UiFormByAllDic);
        if (UiFormByAllDic != null)
        {
            _DicCurrentUIForm.Add(UIFormName,UiFormByAllDic);
            UiFormByAllDic.Display();
        }

    }

    /// <summary>
    /// 关闭窗体，且显示其它窗体  如场景切换时
    /// </summary>
    /// <param name="UIFormName">关闭指定UI窗体的名称</param>
    private void ExitUIFormAndDisplayOther(string UIFormName)
    {
        BaseUIForms baseUiForms;

        //参数检查
        if (string.IsNullOrEmpty(UIFormName))
            return;
        _DicCurrentUIForm.TryGetValue(UIFormName, out baseUiForms);     //检查当前显示UI窗体集合中是否有元素
        if (baseUiForms == null)
            return;

        //当前窗体设置为隐藏状态，且从“正在显示UI窗体”集合中移除
        baseUiForms.Hiding();
        _DicCurrentUIForm.Remove(UIFormName);
        

        //把“正在显示UI窗体”集合和“栈集合”中所有的元素都做再显示处理
        foreach (BaseUIForms baseUi in _DicCurrentUIForm.Values)
        {
            baseUi.ReDisplay();
        }

        foreach (BaseUIForms baseUi in _StaCurrentUIForm)
        {
            baseUi.ReDisplay();
        }

    }

    private bool ClearStackArray()
    {
        if (_StaCurrentUIForm != null && _StaCurrentUIForm.Count >= 1)
        {
            //清空栈集合
            _StaCurrentUIForm.Clear();
            return true;
        }

        return false;
    }
    #endregion
}
