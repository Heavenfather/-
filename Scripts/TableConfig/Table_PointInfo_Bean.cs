﻿/*
* 此类由导表工具自动生成 Build By Tanhaiwen
* copy right 2019
*/
using System.Collections.Generic;

using Newtonsoft.Json;

using System.IO;

namespace TableConfigs
{
    public class Table_PointInfo_BeanJsonDatas
    {
         public Table_PointInfo_BeanJsonDatas(string jsonPath)
         {
            var json = File.ReadAllText(jsonPath);
            var datas = JsonConvert.DeserializeObject<Table_PointInfo_Bean>(json);
            TableManager._dicTableDatas.Add("Table_PointInfo_Bean", datas);
            List<int> ids = new List<int>();
            Dictionary<int, object> data = new Dictionary<int, object>();
            foreach (var item in datas.mDataMap)
            {
               ids.Add(item.Value.ID);
               data.Add(item.Value.ID, item.Value);
            }
            TableManager._allIds.Add("Table_PointInfo_Bean",ids);
            TableManager._dicData.Add("Table_PointInfo_Bean",data);
         }
    }
    public class Table_PointInfo_Bean
    {
        public Dictionary<string, Table_PointInfo_DefineData> mDataMap;
    }

    public class Table_PointInfo_DefineData
    {
        //关卡ID
        public int ID;
        //守卫信息
        public string SoldierInfo;
        //攻击的目标
        public string AttackTarget;
    }
}
