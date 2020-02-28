using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OL.Utils.Extensions
{
   public static  class MathEx
    {
        private static DateTime _dt1970 = new DateTime(1970, 1, 1);
        #region ConvertToInt
   
        /// <summary>
        /// 转为整数，转换失败时返回默认值。支持字符串、全角、字节数组（小端）、时间（Unix秒）
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static Int32 ConvertToInt(this object value, Int32 defaultValue = 0)
        {
            if (value.IsEmpty()) return defaultValue ;
            // 特殊处理字符串，也是最常见的
            if (value is String str)
            {
                // 拷贝而来的逗号分隔整数
                str = str.Replace(",", null);
                str = ToDBC(str).Trim();
                if (str.IsEmpty()) return defaultValue;
                if (Int32.TryParse(str, out var n)) return n;
                return defaultValue;
            }
            // 特殊处理时间，转Unix秒
            if (value is DateTime dt)
            {
                if (dt == DateTime.MinValue) return 0;

                //// 先转UTC时间再相减，以得到绝对时间差
                return (Int32)(dt - _dt1970).TotalSeconds;
            }
            if (value is Byte[] buf)
            {
                if (buf == null || buf.Length < 1) return defaultValue;

                switch (buf.Length)
                {
                    case 1:
                        return buf[0];
                    case 2:
                        return BitConverter.ToInt16(buf, 0);
                    case 3:
                        return BitConverter.ToInt32(new Byte[] { buf[0], buf[1], buf[2], 0 }, 0);
                    case 4:
                        return BitConverter.ToInt32(buf, 0);
                    default:
                        break;
                }
            }
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return defaultValue; }
        }
        #endregion
        #region ConvertToInt64
        /// <summary>转为长整数。支持字符串、全角、字节数组（小端）、时间（Unix毫秒）
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static Int64 ConvertToInt64(this object value, Int64 defaultValue = 0)
        {
            if (value.IsEmpty()) return defaultValue;

            // 特殊处理字符串，也是最常见的
            if (value is String str)
            {
                // 拷贝而来的逗号分隔整数
                str = str.Replace(",", null);
                str = ToDBC(str).Trim();
                if (str.IsEmpty()) return defaultValue;
                if (Int64.TryParse(str, out var n)) return n;
                return defaultValue;
            }

            // 特殊处理时间，转Unix毫秒
            if (value is DateTime dt)
            {
                if (dt == DateTime.MinValue) return 0;
                //// 先转UTC时间再相减，以得到绝对时间差
                return (Int64)(dt - _dt1970).TotalMilliseconds;
            }

            if (value is Byte[] buf)
            {
                if (buf == null || buf.Length < 1) return defaultValue;

                switch (buf.Length)
                {
                    case 1:
                        return buf[0];
                    case 2:
                        return BitConverter.ToInt16(buf, 0);
                    case 3:
                        return BitConverter.ToInt32(new Byte[] { buf[0], buf[1], buf[2], 0 }, 0);
                    case 4:
                        return BitConverter.ToInt32(buf, 0);
                    case 8:
                        return BitConverter.ToInt64(buf, 0);
                    default:
                        break;
                }
            }

            //暂时不做处理  先处理异常转换
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return defaultValue; }
        }
        #endregion
        #region ConvertToDouble
        /// <summary>
        /// 转为双精度浮点数
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static double ConvertToDouble(this object value, double defaultValue = 0.0)
        {
            if (value.IsEmpty()) return defaultValue;
            if (value is String str)
            {
                str = ToDBC(str).Trim();
                if (str.IsEmpty()) return defaultValue;
                if (Double.TryParse(str, out var n)) return n;
                return defaultValue;
            }
            else if (value is Byte[] buf)
            {
                if (buf == null || buf.Length < 1) return defaultValue;
                switch (buf.Length)
                {
                    case 1:
                        return buf[0];
                    case 2:
                        return BitConverter.ToInt16(buf, 0);
                    case 3:
                        return BitConverter.ToInt32(new Byte[] { buf[0], buf[1], buf[2], 0 }, 0);
                    case 4:
                        return BitConverter.ToInt32(buf, 0);
                    default:
                        // 凑够8字节
                        if (buf.Length < 8)
                        {
                            var bts = new Byte[8];
                            Buffer.BlockCopy(buf, 0, bts, 0, buf.Length);
                            buf = bts;
                        }
                        return BitConverter.ToDouble(buf, 0);
                }
            }

            try
            {
                return Convert.ToDouble(value);
            }
            catch { return defaultValue; }
        }
        #endregion
        #region ConvertToFloat
        /// <summary>转为4字节单精度浮点数</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static float  ConvertToFloat(this object value, float  defaultValue = 0.0f)
        {
            if (value.IsEmpty()) return defaultValue;
            if (value is String str)
            {
                str = ToDBC(str).Trim();
                if (str.IsEmpty()) return defaultValue;
                if (float .TryParse(str, out var n)) return n;
                return defaultValue;
            }
            else if (value is Byte[] buf)
            {
                if (buf == null || buf.Length < 1) return defaultValue;
                switch (buf.Length)
                {
                    case 1:
                        return buf[0];
                    case 2:
                        return BitConverter.ToInt16(buf, 0);
                    case 3:
                        return BitConverter.ToInt32(new Byte[] { buf[0], buf[1], buf[2], 0 }, 0);
                    case 4:
                        return BitConverter.ToInt32(buf, 0);
                    default:
                        // 凑够8字节
                        if (buf.Length < 8)
                        {
                            var bts = new Byte[8];
                            Buffer.BlockCopy(buf, 0, bts, 0, buf.Length);
                            buf = bts;
                        }
                        return BitConverter.ToSingle(buf, 0);
                }
            }
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return defaultValue; }
        }
        #endregion
        #region ConvertToDecimal
        /// <summary>转为Decimal</summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static decimal ConvertToDecimal(this object value, decimal defaultValue = 0.0m)
        {
            if (value.IsEmpty()) return defaultValue;
            if (value is String str)
            {
                str = ToDBC(str).Trim();
                if (str.IsEmpty()) return defaultValue;
                if (decimal.TryParse(str, out var n)) return n;
                return defaultValue;
            }
            else if (value is Byte[] buf)
            {
                if (buf == null || buf.Length < 1) return defaultValue;
                switch (buf.Length)
                {
                    case 1:
                        return buf[0];
                    case 2:
                        return BitConverter.ToInt16(buf, 0);
                    case 3:
                        return BitConverter.ToInt32(new Byte[] { buf[0], buf[1], buf[2], 0 }, 0);
                    case 4:
                        return BitConverter.ToInt32(buf, 0);
                    default:
                        // 凑够8字节
                        if (buf.Length < 8)
                        {
                            var bts = new Byte[8];
                            Buffer.BlockCopy(buf, 0, bts, 0, buf.Length);
                            buf = bts;
                        }
                        return (decimal ) BitConverter.ToDouble(buf, 0);
                }
            }

            try
            {
                return (decimal )Convert.ToDouble(value);
            }
            catch { return defaultValue; }
        }
        #endregion
        #region ConvertToString
        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static string  ConvertToString( object value)
        {
            if (value.IsEmpty()) return string.Empty;
            return value.ToString();
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static string ConvertToString(this object value, string defaultValue = "")
        {
            if (value.IsEmpty()) return defaultValue;
            return value.ToString();
        }
        #endregion
        #region ConvertToBool
        /// <summary>
        /// 转为布尔型。支持大小写True/False、0和非零
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static bool ConvertToBool(this object value, bool defaultValue = false)
        {
            if (value.IsEmpty()) return defaultValue;
            // 特殊处理字符串，也是最常见的
            if (value is String str)
            {
                str = str.Trim();
                if (str.IsEmpty()) return defaultValue;
                if (Boolean.TryParse(str, out var b)) return b;
                if (String.Equals(str, Boolean.TrueString, StringComparison.OrdinalIgnoreCase)) return true;
                if (String.Equals(str, Boolean.FalseString, StringComparison.OrdinalIgnoreCase)) return false;
                // 特殊处理用数字0和1表示布尔型
                str = ToDBC(str);
                if (Int32.TryParse(str, out var n)) return n > 0;
                return defaultValue;            
            }
            try
            {
                return Convert.ToBoolean(value);
            }
            catch { return defaultValue; }
        }
        #endregion
        /// <summary>
        /// 转为时间日期，转换失败时返回最小时间。支持字符串、整数（Unix秒）
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
        /// <returns></returns>
        public static  DateTime ConvertToDateTime(this object  value, DateTime defaultValue )
        {
           
            if (value.IsEmpty()) return defaultValue;
            // 特殊处理字符串，也是最常见的
            if (value is String str)            {
                str = str.Trim();
                if (str.IsEmpty()) return defaultValue;
                if (DateTime.TryParse(str, out var n)) return n;
                if (str.Contains("-") && DateTime.TryParseExact(str, "yyyy-M-d", null, DateTimeStyles.None, out n)) return n;
                if (str.Contains("/") && DateTime.TryParseExact(str, "yyyy/M/d", null, DateTimeStyles.None, out n)) return n;
                if (DateTime.TryParse(str, out n)) return n;
                return defaultValue;
            }
            // 特殊处理整数，Unix秒，绝对时间差，不考虑UTC时间和本地时间。
            if (value is Int32 k) return k == 0 ? DateTime.MinValue : _dt1970.AddSeconds(k);
            if (value is Int64 m)
            {
                if (m > 100 * 365 * 24 * 3600L)
                    return _dt1970.AddMilliseconds(m);
                else
                    return _dt1970.AddSeconds(m);
            }
            try
            {
                return Convert.ToDateTime(value);
            }
            catch { return defaultValue; }
        }

        #region ToDBC
        /// <summary>全角为半角</summary>
        /// <remarks>全角半角的关系是相差0xFEE0</remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public  static string ToDBC(this  string value)
        {
            var ch = value.ToCharArray();
            for (var i = 0; i < ch.Length; i++)
            {
                // 全角空格
                if (ch[i] == 0x3000)
                    ch[i] = (Char)0x20;
                else if (ch[i] > 0xFF00 && ch[i] < 0xFF5F)
                    ch[i] = (Char)(ch[i] - 0xFEE0);
            }
            return new String(ch);
        }
        #endregion
        #region ConvertToFullString
        /// <summary>
        /// 时间日期转为yyyy-MM-dd HH:mm:ss完整字符串
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="emptyValue">字符串空值时显示的字符串，null表示原样显示最小时间，String.Empty表示不显示</param>
        /// <returns></returns>
        public static string ConvertToFullString(this DateTime value, string emptyValue = null)
        {
            if (emptyValue != null && value <= DateTime.MinValue) return emptyValue;
            var cs = "yyyy-MM-dd HH:mm:ss".ToCharArray();
            var k = 0;
            var y = value.Year;
            cs[k++] = (Char)('0' + (y / 1000));
            y %= 1000;
            cs[k++] = (Char)('0' + (y / 100));
            y %= 100;
            cs[k++] = (Char)('0' + (y / 10));
            y %= 10;
            cs[k++] = (Char)('0' + y);
            k++;

            var m = value.Month;
            cs[k++] = (Char)('0' + (m / 10));
            m %= 10;
            cs[k++] = (Char)('0' + m);
            k++;

            m = value.Day;
            cs[k++] = (Char)('0' + (m / 10));
            m %= 10;
            cs[k++] = (Char)('0' + m);
            k++;

            m = value.Hour;
            cs[k++] = (Char)('0' + (m / 10));
            m %= 10;
            cs[k++] = (Char)('0' + m);
            k++;

            m = value.Minute;
            cs[k++] = (Char)('0' + (m / 10));
            m %= 10;
            cs[k++] = (Char)('0' + m);
            k++;

            m = value.Second;
            cs[k++] = (Char)('0' + (m / 10));
            m %= 10;
            cs[k++] = (Char)('0' + m);
            k++;

            return new String(cs);
        }
        #endregion
        /// <summary>
        /// 时间日期转为指定格式字符串
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="emptyValue">字符串空值时显示的字符串，null表示原样显示最小时间，String.Empty表示不显示</param>
        /// <returns></returns>
        public static  string  ConvertToString(DateTime value, string format, string emptyValue =null )
        {
            if (emptyValue != null && value <= DateTime.MinValue) return emptyValue;
            if (format == null || format == "yyyy-MM-dd HH:mm:ss") return ConvertToFullString(value, emptyValue);
            return value.ToString(format);
        }
        public static string GetRandomData(int beginSeed,int endseed=100)
        {
            var value=  new Random(Guid.NewGuid().GetHashCode()).Next(beginSeed, endseed);
            return value.ToString();

        }


    }
}
