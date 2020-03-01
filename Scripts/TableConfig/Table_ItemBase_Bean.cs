/*
* 此类由导表工具自动生成 Build By Tanhaiwen
* copy right 2019
*/
using System.Collections.Generic;

using Newtonsoft.Json;

using System.IO;

namespace TableConfigs
{
    public class Table_ItemBase_BeanJsonDatas
    {
         public Table_ItemBase_BeanJsonDatas(string jsonPath)
         {
            var json = File.ReadAllText(jsonPath);
            var datas = JsonConvert.DeserializeObject<Table_ItemBase_Bean>(json);
            TableManager._dicTableDatas.Add("Table_ItemBase_Bean", datas);
            List<int> ids = new List<int>();
            Dictionary<int, object> data = new Dictionary<int, object>();
            foreach (var item in datas.mDataMap)
            {
               ids.Add(item.Value.ID);
               data.Add(item.Value.ID, item.Value);
            }
            TableManager._allIds.Add("Table_ItemBase_Bean",ids);
            TableManager._dicData.Add("Table_ItemBase_Bean",data);
         }
    }
    public class Table_ItemBase_Bean
    {
        public Dictionary<string, Table_ItemBase_DefineData> mDataMap;
    }

    public class Table_ItemBase_DefineData
    {
        //ID
        public int ID;
        //名称
        public string Name;
        //IconID
        public int Iconid;
        //描述
        public string Describe;
        //品质
        public int quanlity;
        //售价
        public string Sale;
    }
}
