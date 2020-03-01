using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SoldierHPCanvas : BaseUIForms
{
    private void Start()
    {
    }
    private void Update()
    {
        //使canvas始终都是面向主摄像机的
        transform.rotation = Camera.main.transform.rotation;
    }

}
