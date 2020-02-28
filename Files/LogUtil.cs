using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace OL.Utils.Files
{
    /// <summary>
    /// 多线程安全Log记录工具
    /// </summary>
    public class LogUtil
    {
        //System.Diagnostics.Debug.WriteLine("Debug模式可见")
        //System.Diagnostics.Trace.WriteLine("Debug、Release都可见");
        private static Thread WriteThread;
        private static readonly Queue<string> MsgQueue;
        private static readonly string FilePath;

        private static Boolean autoResetEventFlag = false;
        private static AutoResetEvent aEvent = new AutoResetEvent(false);
        private static bool flag = true;
        public static bool LogFlag = true;

        #region LogUtil
        static LogUtil()
        {
            FilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\";
            WriteThread = new Thread(WriteMsg);
            MsgQueue = new Queue<string>();
            WriteThread.Start();
        } 
        #endregion

        #region LogInfo
        public static void LogInfo(string msg)
        {
            Monitor.Enter(MsgQueue);
            MsgQueue.Enqueue(string.Format("{0} {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "Info", msg));
            Monitor.Exit(MsgQueue);
            if (autoResetEventFlag)
            {
                aEvent.Set();
            }
        }
        public static void LogInfo(string format, object arg0)
        {
            LogInfo(string.Format(format, arg0));
        }
        public static void LogInfo(string format, object arg0, object arg1)
        { LogInfo(string.Format(format, arg0, arg1)); }
        public static void LogInfo(string format, object arg0, object arg1, object arg2)
        { LogInfo(string.Format(format, arg0, arg1, arg2)); }
        public static void LogInfo(string format, object arg0, object arg1, object arg2, object arg3)
        { LogInfo(string.Format(format, arg0, arg1, arg2, arg3)); }
        public static void LogInfo(string format, params object[] args)
        {
            var msg = format;
            try
            {
                msg = string.Format(format, args);
            }
            catch
            { }
            LogInfo(msg);
        }


        #endregion
        #region LogError
        public static void LogError(string msg)
        {
            Monitor.Enter(MsgQueue);
            
            MsgQueue.Enqueue(string.Format("{0} {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "Error", msg));
            Monitor.Exit(MsgQueue);
            if (autoResetEventFlag)
            {
                aEvent.Set();
            }
        }
        public static void LogError(string format, object arg0)
        {
            LogError(string.Format(format,arg0));
        }
        public static void LogError(string format, object arg0,object arg1)
        { LogError(string.Format(format, arg0,arg1));  }       
        public static void LogError(string format, object arg0,object arg1,object arg2)
        { LogError(string.Format(format, arg0, arg1,arg2)); }
        public static void LogError(string format, object arg0, object arg1, object arg2,object arg3)
        { LogError(string.Format(format, arg0, arg1,arg2,arg3)); }
        public static void LogError(string format,params object[] args)
        {
            var msg = format;
            try
            {
                msg = string.Format(format,args);                
            }
            catch
            { }
            LogError(msg);
        }
        #endregion
        #region LogWarn
        public static void LogWarn(string msg)
        {
            Monitor.Enter(MsgQueue);
            MsgQueue.Enqueue(string.Format("{0} {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "Warn", msg));
            Monitor.Exit(MsgQueue);
            if (autoResetEventFlag)
            {
                aEvent.Set();
            }
        }
        public static void LogWarn(string format, object arg0)
        {
            LogWarn(string.Format(format, arg0));
        }
        public static void LogWarn(string format, object arg0, object arg1)
        { LogWarn(string.Format(format, arg0, arg1)); }
        public static void LogWarn(string format, object arg0, object arg1, object arg2)
        { LogWarn(string.Format(format, arg0, arg1, arg2)); }
        public static void LogWarn(string format, object arg0, object arg1, object arg2, object arg3)
        { LogWarn(string.Format(format, arg0, arg1, arg2, arg3)); }
        public static void LogWarn(string format, params object[] args)
        {
            var msg = format;
            try
            {
                msg = string.Format(format, args);
            }
            catch
            { }
            LogWarn(msg);
        }
        #endregion

        #region ExitThread
        /// <summary>
        /// ExitThread是退出日志记录线程的方法，一旦退出，无法开启，一般在程序关闭时执行
        /// </summary>
        public static void ExitThread()
        {
            flag = false;
            aEvent.Set();//恢复线程执行
        }
        #endregion
        #region WriteMsg
        private static void WriteMsg()
        {
            while (flag)
            {
                //进行记录
                if (LogFlag)
                {
                    autoResetEventFlag = false;
                    FileUtil.CreateDirectory(FilePath);                    
                    string fileName = FilePath + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                    var logStreamWriter = new StreamWriter(fileName, true);
                    while (MsgQueue.Count > 0)
                    {
                        Monitor.Enter(MsgQueue);
                        string msg = MsgQueue.Dequeue();
                        Monitor.Exit(MsgQueue);
                        logStreamWriter.WriteLine(msg);
                        if (GetFileSize(fileName) > 1024 * 5)
                        {
                            logStreamWriter.Flush();
                            logStreamWriter.Close();
                            CopyToBakup(fileName);
                            logStreamWriter = new StreamWriter(fileName, false);
                            logStreamWriter.Write("");
                            logStreamWriter.Flush();
                            logStreamWriter.Close();
                            logStreamWriter = new StreamWriter(fileName, true);
                        }
                        //下面用于DbgView.exe工具进行在线调试
                        System.Diagnostics.Debug.WriteLine("Debug:" + msg);
                        System.Diagnostics.Trace.WriteLine("Release:" + msg);
                    }
                    logStreamWriter.Flush();
                    logStreamWriter.Close();
                    autoResetEventFlag = true;
                    aEvent.WaitOne();
                }
                else
                {
                    autoResetEventFlag = true;
                    aEvent.WaitOne();
                }
            }
        }
        #endregion
        #region CopyToBak
        private static long GetFileSize(string fileName)
        {
            long strRe = 0;
            if (File.Exists(fileName))
            {
                var myFs = new FileInfo(fileName);
                strRe = myFs.Length / 1024;
                //Console.WriteLine(strRe);
            }
            return strRe;
        }
        #endregion
        #region CopyToBakup
        private static void CopyToBakup(string fileName)
        {
            int fileCount = 0;
            string sBakName = "";
            do
            {
                fileCount++;
                sBakName = fileName + "." + fileCount + ".BAK";
            }
            while (File.Exists(sBakName));
            File.Copy(fileName, sBakName);
        }
        #endregion

        void Demo()
        {
            LogUtil.LogFlag = true; //开启记录
            LogUtil.LogInfo("==========日志记录内容 Info====");
            LogUtil.LogWarn("==========日志记录内容 Warn====");
            LogUtil.LogError("==========日志记录内容 Error====");
            LogUtil.LogFlag = false;//停止记录
            LogUtil.ExitThread();// 退出日志记录线程，一般在程序退出时候调用。
        }
    }
}
