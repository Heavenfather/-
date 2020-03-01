using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using LitJson;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// 本地存储管理类
///  1.文件存储将使用二进制形式存储 节省空间和便于计算机读取，不再需要计算做转换
///  2.文件将区分为两大类
///     （1）一个将存数据的扩展名 .bin文件
///     (2)一个存数据
/// </summary>
public class LocalManager {
    private LocalManager() {

    }

    /// <summary>
    /// 打开存储扩展数据类型
    /// </summary>
    /// <param name="dataName"></param>
    /// <returns></returns>
    public static string DataExt(string dataName) {
        //保存的扩展文件格式为 xxx.bin
        string filePathData = string.Format("/Extension/{0}.bin", dataName);
        string fileExt = "";
        if (File.Exists(Application.streamingAssetsPath + filePathData)) {
            //实例化一个二进制的文件
            BinaryFormatter bf = new BinaryFormatter();
            //打开文件 加载文件数据
            FileStream loadData = File.Open(Application.streamingAssetsPath + filePathData, FileMode.Open);
            //反序列化数据
            fileExt = (string)bf.Deserialize(loadData);
            loadData.Close();
        }
        return fileExt;
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="fileName">数据存储Key</param>
    /// <param name="savedata">数据源</param>
    public static void Save<T>(string dataName, T savedata) {
        //Get数据的类型
        Type dataType = savedata.GetType();
        //文件扩展目录
        var extensionPath = string.Format("{0}/Extension", Application.streamingAssetsPath);
        //数据类型的存放目录
        var saveDataPath = string.Format("{0}/LocalData", Application.streamingAssetsPath);
        //如果没有文件夹就创建出一个文件夹
        if (!Directory.Exists(extensionPath))
            Directory.CreateDirectory(extensionPath);
        if (!Directory.Exists(saveDataPath))
            Directory.CreateDirectory(saveDataPath);
        //实例化扩展目录的二进制文件
        BinaryFormatter extensionFormatter = new BinaryFormatter();
        //保存的二进制文件路径
        string filePathData = "/Extension/" + dataName + ".bin";
        //新建一个文件
        FileStream saveExtensionStreamData = File.Create(Application.streamingAssetsPath + filePathData);
        //序列化扩展名到新建的文件中
        extensionFormatter.Serialize(saveExtensionStreamData, dataType.Name);
        saveExtensionStreamData.Close();

        //创建数据
        BinaryFormatter bf = new BinaryFormatter();
        //文件的保存格式为 数据存储的Key.数据类名称  比如存储账户信息 Account.AccountData 前缀为自定义命名存放的名称 后缀为新建的类即保存下来的数据
        string filePath = string.Format("/LocalData/{0}.{1}", dataName, dataType.Name);
        FileStream saveStream = File.Create(Application.streamingAssetsPath + filePath);
        //保存数据
        bf.Serialize(saveStream, savedata);
        saveStream.Close();
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataName"></param>
    /// <returns></returns>
    public static T Load<T>(string dataName) {
        //给加载的类型初始化个默认值
        T result = default(T);
        //得到扩展文件名
        string fileExt = DataExt(dataName);
        if (string.IsNullOrEmpty(fileExt))
            return result;
        //取存放的二进制数据文件
        string fileDataPath = string.Format("/LocalData/{0}.{1}", dataName, fileExt);
        //判空
        if (File.Exists(Application.streamingAssetsPath + fileDataPath)) {
            //存在数据
            //初始化一个二进制文件
            BinaryFormatter bf = new BinaryFormatter();
            //打开文件
            FileStream fs = File.Open(Application.streamingAssetsPath + fileDataPath, FileMode.Open);
            //数据反序列化 得到数据
            result = (T)bf.Deserialize(fs);
            fs.Close();
        }else{
            Debug.LogError("没有相关的存储数据:" + fileDataPath);
        }
        return result;
    }

    /// <summary>
    /// 检查数据是否存在
    /// </summary>
    /// <param name="dataName"></param>
    /// <returns></returns>
    public static bool Exit(string dataName) {
        //只需要检查扩展名存不存在就行
        string filePathData = string.Format("{0}/Extension/{1}.bin", Application.streamingAssetsPath, dataName);
        if (File.Exists(filePathData))
            return true;
        else
            return false;
    }

    /// <summary>
    /// 删除某个数据
    /// </summary>
    /// <param name="dataName">数据名</param>
    public static void Delete(string dataName) {
        string extensionFilePathData = string.Format("{0}/Extension/{1}.bin", Application.streamingAssetsPath, dataName);
        if (File.Exists(extensionFilePathData)) {
            File.Delete(extensionFilePathData);
            Debug.LogWarning("删除文件:" + extensionFilePathData);
        }
        else{
            Debug.LogError("扩展文件不存在：" + extensionFilePathData);
            return;
        }
        //得到数据扩展名
        string dataExt = DataExt(dataName);
        if (!string.IsNullOrEmpty(dataExt)) {
            string filePath = string.Format("{0}/LocalData/{1}.{2}", Application.streamingAssetsPath, dataName, dataExt);
            if (File.Exists(filePath)) {
                File.Delete(filePath);
                Debug.LogWarning("删除文件:" + filePath);
            }
        }
    }

    /// <summary>
    /// 删除所有数据
    /// </summary>
    public static void DeleteAllData() {
        var path = string.Format("{0}/Extension", Application.streamingAssetsPath);
        if (Directory.Exists(path)) {
            foreach (string file in Directory.GetFiles(path)) {
                File.Delete(file);
                Debug.LogWarning("删除文件：" + file);
            }
        }
        path = string.Format("{0}/LocalData", Application.streamingAssetsPath);
        if (Directory.Exists(path)) {
            foreach (var file in Directory.GetFiles(path)) {
                File.Delete(file);
                Debug.LogWarning("删除文件：" + file);
            }
        }
    }
    
}