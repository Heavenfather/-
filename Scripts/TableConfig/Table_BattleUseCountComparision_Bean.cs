﻿/*
* 此类由导表工具自动生成 Build By Tanhaiwen
* copy right 2019
*/
using System.Collections.Generic;

using Newtonsoft.Json;

using System.IO;

namespace TableConfigs
{
    public class Table_BattleUseCountComparision_BeanJsonDatas
    {
         public Table_BattleUseCountComparision_BeanJsonDatas(string jsonPath)
         {
            var json = File.ReadAllText(jsonPath);
            var datas = JsonConvert.DeserializeObject<Table_BattleUseCountComparision_Bean>(json);
            TableManager._dicTableDatas.Add("Table_BattleUseCountComparision_Bean", datas);
            List<int> ids = new List<int>();
            Dictionary<int, object> data = new Dictionary<int, object>();
            foreach (var item in datas.mDataMap)
            {
               ids.Add(item.Value.ID);
               data.Add(item.Value.ID, item.Value);
            }
            TableManager._allIds.Add("Table_BattleUseCountComparision_Bean",ids);
            TableManager._dicData.Add("Table_BattleUseCountComparision_Bean",data);
         }
    }
    public class Table_BattleUseCountComparision_Bean
    {
        public Dictionary<string, Table_BattleUseCountComparision_DefineData> mDataMap;
    }

    public class Table_BattleUseCountComparision_DefineData
    {
        //等级
        public int ID;
        //最多生成士兵数量
        public int MaxCreateSoldierNum;
        //使用卡牌次数
        public int MaxUseCardCount;
    }
}
