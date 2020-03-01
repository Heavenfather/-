/*
* 此类由导表工具自动生成 Build By Tanhaiwen
* copy right 2019
*/
using System.Collections.Generic;

using Newtonsoft.Json;

using System.IO;

namespace TableConfigs
{
    public class Table_RoleUpgradeExp_BeanJsonDatas
    {
         public Table_RoleUpgradeExp_BeanJsonDatas(string jsonPath)
         {
            var json = File.ReadAllText(jsonPath);
            var datas = JsonConvert.DeserializeObject<Table_RoleUpgradeExp_Bean>(json);
            TableManager._dicTableDatas.Add("Table_RoleUpgradeExp_Bean", datas);
            List<int> ids = new List<int>();
            Dictionary<int, object> data = new Dictionary<int, object>();
            foreach (var item in datas.mDataMap)
            {
               ids.Add(item.Value.ID);
               data.Add(item.Value.ID, item.Value);
            }
            TableManager._allIds.Add("Table_RoleUpgradeExp_Bean",ids);
            TableManager._dicData.Add("Table_RoleUpgradeExp_Bean",data);
         }
    }
    public class Table_RoleUpgradeExp_Bean
    {
        public Dictionary<string, Table_RoleUpgradeExp_DefineData> mDataMap;
    }

    public class Table_RoleUpgradeExp_DefineData
    {
        //等级
        public int ID;
        //需求经验
        public int NeedExp;
    }
}
