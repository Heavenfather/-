using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    /// <summary>
    /// UnityHelper帮助脚本
    /// 作用：
    ///     1.集成整个项目中通用的方法
    /// </summary>
    public class UnityHelper : MonoBehaviour
    {

        #region 查找工具

        /// <summary>
        /// 通过子物体名称获取子物体的控件
        /// </summary>
        public static T GetComponentByChild<T>(Transform tran, string name) where T : Component
        {
            Transform gObject = tran.Find(name);
            if (gObject == null)
                return null;

            return gObject.GetComponent<T>();
        }
        /// <summary>
        /// 通过子物体名称获取子物体的控件
        /// </summary>
        public static T GetComponentByChild<T>(GameObject obj, string name) where T : Component
        {
            Transform gObject = obj.transform.Find(name);
            if (gObject == null)
                return null;

            return gObject.GetComponent<T>();
        }

        /// <summary>
        /// 查找父节点下的子节点
        /// 内部使用递归算法
        /// </summary>
        /// <param name="goParent">父节点</param>
        /// <param name="child">查找子对象名称</param>
        /// <returns>Transform</returns>
        public static Transform FindTheChildNode(GameObject goParent, string child)
        {
            Transform searchTransform = null;       //查找结果

            searchTransform = goParent.transform.Find(child);
            if (searchTransform == null)
            {
                foreach (Transform tra in goParent.transform)
                {
                    //使用递归一层一层的找
                    searchTransform = FindTheChildNode(tra.gameObject, child);
                    if (searchTransform != null)
                        return searchTransform;
                }
            }

            return searchTransform;
        }

        /// <summary>
        /// 通过组件查找场景中所有的物体，包括隐藏和激活的
        /// </summary>
        public new static List<T> FindObjectsOfType<T>() where T : Component
        {
            List<T> objs = new List<T>();
            List<T> subObjs = new List<T>();
            GameObject[] rootObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject rootObj in rootObjs)
            {
                rootObj.transform.GetComponentsInChildren(true, subObjs);
                objs.AddRange(subObjs);
            }
            return objs;
        }

        /// <summary>
        /// 查找兄弟
        /// </summary>
        public static GameObject FindBrother(GameObject obj, string name)
        {
            GameObject gObject = null;
            if (obj.transform.parent)
            {
                Transform tf = obj.transform.parent.Find(name);
                gObject = tf ? tf.gameObject : null;
            }
            else
            {
                GameObject[] rootObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                foreach (GameObject rootObj in rootObjs)
                {
                    if (rootObj.name == name)
                    {
                        gObject = rootObj;
                        break;
                    }
                }
            }
            return gObject;
        }


        /// <summary>
        /// 获取子节点对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="goParent">父对象</param>
        /// <param name="childName">子对象名称</param>
        /// <returns></returns>
        public static T GetChildNodeComponentScript<T>(GameObject goParent, string childName) where T : Component     //限定这个泛型是一个组件
        {
            Transform searchTransform = null;       //查找结果
            searchTransform = FindTheChildNode(goParent, childName);

            if (searchTransform != null)
            {
                return searchTransform.gameObject.GetComponent<T>();
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 给子节点添加脚本
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="goParen">对象</param>
        /// <param name="childName">子对象名称</param>
        /// <returns></returns>
        public static T AddChildNodeComponent<T>(GameObject goParen, string childName) where T : Component
        {
            Transform searchTransform = null;       //查找子节点的结果
                                                    //查找特定子节点
            searchTransform = FindTheChildNode(goParen, childName);

            //如果查找成功，再考虑这个对象中是否已经存在了相同脚本，有就删除，没有就添加

            if (searchTransform != null)
            {
                T[] componentScriptsArray = searchTransform.GetComponents<T>();

                for (int i = 0; i < componentScriptsArray.Length; i++)
                {
                    if (componentScriptsArray[i] != null)
                        Destroy(componentScriptsArray[i]);
                }

                return searchTransform.gameObject.AddComponent<T>();

            }
            //如果查找不成功，返回一个null
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 给子节点添加父对象
        /// </summary>
        /// <param name="parentNode">父对象方位</param>
        /// <param name="childNode">子对象方位</param>
        public static void AddChildNodeToParentNode(Transform parentNode, Transform childNode)
        {
            childNode.SetParent(parentNode);
            childNode.localPosition = Vector3.zero;
            childNode.localScale = Vector3.one;
            childNode.localEulerAngles = Vector3.zero;
        }

        /// <summary>
        /// 获取RectTransform组件
        /// </summary>
        public static RectTransform rectTransform(Transform tran)
        {
            return tran.GetComponent<RectTransform>();
        }
        /// <summary>
        /// 获取RectTransform组件
        /// </summary>
        public static RectTransform rectTransform(GameObject obj)
        {
            return obj.GetComponent<RectTransform>();
        }
        #endregion

        #region 日志工具
        /// <summary>
        /// 打印普通日志
        /// </summary>
        public static void LogInfo(string value)
        {
            Debug.Log(string.Format("<b><color=cyan>[Info]</color></b> {0}", value));
        }
        /// <summary>
        /// 打印警告日志
        /// </summary>
        public static void LogWarning(string value)
        {
            Debug.LogWarning(string.Format("<b><color=yellow>[Warning]</color></b> {0}", value));
        }
        /// <summary>
        /// 打印错误日志
        /// </summary>
        public static void LogError(string value)
        {
            Debug.LogError(string.Format("<b><color=red>[Error]</color></b> {0}", value));
        }
        #endregion

        #region 反射工具
        /// <summary>
        /// 当前的运行时程序集
        /// </summary>
        private static readonly HashSet<string> RunTimeAssemblies = new HashSet<string>() { "Assembly-CSharp", "UnityEngine" };
        /// <summary>
        /// 从当前程序域的运行时程序集中获取所有类型
        /// </summary>
        public static List<Type> GetTypesInRunTimeAssemblies()
        {
            //存放所有程序域的type
            List<Type> types = new List<Type>();
            //获得程序所有的Assembly
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();         
            for (int i = 0; i < assemblys.Length; i++)
            {
                //取出自己需要用到的Assembly
                if (RunTimeAssemblies.Contains(assemblys[i].GetName().Name))
                {
                    types.AddRange(assemblys[i].GetTypes());
                }
            }
            return types;
        }
        /// <summary>
        /// 从当前程序域的运行时程序集中获取指定类型
        /// </summary>
        public static Type GetTypeInRunTimeAssemblies(string typeName)
        {
            Type type = null;
            foreach (string assembly in RunTimeAssemblies)
            {
                type = Type.GetType(string.Format("{0},{1}", typeName, assembly));
                if (type != null)
                {
                    return type;
                }
            }
            LogError("获取类型 " + typeName + " 失败！当前运行时程序集中不存在此类型！");
            return null;
        }
        #endregion

        #region 其他
        /// <summary>
        /// 把面向目标对象的方法提取出来
        /// </summary>
        /// <param name="self">旋转的自身</param>
        /// <param name="goal">需要面向的目标对象</param>
        /// <param name="rotateSpeed">旋转速度</param>
        public static void FaceToGo(Transform self, Transform goal, float rotateSpeed)
        {
            //通过四元数的计算来让主角朝向敌人
            self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(new Vector3(goal.position.x, 0, goal.position.z) - new Vector3(self.position.x, 0, self.position.z)), rotateSpeed);
        }
        
        /// <summary>
        /// 计算动态生成的Cell中可拖动区域的大小
        /// </summary>
        /// <param name="axis">水平或垂直方向</param>
        /// <param name="cell">生成的Cell</param>
        /// <param name="layoutGo">Cell的父节点</param>
        /// <param name="createNum">创建数量</param>
        /// <param name="rowOrcolumeNum">行的数量或者列的数量（视是水平或者垂直拖动情况而定）</param>
        /// <returns></returns>
        public static void SetScrollArea(RectTransform.Axis axis,GameObject layoutGo,int createNum,int rowOrcolumeNum)
        {
            float result = 0;
            float cellWithOrHeight = 0;
            float spaceXOrY = 0;
            if (axis == RectTransform.Axis.Horizontal)
            {
                cellWithOrHeight=layoutGo.GetComponent<GridLayoutGroup>().cellSize.x;
                spaceXOrY =layoutGo.GetComponent<GridLayoutGroup>().spacing.x;
            }
            else if (axis == RectTransform.Axis.Vertical)
            {
                cellWithOrHeight=layoutGo.GetComponent<GridLayoutGroup>().cellSize.y;
                spaceXOrY=layoutGo.GetComponent<GridLayoutGroup>().spacing.y;
            }

            result = (createNum / rowOrcolumeNum) * (cellWithOrHeight + spaceXOrY);
            if (createNum % rowOrcolumeNum != 0)
            {
                result += cellWithOrHeight + spaceXOrY;
            }

            layoutGo.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(axis, result);
            
        }
        #endregion
        

    }
}
