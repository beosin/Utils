using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace OL.Utils.Extensions
{
    /// <summary>X组件异常</summary>
    [Serializable]
    public   class ExceptionEx:Exception 
    {
        /// <summary>初始化</summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
       // public ExceptionEx(String format, params Object[] args) : base(format.F(args)) { }
        public static string ExceptionToString( Exception exception)
        {
            return exception.Message 
                .ToString();
            //return exception.Demystify()
            //    .ToString();
        }
    }
}
