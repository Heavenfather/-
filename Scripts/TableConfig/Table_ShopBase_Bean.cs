/*
* 此类由导表工具自动生成 Build By Tanhaiwen
* copy right 2019
*/
using System.Collections.Generic;

using Newtonsoft.Json;

using System.IO;

namespace TableConfigs
{
    public class Table_ShopBase_BeanJsonDatas
    {
         public Table_ShopBase_BeanJsonDatas(string jsonPath)
         {
            var json = File.ReadAllText(jsonPath);
            var datas = JsonConvert.DeserializeObject<Table_ShopBase_Bean>(json);
            TableManager._dicTableDatas.Add("Table_ShopBase_Bean", datas);
            List<int> ids = new List<int>();
            Dictionary<int, object> data = new Dictionary<int, object>();
            foreach (var item in datas.mDataMap)
            {
               ids.Add(item.Value.ID);
               data.Add(item.Value.ID, item.Value);
            }
            TableManager._allIds.Add("Table_ShopBase_Bean",ids);
            TableManager._dicData.Add("Table_ShopBase_Bean",data);
         }
    }
    public class Table_ShopBase_Bean
    {
        public Dictionary<string, Table_ShopBase_DefineData> mDataMap;
    }

    public class Table_ShopBase_DefineData
    {
        //ID
        public int ID;
        //名称
        public string Name;
        //消耗货币类型
        public int CurrencyType;
        //图标ID
        public int IconID;
        //商品的类型
        public int ShopType;
        //价格
        public int Price;
    }
}
