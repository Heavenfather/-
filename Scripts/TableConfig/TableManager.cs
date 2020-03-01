using System.Collections.Generic;
using UnityEngine;
using System;

namespace TableConfigs
{
    public class TableManager
    {

       public static Dictionary<string,object> _dicTableDatas = new Dictionary<string, object>();
       public static Dictionary<string, List<int>> _allIds = new Dictionary<string, List<int>>();
       public static Dictionary<string, Dictionary<int, object>> _dicData = new Dictionary<string, Dictionary<int, object>>();

       private Table_AttributeBase_BeanJsonDatas table_AttributeBase_Bean;
       private Table_AudioBase_BeanJsonDatas table_AudioBase_Bean;
       private Table_BattleUseCountComparision_BeanJsonDatas table_BattleUseCountComparision_Bean;
       private Table_CelueCardBase_BeanJsonDatas table_CelueCardBase_Bean;
       private Table_CheckPoints_BeanJsonDatas table_CheckPoints_Bean;
       private Table_ItemBase_BeanJsonDatas table_ItemBase_Bean;
       private Table_PointInfo_BeanJsonDatas table_PointInfo_Bean;
       private Table_RoleUpgradeExp_BeanJsonDatas table_RoleUpgradeExp_Bean;
       private Table_ShopBase_BeanJsonDatas table_ShopBase_Bean;
       private Table_WujiangCarBase_BeanJsonDatas table_WujiangCarBase_Bean;
       private Table_WujiangUpgrade_BeanJsonDatas table_WujiangUpgrade_Bean;
       private static TableManager _instance;
       public static TableManager GetInstance()
       {
          if (_instance == null)
          {
             _instance = new TableManager();
          }
          return _instance;
       }
       private TableManager(){}

       public void InitTableData()
       {
         table_AttributeBase_Bean = new Table_AttributeBase_BeanJsonDatas(GetTablePathByName("AttributeBase"));
         table_AudioBase_Bean = new Table_AudioBase_BeanJsonDatas(GetTablePathByName("AudioBase"));
         table_BattleUseCountComparision_Bean = new Table_BattleUseCountComparision_BeanJsonDatas(GetTablePathByName("BattleUseCountComparision"));
         table_CelueCardBase_Bean = new Table_CelueCardBase_BeanJsonDatas(GetTablePathByName("CelueCardBase"));
         table_CheckPoints_Bean = new Table_CheckPoints_BeanJsonDatas(GetTablePathByName("CheckPoints"));
         table_ItemBase_Bean = new Table_ItemBase_BeanJsonDatas(GetTablePathByName("ItemBase"));
         table_PointInfo_Bean = new Table_PointInfo_BeanJsonDatas(GetTablePathByName("PointInfo"));
         table_RoleUpgradeExp_Bean = new Table_RoleUpgradeExp_BeanJsonDatas(GetTablePathByName("RoleUpgradeExp"));
         table_ShopBase_Bean = new Table_ShopBase_BeanJsonDatas(GetTablePathByName("ShopBase"));
         table_WujiangCarBase_Bean = new Table_WujiangCarBase_BeanJsonDatas(GetTablePathByName("WujiangCarBase"));
         table_WujiangUpgrade_Bean = new Table_WujiangUpgrade_BeanJsonDatas(GetTablePathByName("WujiangUpgrade"));
       }

       public string GetTablePathByName(string name)
       {
          string path = Application.streamingAssetsPath+"/Table_"+name+"_Bean.json";
          return path;
       }

       /// <summary>
       /// 根据表名返回全部的ID
       /// </summary>
       ///<param name="tableName">表名</param>
       public List<int> GetAllIds(string tableName)
       {
          string realName = "Table_"+tableName+"_Bean";
          List<int> ids;
          _allIds.TryGetValue(realName, out ids);
          return ids;
       }

       /// <summary>
       /// 指定表名和id返回数据结果
       /// </summary>
       ///<param name="tableName">表名</param>
       ///<param name="id">需查询的ID</param>
       public object GetDataById(string tableName,int id)
       {
          string realName = "Table_"+tableName+"_Bean";
          Dictionary<int, object> tableData;
          _dicData.TryGetValue(realName, out tableData);
          object data;
          tableData.TryGetValue(id, out data);
          if (data != null)
          {
             return data;
          }
          Debug.LogError(tableName+"表里没有 id="+id);
          return null;
       }
    }
}
