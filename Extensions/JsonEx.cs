using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OL.Utils.Extensions
{
   public static  class JsonEx
    {
        /// <summary>
        /// 从json字符串取某个值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="filedName"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetJsonValue( this string json ,string filedName,string defaultValue="")
        {
            if (json.IsJsonEmpty()) return defaultValue;
            try
            {
                JObject data = JObject.Parse(json);
                var result = data[filedName];
                if (result == null) return defaultValue;
                return data[filedName].ConvertToString();
            }
            catch 
            {
                return defaultValue;
            }
        }
        public static DataTable ConvertToDataTable(this string json)
        {            
            return json.IsJsonEmpty() ? null : JsonConvert.DeserializeObject<DataTable>(json);
        }
        public static object ConvertToJson(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject(json);
        }

        public static string ConvertToJson(this object value)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
           JsonConvert.SerializeObject(value, Formatting.Indented, jsetting);
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
           
            var result = JsonConvert.SerializeObject(value, Formatting.Indented, jsetting);
            //var result= JsonConvert.SerializeObject(value, timeConverter);
            return result.Equals("null")?"":result;
        }

        public static string ConvertToJson(this object value, string dateTimeFormat)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = dateTimeFormat.IsEmpty() ? "yyyy-MM-dd HH:mm:ss" : dateTimeFormat };
            return JsonConvert.SerializeObject(value, timeConverter);
        }

        public static T ConvertToObject<T>(this string json)
        {
            //return json == null ? default : JsonConvert.DeserializeObject<T>(json);
            return json == null ? default : JsonConvert.DeserializeObject<T>(json);
        }

        public static List<T> ConvertToToList<T>(this string json)
        {
            
            return json == null ? null : JsonConvert.DeserializeObject<List<T>>(json);
        }
        public static JObject ConvertToJObject(this string json)
        {
            return json == null ? JObject.Parse("{}") : JObject.Parse(json.Replace("&nbsp;", ""));
        }

        /// <summary> 
        /// Json格式转换成键值对，键值对中的Key需要区分大小写 ,不考虑json本身就是一个数组的形式
        /// </summary> 
        /// <param name="JsonData">需要转换的Json文本数据</param> 
        /// <returns></returns> 
        public static Dictionary<string, object> ConvertToDictionary(this  string jsonData)
        {
            object Data = null;
            Dictionary<string, object> Dic = new Dictionary<string, object>();
            MatchCollection Match = Regex.Matches(jsonData, @"""(.+?)"": {0,1}(\[[\s\S]+?\]|null|"".+?""|(\-|\+)?\d+(\.\d+)?)");//使用正则表达式匹配出JSON数据中的键与值 
            foreach (Match item in Match)
            {
                try
                {
                    if (item.Groups[2].ToString().ToLower() == "null") Data = null;//如果数据为null(字符串类型),直接转换成null 
                    else Data = item.Groups[2].ToString(); //数据为数字、字符串中的一类，直接写入 
                    Dic.Add(item.Groups[1].ToString(), Data);
                }
                catch { }
            }
            return Dic;
        }
    }
}
