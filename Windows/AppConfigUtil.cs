using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using OL.Utils.Extensions;
namespace OL.Utils.Windows
{
   public  class AppConfigUtil
    {
        /// <summary>
        /// 修改和添加AppSettings中配置 如果相应的Key存在则修改 如不存在则添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetConfigValue(string key, object  value)
        {
            try
            {
                var setValue = string.Empty;
                if (!value.IsEmpty()) setValue = value.ToString();
                 Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] != null)
                {
                    config.AppSettings.Settings[key].Value = setValue;
                }
                else
                {
                    config.AppSettings.Settings.Add(key, setValue);
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyName"></param>
        public static  void DeleteConfigValue(string keyName)
        {
            //删除配置文件键为keyName的项  
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(keyName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 查看相应Key的Value
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static  string GetConfigValue(string keyName)
        {
            //返回配置文件中键为keyName的项的值  
            return ConfigurationManager.AppSettings[keyName];
        }
    }
}
