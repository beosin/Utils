using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
namespace OL.Utils.Extensions
{
    /// <summary>枚举类型助手k=扩展类</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EnumEx
    {
        public static string GetDescription(this Enum obj)
        {
            var fi = obj.GetType().GetField(obj.ToString());
            var arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arrDesc[0]?.Description;
        }

        //public static string GetDescription<T>(this Enum obj)
        //{
        //    var refactor = typeof(T).GetField(obj.ToString()).GetReflector();
        //    var arrDesc = refactor.GetCustomAttributes(typeof(DescriptionAttribute));
        //    return ((DescriptionAttribute)arrDesc[0]).Description;
        //}

        public static T ToEnum<T>(this string obj)
        {
            return (T)Enum.Parse(typeof(T), obj);
        }

        //public static string GetDescription<T>(string obj)
        //{
        //    var refactor = typeof(T).GetField(obj).GetReflector();
        //    var arrDesc = refactor.GetCustomAttributes(typeof(DescriptionAttribute));
        //    return ((DescriptionAttribute)arrDesc[0]).Description;
        //}

        public static Dictionary<int, string> ToDict(this Enum obj)
        {
            var enumDict = new Dictionary<int, string>();
            foreach (int enumValue in Enum.GetValues(obj.GetType()))
            {
                enumDict.Add(enumValue, enumValue.ToString());
            }
            return enumDict;
        }

        public static List<string> ToList<T>()
        {
            //Type t = typeof(T);
            //var ty = t.GetFields(BindingFlags.Public | BindingFlags.Static)
            //.Where(p => t.IsAssignableFrom(p.FieldType))
            //.Select(pi => (T)pi.GetValue(null))
            //.ToList();
            return Enum.GetValues(typeof(T)).Cast<T>().Select(x => x.ToString()).ToList();
        }

        public static List<KeyValuePair<int, string>> ToKVList<T>()
        {
            var keys = new List<KeyValuePair<int, string>>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                DescriptionAttribute da = objArr?[0] as DescriptionAttribute;
                keys.Add(new KeyValuePair<int, string>(Convert.ToInt32(e), da.Description));
            }
            return keys;
        }

        public static List<KeyValuePair<int, string>> ToKVListLinq<T>()
        {
            Type t = typeof(T);
            return t.GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(p => t.IsAssignableFrom(p.FieldType))
        .Select(a => new KeyValuePair<int, string>(Convert.ToInt32(a.GetValue(null)),
        (a.GetCustomAttributes(typeof(DescriptionAttribute), true)?[0] as DescriptionAttribute).Description)
        ).ToList();
        }

        /// <summary>枚举变量是否包含指定标识</summary>
        /// <param name="value">枚举变量</param>
        /// <param name="flag">要判断的标识</param>
        /// <returns></returns>
        public static Boolean Has(this Enum value, Enum flag)
        {

            if (value.GetType() != flag.GetType()) throw new ArgumentException("flag", "枚举标识判断必须是相同的类型！");

            var num = Convert.ToUInt64(flag);
            return (Convert.ToUInt64(value) & num) == num;
        }

        /// <summary>设置标识位</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="flag"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public static T Set<T>(this Enum source, T flag, Boolean value)
        {
            if (!(source is T)) throw new ArgumentException("source", "枚举标识判断必须是相同的类型！");

            var s = Convert.ToUInt64(source);
            var f = Convert.ToUInt64(flag);

            if (value)
            {
                s |= f;
            }
            else
            {
                s &= ~f;
            }

            return (T)Enum.ToObject(typeof(T), s);
        }

        ///// <summary>获取枚举字段的注释</summary>
        ///// <param name="value">数值</param>
        ///// <returns></returns>
        //public static String GetDescription(this Enum value)
        //{
        //    if (value == null) return null;

        //    var type = value.GetType();
        //    var item = type.GetField(value.ToString(), BindingFlags.Public | BindingFlags.Static);
        //    if (item == null) return null;
        //    //var att = AttributeX.GetCustomAttribute<DescriptionAttribute>(item, false);
        //    var att = item.GetCustomAttribute<DescriptionAttribute>(false);
        //    if (att != null && !String.IsNullOrEmpty(att.Description)) return att.Description;

        //    return null;
        //}

        /// <summary>获取枚举类型的所有字段注释</summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        //public static Dictionary<TEnum, String> GetDescriptions<TEnum>()
        //{
        //    var dic = new Dictionary<TEnum, String>();

        //    foreach (var item in GetDescriptions(typeof(TEnum)))
        //    {
        //        dic.Add((TEnum)(Object)item.Key, item.Value);
        //    }

        //    return dic;
        //}

        ///// <summary>获取枚举类型的所有字段注释</summary>
        ///// <param name="enumType"></param>
        ///// <returns></returns>
        //public static Dictionary<Int32, String> GetDescriptions(Type enumType)
        //{
        //    var dic = new Dictionary<Int32, String>();
        //    foreach (var item in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
        //    {
        //        if (!item.IsStatic) continue;

        //        // 这里的快速访问方法会报错
        //        //FieldInfoX fix = FieldInfoX.Create(item);
        //        //PermissionFlags value = (PermissionFlags)fix.GetValue(null);
        //        var value = Convert.ToInt32(item.GetValue(null));

        //        var des = item.Name;

        //        //var dna = AttributeX.GetCustomAttribute<DisplayNameAttribute>(item, false);
        //        var dna = item.GetCustomAttribute<DisplayNameAttribute>(false);
        //        if (dna != null && !String.IsNullOrEmpty(dna.DisplayName)) des = dna.DisplayName;

        //        //var att = AttributeX.GetCustomAttribute<DescriptionAttribute>(item, false);
        //        var att = item.GetCustomAttribute<DescriptionAttribute>(false);
        //        if (att != null && !String.IsNullOrEmpty(att.Description)) des = att.Description;
        //        //dic.Add(value, des);
        //        // 有些枚举可能不同名称有相同的值
        //        dic[value] = des;
        //    }

        //    return dic;
        //}


        
       


    }
}
