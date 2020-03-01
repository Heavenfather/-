/*
* 此类由导表工具自动生成 Build By Tanhaiwen
* copy right 2019
*/
using System.Collections.Generic;

using Newtonsoft.Json;

using System.IO;

namespace TableConfigs
{
    public class Table_CelueCardBase_BeanJsonDatas
    {
         public Table_CelueCardBase_BeanJsonDatas(string jsonPath)
         {
            var json = File.ReadAllText(jsonPath);
            var datas = JsonConvert.DeserializeObject<Table_CelueCardBase_Bean>(json);
            TableManager._dicTableDatas.Add("Table_CelueCardBase_Bean", datas);
            List<int> ids = new List<int>();
            Dictionary<int, object> data = new Dictionary<int, object>();
            foreach (var item in datas.mDataMap)
            {
               ids.Add(item.Value.ID);
               data.Add(item.Value.ID, item.Value);
            }
            TableManager._allIds.Add("Table_CelueCardBase_Bean",ids);
            TableManager._dicData.Add("Table_CelueCardBase_Bean",data);
         }
    }
    public class Table_CelueCardBase_Bean
    {
        public Dictionary<string, Table_CelueCardBase_DefineData> mDataMap;
    }

    public class Table_CelueCardBase_DefineData
    {
        //ID
        public int ID;
        //名称
        public string Name;
        //属性
        public string Attribute;
        //描述
        public string Description;
    }
}
