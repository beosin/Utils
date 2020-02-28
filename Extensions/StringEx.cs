using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace OL.Utils.Extensions
{
    /// <summary>
    /// string 扩展
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// 是null还是string.Empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNull(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// null、空还是仅由空白字符串组成
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsEmptyOrZero(this string str)
        {
            return string.IsNullOrWhiteSpace(str) || str == "0";
        }

        public static bool IsNull2(this string value)
        {
            return value == null || value == "" || value == string.Empty || value == " " || value.Length == 0;
        }

        public static bool IsEmpty(this object value)
        {
            if (value == null || value == DBNull.Value) 
                return true;
            return string.IsNullOrWhiteSpace(value.ToString());
        }
        public static bool IsJsonEmpty(this object value)
        {
            if (value == null || value == DBNull.Value)
                return true;
            if (value.ToString().Equals("[]")) return true;
            return string.IsNullOrWhiteSpace(value.ToString());


        }
        /// <summary>
        /// 判断是不是为null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullT<T>(this T t) where T : class
        {
            return t == null;
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            //return source == null || source.Count <= 0;
            return source.Any();
        }

        /// <summary>
        /// 判断List<T>是不是为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullT<T>(this List<T> t) where T : class
        {
            return t == null || t.Count == 0;
        }

        public static bool IsNullLt<T>(this List<T> t)
        {
            return t == null || t.Count == 0;
        }

        public static bool IsNullDataTable(this DataTable table)
        {
            return table == null || table.Rows.Count == 0;
        }

        public static bool IsNullT<T>(this IEnumerable<T> value)
        {
            if (value == null)
                return true;
            return !value.Any();
        }

        public static bool IsNull<T>(this T t) where T : class
        {
            if (t == null)
            {
                return true;
            }
            if (t is string)
            {
                return string.IsNullOrWhiteSpace(t.ToString().Trim());
            }
            if (t is DBNull)
            {
                return true;
            }
            if (t.GetType() == typeof(DataTable))
            {
                Type entityType = typeof(T);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
                DataTable dt = new DataTable();
                foreach (PropertyDescriptor prop in properties)
                {
                    dt.Columns.Add(prop.Name);
                }
                return dt == null || dt.Rows.Count == 0;
            }
            return false;
        }

        public static string[] ToSplit(this object obj, char c = '|')
        {
            return obj.ToString().Split(c);
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToFirstUpper(this string str)
        {
            if (str.IsEmpty())
            {
                throw new ArgumentNullException(nameof(str));
            }
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        /// <summary>
        /// 字符串的长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="flag">默认字符集是UTF8,</param>
        /// <returns></returns>
        public static int LengthH(this string str, EncodingType type = EncodingType.UTF8)
        {
            return str.ToBytes(type).Length;
        }

        /// <summary>
        ///是否包含或全部是中文
        /// </summary>
        /// <param name="str"></param>
        /// <param name="match">true全中文，false含有中文</param>
        /// <returns></returns>
        public static bool IsChinese(this string str, bool match = false)
        {
            var bytes = str.ToBytes(EncodingType.gb2312);
            return match ? bytes.Length == str.Length * 2 : bytes.Length > str.Length;
        }

        public static bool IsMatch(this string value, string pattern, RegexOptions options)
        {
            return value != null && Regex.IsMatch(value, pattern, options);
        }

        public static bool IsMatch(this string value, string pattern)
        {
            return value != null && Regex.IsMatch(value, pattern);
        }



        #region 字符串扩展
        /// <summary>
        /// 忽略大小写的字符串相等比较，判断是否以任意一个待比较字符串相等
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="strs">待比较字符串数组</param>
        /// <returns></returns>
        public static Boolean EqualIgnoreCase(this string value, params String[] strs)
        {
            foreach (var item in strs)
            {
                if (String.Equals(value, item, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>
        /// 忽略大小写的字符串开始比较，判断是否以任意一个待比较字符串开始
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="strs">待比较字符串数组</param>
        /// <returns></returns>
        public static Boolean StartsWithIgnoreCase(this String value, params String[] strs)
        {
            if (value.IsEmpty()) return false;
            foreach (var item in strs)
            {
                if (value.StartsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>
        /// 忽略大小写的字符串结束比较，判断是否以任意一个待比较字符串结束
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="strs">待比较字符串数组</param>
        /// <returns></returns>
        public static Boolean EndsWithIgnoreCase(this String value, params String[] strs)
        {
            if (value == null || String.IsNullOrEmpty(value)) return false;
            foreach (var item in strs)
            {
                if (value.EndsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>
        /// 拆分字符串，过滤空格，无效时返回空数组
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="separators">分组分隔符，默认逗号分号</param>
        /// <returns></returns>
        public static String[] Split(this String  value, params String[] separators)
        {
            //!! netcore3.0中新增Split(String? separator, StringSplitOptions options = StringSplitOptions.None)，优先于StringHelper扩展
            if (value.IsEmpty()) return new String[0];
            if (separators == null || separators.Length < 1 || separators.Length == 1 && separators[0].IsEmpty()) separators = new String[] { ",", ";" };

            return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 拆分字符串成为整型数组，默认逗号分号分隔，无效时返回空数组
        /// </summary>
        /// <remarks>过滤空格、过滤无效、不过滤重复</remarks>
        /// <param name="value">字符串</param>
        /// <param name="separators">分组分隔符，默认逗号分号</param>
        /// <returns></returns>
        public static Int32[] SplitAsInt(this String value, params String[] separators)
        {
            if (value == null || String.IsNullOrEmpty(value)) return new Int32[0];
            if (separators == null || separators.Length < 1) separators = new String[] { ",", ";" };

            var ss = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<Int32>();
            foreach (var item in ss)
            {
                if (!Int32.TryParse(item.Trim(), out var id)) continue;

                // 本意只是拆分字符串然后转为数字，不应该过滤重复项
                //if (!list.Contains(id))
                list.Add(id);
            }

            return list.ToArray();
        }

       

       


        /// <summary>追加分隔符字符串，忽略开头，常用于拼接</summary>
        /// <param name="sb">字符串构造者</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static StringBuilder Separate(this StringBuilder sb, String separator)
        {
            if (separator.IsEmpty()) return sb;
            if (sb.Length > 0) sb.Append(separator);
            return sb;
        }

        /// <summary>字符串转数组</summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码，默认utf-8无BOM</param>
        /// <returns></returns>
        public static Byte[] GetBytes(this String value, Encoding encoding = null)
        {
            //if (value == null) return null;
            if (value.IsEmpty()) return new Byte[0];
            if (encoding == null) encoding = Encoding.UTF8;
            return encoding.GetBytes(value);
        }


        #endregion
        #region 截取扩展
        /// <summary>
        /// 确保字符串以指定的另一字符串开始，不区分大小写
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static String EnsureStart(this String value, String start)
        {
            if (start.IsEmpty()) return value;
            if (value.IsEmpty()) return start;

            if (value.StartsWith(start, StringComparison.OrdinalIgnoreCase)) return value;
            return start + value;
        }

        /// <summary>
        /// 确保字符串以指定的另一字符串结束，不区分大小写
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static String EnsureEnd(this String value, String end)
        {
            if (end.IsEmpty()) return value;
            if (value.IsEmpty()) return end;
            if (value.EndsWith(end, StringComparison.OrdinalIgnoreCase)) return value;
            return value + end;
        }

        /// <summary>
        /// 从当前字符串开头移除另一字符串，不区分大小写，循环多次匹配前缀
        /// </summary>
        /// <param name="value">当前字符串</param>
        /// <param name="starts">另一字符串</param>
        /// <returns></returns>
        public static String TrimStart(this String value, params String[] starts)
        {
            if (value.IsEmpty()) return value;
            if (starts == null || starts.Length < 1 || starts[0].IsEmpty()) return value;
            for (var i = 0; i < starts.Length; i++)
            {
                if (value.StartsWith(starts[i], StringComparison.OrdinalIgnoreCase))
                {
                    value = value.Substring(starts[i].Length);
                    if (value.IsEmpty()) break;
                    // 从头开始
                    i = -1;
                }
            }
            return value;
        }

        /// <summary>
        /// 从当前字符串结尾移除另一字符串，不区分大小写，循环多次匹配后缀
        /// </summary>
        /// <param name="value">当前字符串</param>
        /// <param name="ends">另一字符串</param>
        /// <returns></returns>
        public static String TrimEnd(this String value, params String[] ends)
        {
            if (value.IsEmpty()) return value;
            if (ends == null || ends.Length < 1 || ends[0].IsEmpty()) return value;
            for (var i = 0; i < ends.Length; i++)
            {
                if (value.EndsWith(ends[i], StringComparison.OrdinalIgnoreCase))
                {
                    value = value.Substring(0, value.Length - ends[i].Length);
                    if (value.IsEmpty()) break;
                    // 从头开始
                    i = -1;
                }
            }
            return value;
        }

        /// <summary>
        /// 从字符串中检索子字符串，在指定头部字符串之后，指定尾部字符串之前
        /// </summary>
        /// <remarks>常用于截取xml某一个元素等操作</remarks>
        /// <param name="value">目标字符串</param>
        /// <param name="after">头部字符串，在它之后</param>
        /// <param name="before">尾部字符串，在它之前</param>
        /// <param name="startIndex">搜索的开始位置</param>
        /// <param name="positions">位置数组，两个元素分别记录头尾位置</param>
        /// <returns></returns>
        public static String  Substring(this String value, String after, String  before = null, Int32 startIndex = 0, Int32[]  positions = null)
        {
            if (value.IsEmpty()) return value;
            if (after.IsEmpty()  && before.IsEmpty()) return value;

            /*
             * 1，只有start，从该字符串之后部分
             * 2，只有end，从开头到该字符串之前
             * 3，同时start和end，取中间部分
             */

            var p = -1;
            if (!after.IsEmpty())
            {
                p = value.IndexOf(after, startIndex);
                if (p < 0) return null;
                p += after.Length;
                // 记录位置
                if (positions != null && positions.Length > 0) positions[0] = p;
            }

            if (before.IsEmpty()) return value.Substring(p);

            var f = value.IndexOf(before, p >= 0 ? p : startIndex);
            if (f < 0) return null;

            // 记录位置
            if (positions != null && positions.Length > 1) positions[1] = f;

            if (p >= 0)
                return value.Substring(p, f - p);
            else
                return value.Substring(0, f);
        }

        /// <summary>
        /// 根据最大长度截取字符串，并允许以指定空白填充末尾
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="maxLength">截取后字符串的最大允许长度，包含后面填充</param>
        /// <param name="pad">需要填充在后面的字符串，比如几个圆点</param>
        /// <returns></returns>
        public static String Cut(this String value, Int32 maxLength, String  pad = "")
        {
            if (value.IsEmpty() || maxLength <= 0 || value.Length < maxLength) return value;

            // 计算截取长度
            var len = maxLength;
            if (!pad.IsEmpty()) len -= pad.Length;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            return value.Substring(0, len) + pad;
        }

        /// <summary>
        /// 从当前字符串开头移除另一字符串以及之前的部分
        /// </summary>
        /// <param name="value">当前字符串</param>
        /// <param name="starts">另一字符串</param>
        /// <returns></returns>
        public static String CutStart(this String value, params String[] starts)
        {
            if (value.IsEmpty()) return value;
            if (starts == null || starts.Length < 1 || starts[0].IsEmpty()) return value;

            for (var i = 0; i < starts.Length; i++)
            {
                var p = value.IndexOf(starts[i]);
                if (p >= 0)
                {
                    value = value.Substring(p + starts[i].Length);
                    if (value.IsEmpty()) break;
                }
            }
            return value;
        }

        /// <summary>
        /// 从当前字符串结尾移除另一字符串以及之后的部分
        /// </summary>
        /// <param name="value">当前字符串</param>
        /// <param name="ends">另一字符串</param>
        /// <returns></returns>
        public static String CutEnd(this String value, params String[] ends)
        {
            if (value.IsEmpty()) return value;
            if (ends == null || ends.Length < 1 || ends[0].IsEmpty()) return value;
            for (var i = 0; i < ends.Length; i++)
            {
                var p = value.LastIndexOf(ends[i]);
                if (p >= 0)
                {
                    value = value.Substring(0, p);
                    if (value.IsEmpty()) break;
                }
            }
            return value;
        }
        #endregion

        #region CreateGuid
        /// <summary>
        /// 生成ID
        /// </summary>
        /// <param name="isNoUnderline">isNoUnderline=true 生成没有下划线的guiid</param>
        /// <returns></returns>
        public static string CreateGuid(bool isNoUnderline = true)
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        } 
        #endregion


    }
}
