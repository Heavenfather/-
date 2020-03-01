/*
* 此类由导表工具自动生成 Build By Tanhaiwen
* copy right 2019
*/
using System.Collections.Generic;

using Newtonsoft.Json;

using System.IO;

namespace TableConfigs
{
    public class Table_CheckPoints_BeanJsonDatas
    {
         public Table_CheckPoints_BeanJsonDatas(string jsonPath)
         {
            var json = File.ReadAllText(jsonPath);
            var datas = JsonConvert.DeserializeObject<Table_CheckPoints_Bean>(json);
            TableManager._dicTableDatas.Add("Table_CheckPoints_Bean", datas);
            List<int> ids = new List<int>();
            Dictionary<int, object> data = new Dictionary<int, object>();
            foreach (var item in datas.mDataMap)
            {
               ids.Add(item.Value.ID);
               data.Add(item.Value.ID, item.Value);
            }
            TableManager._allIds.Add("Table_CheckPoints_Bean",ids);
            TableManager._dicData.Add("Table_CheckPoints_Bean",data);
         }
    }
    public class Table_CheckPoints_Bean
    {
        public Dictionary<string, Table_CheckPoints_DefineData> mDataMap;
    }

    public class Table_CheckPoints_DefineData
    {
        //关卡ID
        public int ID;
        //关卡人物ID
        public int RoleID;
        //名称
        public string Title;
        //描述
        public string Description;
        //难度
        public int Difficulty;
        //奖励
        public string itemsID;
        //挑战所需等级
        public int NeedLevel;
        //跳转的战斗场景ID
        public int LevelID;
    }
}
