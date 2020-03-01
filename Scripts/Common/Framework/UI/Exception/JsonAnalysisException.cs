using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// JSON解析异常
/// 功能：专门负责对JSON由于路径错误，或者JSON格式错误信息进行捕获并且抛出异常信息
/// </summary>
public class JsonAnalysisException : Exception {
    public JsonAnalysisException() : base()
    {

    }

    public JsonAnalysisException(string exceptionMessage) : base(exceptionMessage)
    {

    }
}
