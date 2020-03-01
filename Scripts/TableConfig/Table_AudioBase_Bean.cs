/*
* 此类由导表工具自动生成 Build By Tanhaiwen
* copy right 2019
*/
using System.Collections.Generic;

using Newtonsoft.Json;

using System.IO;

namespace TableConfigs
{
    public class Table_AudioBase_BeanJsonDatas
    {
         public Table_AudioBase_BeanJsonDatas(string jsonPath)
         {
            var json = File.ReadAllText(jsonPath);
            var datas = JsonConvert.DeserializeObject<Table_AudioBase_Bean>(json);
            TableManager._dicTableDatas.Add("Table_AudioBase_Bean", datas);
            List<int> ids = new List<int>();
            Dictionary<int, object> data = new Dictionary<int, object>();
            foreach (var item in datas.mDataMap)
            {
               ids.Add(item.Value.ID);
               data.Add(item.Value.ID, item.Value);
            }
            TableManager._allIds.Add("Table_AudioBase_Bean",ids);
            TableManager._dicData.Add("Table_AudioBase_Bean",data);
         }
    }
    public class Table_AudioBase_Bean
    {
        public Dictionary<string, Table_AudioBase_DefineData> mDataMap;
    }

    public class Table_AudioBase_DefineData
    {
        //音频ID
        public int ID;
        //音频名称
        public string Name;
        //是否循环
        public int loop;
        //音频类型
        public int type;
        //备注
        public string none;
    }
}
