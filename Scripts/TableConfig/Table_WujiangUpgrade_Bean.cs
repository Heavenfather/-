/*
* 此类由导表工具自动生成 Build By Tanhaiwen
* copy right 2019
*/
using System.Collections.Generic;

using Newtonsoft.Json;

using System.IO;

namespace TableConfigs
{
    public class Table_WujiangUpgrade_BeanJsonDatas
    {
         public Table_WujiangUpgrade_BeanJsonDatas(string jsonPath)
         {
            var json = File.ReadAllText(jsonPath);
            var datas = JsonConvert.DeserializeObject<Table_WujiangUpgrade_Bean>(json);
            TableManager._dicTableDatas.Add("Table_WujiangUpgrade_Bean", datas);
            List<int> ids = new List<int>();
            Dictionary<int, object> data = new Dictionary<int, object>();
            foreach (var item in datas.mDataMap)
            {
               ids.Add(item.Value.ID);
               data.Add(item.Value.ID, item.Value);
            }
            TableManager._allIds.Add("Table_WujiangUpgrade_Bean",ids);
            TableManager._dicData.Add("Table_WujiangUpgrade_Bean",data);
         }
    }
    public class Table_WujiangUpgrade_Bean
    {
        public Dictionary<string, Table_WujiangUpgrade_DefineData> mDataMap;
    }

    public class Table_WujiangUpgrade_DefineData
    {
        //武将ID
        public int ID;
        //消耗材料
        public string Stuff;
        //增加属性ID
        public int UpgradeAttID;
        //升级属性系数
        public int UpgradeCoe;
        //增加基本值
        public int BasicNum;
    }
}
