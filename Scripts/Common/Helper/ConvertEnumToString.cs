using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertEnumToString
{
    private static ConvertEnumToString _instance;

    //用字典存放场景的枚举类型
    private Dictionary<ScenesEnum, string> _DicScenesEnumLib;
    //为了和表里面的场景id对应
    private Dictionary<int, ScenesEnum> _DicSceneEnumId;

    //构造函数
    private ConvertEnumToString()
    {
        _DicScenesEnumLib = new Dictionary<ScenesEnum, string>();
        _DicScenesEnumLib.Add(ScenesEnum.StartScenes, "00_StartScene");
        _DicScenesEnumLib.Add(ScenesEnum.LoadingScenes, "01_LoadingScene");
        _DicScenesEnumLib.Add(ScenesEnum.MainCity, "02_MainCityScene");
        _DicScenesEnumLib.Add(ScenesEnum.BattleScene01, "03_BattleScene1004");
        _DicScenesEnumLib.Add(ScenesEnum.BattleScene02, "04_BattleScene1005");
        _DicScenesEnumLib.Add(ScenesEnum.BattleScene03, "05_BattleScene1006");

        _DicSceneEnumId = new Dictionary<int, ScenesEnum>();
        _DicSceneEnumId.Add(3, ScenesEnum.BattleScene01);
        _DicSceneEnumId.Add(4, ScenesEnum.BattleScene02);
        _DicSceneEnumId.Add(5, ScenesEnum.BattleScene03);
    }

    public static ConvertEnumToString GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ConvertEnumToString();
        }

        return _instance;
    }

    //得到字符串形式的场景名称
    public string GetStrByEnumScenes(ScenesEnum scenesEnum)
    {
        if (_DicScenesEnumLib != null && _DicScenesEnumLib.Count >= 1)
        {
            return _DicScenesEnumLib[scenesEnum];
        }
        else
        {
            Debug.LogWarning(GetType() + "没有得到场景的枚举类型");
            return null;
        }
    }

    public ScenesEnum GetSceneEnumById(int sceneid)
    {
        if (_DicSceneEnumId != null && _DicSceneEnumId.Count >= 1)
        {
            return _DicSceneEnumId[sceneid];
        }
        else
        {
            Debug.LogWarning("没有得到相应的场景枚举，可能是表里面配的id出错了，代码只配了345这几个id");
            return ScenesEnum.BattleScene01;
        }
    } 

}
