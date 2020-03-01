using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机移动 支持鼠标右键左右观察 滚轮向前或向后 键盘WASD上下左右移动
/// </summary>
public class CameraMove : MonoBehaviour
{
    [Header("是否启用鼠标右键进行视角转动")]
    public bool IsOpenMouseRight=false;
    public float sensitivityMouse = 2f;
    public float sensitivetyKeyBoard = 0.1f;
    public float sensitivetyMouseWheel = 10f;
    void Update()
    {
        //滚轮实现镜头缩进和拉远        
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            this.GetComponent<Camera>().fieldOfView = this.GetComponent<Camera>().fieldOfView - Input.GetAxis("Mouse ScrollWheel") * sensitivetyMouseWheel;
        }
        if (IsOpenMouseRight)
        {
            //按着鼠标右键实现视角转动        
            if (Input.GetMouseButton(1))
            {
                transform.Rotate(-Input.GetAxis("Mouse Y") * sensitivityMouse, Input.GetAxis("Mouse X") * sensitivityMouse, 0);
            }
        }
        //键盘按钮←/a和→/d实现视角水平移动，键盘按钮↑/w和↓/s实现视角水平旋转        
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxis("Horizontal") * sensitivetyKeyBoard, 0, 0);
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(0, Input.GetAxis("Vertical") * sensitivetyKeyBoard, 0);
        }
    }
}
