using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TableConfigs;

public class StartProject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TableManager.GetInstance().InitTableData();
        //启动游戏，加载游戏UI
        UIManager.GetInstance().ShowUIForm(SysDefine.SYS_LOGIN_UIFORM);

    }
}
