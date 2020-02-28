using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OL.Utils.Extensions
{
    public static class DateTimeEx
    {
          const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
         const string DateTimeFormat1 = "yyyy-MM-dd HH:mm";
         const string DateTimeFormat2 = "yyyy/MM/dd HH:mm:ss";
         const string DateTimeFormatString = "yyyyMMddHHmmss";
         const string DateTimeShortFormat = "yyyy-MM-dd";
         const string DateTimeShortFormat2 = "yyyy/MM/dd";
         const string SnokId = "yyyyMMddHHmmssffff";
        public static DateTime DateTime => DateTime.Now;

        public static DateTime ToDateTime(this string str)
        {
            return DateTime.TryParse(str, out DateTime date) == true ? date : DateTime.MinValue;
        }

        public static DateTime ToDateTimeB(this string str)
        {
            return DateTime.TryParse(str + " 00:00:00.000", out DateTime date) == true ? date : DateTime.MinValue;
        }

        public static DateTime ToDateTimeE(this string str)
        {
            return DateTime.TryParse(str + " 23:59:59.997", out DateTime date) == true ? date : DateTime.MinValue;
        }

        public static string ToDateTimeString(this DateTime dateTime, string format = DateTimeFormat)
        {
            return dateTime.ToString(format);
        }

        public static DateTime GetDateTime(string format = DateTimeFormat)
        {
            return DateTime.ToString(format).ToDateTime();
        }

        public static string GetDateTimeS(string format = DateTimeFormat)
        {
            return DateTime.ToString(format);
        }

      

        public static DateTime TToDateTime(string timeStamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1, 0, 0, 0);
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }


        /// <summary>
        /// 将时间格式化成 年月日 的形式,如果时间为null，返回当前系统时间
        /// </summary>
        /// <param name="dt">年月日分隔符</param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static  string GetFormatDate(this DateTime dt, char Separator)
        {
            if (dt != null && !dt.Equals(DBNull.Value))
            {
                string tem = string.Format("yyyy{0}MM{1}dd", Separator, Separator);
                return dt.ToString(tem);
            }
            else
            {
                return GetFormatDate(DateTime.Now, Separator);
            }
        }
        /// <summary>
        /// 将时间格式化成 时分秒 的形式,如果时间为null，返回当前系统时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static string GetFormatTime(this DateTime dt, char Separator)
        {
            if (dt != null && !dt.Equals(DBNull.Value))
            {
                string tem = string.Format("hh{0}mm{1}ss", Separator, Separator);
                return dt.ToString(tem);
            }
            else
            {
                return GetFormatDate(DateTime.Now, Separator);
            }
        }
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            decimal mm = (decimal)((decimal)Second / (decimal)60);
            return Convert.ToInt32(Math.Ceiling(mm));
        }

        #region 返回某年某月最后一天
        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
            int Day = lastDay.Day;
            return Day;
        }
        #endregion

        #region 返回时间差

        /// <summary>
        /// 获取日期差
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string GetDateDiff(DateTime src)
        {
            string result = null;
            var currentSecond = (long)(DateTime.Now - src).TotalSeconds;
            long minSecond = 60;                //60s = 1min  
            var hourSecond = minSecond * 60;   //60*60s = 1 hour  
            var daySecond = hourSecond * 24;   //60*60*24s = 1 day  
            var weekSecond = daySecond * 7;    //60*60*24*7s = 1 week  
            var monthSecond = daySecond * 30;  //60*60*24*30s = 1 month  
            var yearSecond = daySecond * 365;  //60*60*24*365s = 1 year  

            if (currentSecond >= yearSecond)
            {
                var year = (int)(currentSecond / yearSecond);
                result = $"{year}年前";
            }
            else if (currentSecond < yearSecond && currentSecond >= monthSecond)
            {
                var month = (int)(currentSecond / monthSecond);
                result = $"{month}个月前";
            }
            else if (currentSecond < monthSecond && currentSecond >= weekSecond)
            {
                var week = (int)(currentSecond / weekSecond);
                result = $"{week}周前";
            }
            else if (currentSecond < weekSecond && currentSecond >= daySecond)
            {
                var day = (int)(currentSecond / daySecond);
                result = $"{day}天前";
            }
            else if (currentSecond < daySecond && currentSecond >= hourSecond)
            {
                var hour = (int)(currentSecond / hourSecond);
                result = $"{hour}小时前";
            }
            else if (currentSecond < hourSecond && currentSecond >= minSecond)
            {
                var min = (int)(currentSecond / minSecond);
                result = $"{min}分钟前";
            }
            else if (currentSecond < minSecond && currentSecond >= 0)
            {
                result = "刚刚";
            }
            else
            {
                result = src.ToString("yyyy/MM/dd HH:mm:ss");
            }
            return result;
        }
        public static string GetDateDiff(DateTime beginTime, DateTime endTime)
        {
            return GetDateDiff(beginTime, endTime);
            //string dateDiff = null;
            //try
            //{
            //    //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            //    //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            //    //TimeSpan ts = ts1.Subtract(ts2).Duration();
            //    TimeSpan ts = endTime - beginTime;
            //    if (ts.Days >= 1)
            //    {
            //        dateDiff = beginTime.Month.ToString() + "月" + beginTime.Day.ToString() + "日";
            //    }
            //    else
            //    {
            //        if (ts.Hours > 1) 
            //        {
            //            dateDiff = ts.Hours.ToString() + "小时前";
            //        }
            //        else
            //        {
            //            dateDiff = ts.Minutes.ToString() + "分钟前";
            //        }
            //    }
            //}
            //catch
            //{ }
            //return dateDiff;
        }
        #endregion

        #region 获得两个日期的间隔
        /// <summary>
        /// 获得两个日期的间隔
        /// </summary>
        /// <param name="DateTime1">日期一。</param>
        /// <param name="DateTime2">日期二。</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static TimeSpan DateDiff2(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts;
        }
        #endregion

        #region 格式化日期时间
        /// <summary>
        /// 格式化日期时间
        /// </summary>
        /// <param name="dateTime1">日期时间</param>
        /// <param name="dateMode">显示模式0:yyyy-MM-dd 1:yyyy-MM-dd HH:mm:ss 2:yyyy/MM/dd 3:yyyy年MM月dd日 4:MM-dd 5:MM月dd日 6:MM月dd日 7:yyyy-MM 8:yyyy/MM 9:yyyy年MM月 10：yyyy-MM-dd HH:mm  11：yyyy/MM/dd HH:mm:ss 12：yyyyMMddHHmmss 13：yyyyMMddHHmmssffff</param>
        /// <returns>0-9种模式的日期</returns>
        public static string FormatDate(this DateTime dateTime1, int dateMode)
        {
            switch (dateMode)
            {
                case 0:
                    return dateTime1.ToString("yyyy-MM-dd");
                case 1:
                    return dateTime1.ToString("yyyy-MM-dd HH:mm:ss");
                case 2:
                    return dateTime1.ToString("yyyy/MM/dd");
                case 3:
                    return dateTime1.ToString("yyyy年MM月dd日");
                case 4:
                    return dateTime1.ToString("MM-dd");
                case 5:
                    return dateTime1.ToString("MM/dd");
                case 6:
                    return dateTime1.ToString("MM月dd日");
                case 7:
                    return dateTime1.ToString("yyyy-MM");
                case 8:
                    return dateTime1.ToString("yyyy/MM");
                case 9:
                    return dateTime1.ToString("yyyy年MM月");
               case 10:
                    return dateTime1.ToString("yyyy-MM-dd HH:mm");
               case 11:
                    return dateTime1.ToString("yyyy/MM/dd HH:mm:ss");
               case 12:
                    return dateTime1.ToString("yyyyMMddHHmmss");
               case 13:
                    return dateTime1.ToString("yyyyMMddHHmmssffff");
             //case 14:
             //       return dateTime1.ToString("yyyyMMddHHmmss");
             //case 15:
             //       return dateTime1.ToString("yyyyMMddHHmmss");
                default:
                    return dateTime1.ToString();
            }
        }
        #endregion
        #region 返回相差 秒数、分钟数、小时数
        public static long GetTotalMilliseconds()
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0);
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
        /// <summary>
        /// 返回与当前时间相差的秒数
        /// </summary>
        /// <param name="dateTime">时间1</param>
        /// <param name="second">秒数</param>
        /// <returns>相差的秒数</returns>
        public static int DateOfDiffSeconds(DateTime dateTime, int second = 0)
        {
            return DateOfDiffSeconds(dateTime, DateTime.Now);
        }
        /// <summary>
        /// 返回相差的秒数
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static int DateOfDiffSeconds(DateTime beginTime,DateTime endTime, int second = 0)
        {
            if (beginTime.ToString("yyyy-MM-dd") == "1900-01-01") return 1;
            TimeSpan ts = endTime - beginTime.AddSeconds(second);
            if (ts.TotalSeconds > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalSeconds < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalSeconds;
        }
        /// <summary>
        /// 返回与当前相差的分钟数
        /// </summary>
        /// <param name="time">时间字符串</param>
        /// <param name="minutes">分钟数</param>
        /// <returns>相差的分钟数</returns>
        public static int DateOfDiffMinutes(DateTime dateTime, int minutes = 0)
        {
            return DateOfDiffMinutes(dateTime, DateTime.Now, minutes);
        }
        /// <summary>
        /// 返回相差的分钟数
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="minutes"></param>
        /// <returns>相差的分钟数</returns>
        public static int DateOfDiffMinutes(DateTime beginTime,DateTime endTime, int minutes = 0)
        {
            if (beginTime.ToString("yyyy-MM-dd") == "1900-01-01") return 1;
            TimeSpan ts = endTime - beginTime.AddMinutes(minutes);
            if (ts.TotalMinutes > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalMinutes < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalMinutes;
        }
        /// <summary>
        /// 返回与当前相差的小时数
        /// </summary>
        /// <param name="time">时间字符串</param>
        /// <param name="hours">小时数</param>
        /// <returns>相差的小时数</returns>
        public static int DateOfDiffHours(DateTime dateTime, int hours = 0)
        {
            return DateOfDiffHours(dateTime, DateTime.Now, hours);
        }

        /// <summary>
        /// 返回相差的小时数
        /// </summary>
        /// <param name="time">时间字符串</param>
        /// <param name="hours">小时数</param>
        /// <returns>相差的小时数</returns>
        public static int DateOfDiffHours(DateTime beiginTime,DateTime endTime, int hours = 0)
        {
            if (beiginTime.ToString("yyyy-MM-dd") == "1900-01-01") return 1;
            TimeSpan ts = endTime - beiginTime.AddHours(hours);
            if (ts.TotalHours > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalHours < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalHours;
        }

        #endregion

        #region 得到随机日期
        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns>间隔日期之间的 随机日期</returns>
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            System.TimeSpan ts = new System.TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }
        #endregion
    }
}