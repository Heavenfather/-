using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : BaseUIForms
{
    public Slider SliderLoadingProgress;    //进度条
    private float _ProgressNumber;  //进度数值
    private AsyncOperation _AsyOper;

    public void Awake()
    {
        //设置窗体性质
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.HideOther;
        base.CurrentUIType.UIForm_Type = UIFormType.Norlmal;
        base.CurrentUIType.UIForm_Luceny = UIFormLucencyType.Lucency;

    }


    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        //启动LoadingScenesProgress线程
        StartCoroutine("LoadingScenesProgress");

    }

    //异步加载场景
    IEnumerator LoadingScenesProgress()
    {
        _AsyOper = SceneManager.LoadSceneAsync(ConvertEnumToString.GetInstance().GetStrByEnumScenes(ScenesEnum.MainCity));

        OpenUIForm(SysDefine.SYS_MAINCITY_UIFORM);
        CloseUIForm(SysDefine.SYS_LOADING_UIFORM);
        _ProgressNumber = _AsyOper.progress;
        yield return _AsyOper;
    }

    // Update is called once per frame
    void Update()
    {
        //用于改变进度条的显示，让进度条每一帧都自加
        if (SliderLoadingProgress.value <= 1)
        {
            _ProgressNumber += 0.01f;
        }

        SliderLoadingProgress.value = _ProgressNumber;
    }

}
