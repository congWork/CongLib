using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CongLib.Helpers
{
   
    public class CommonHelper
    {
        public static HostingPlatforms CurrentPlatform
        {
            get
            {
                string _currPlatform = "";
                string _appSKey = "CurrentHostingPlatform";
                try
                {
                    _currPlatform = Config_GetAppSetting(_appSKey);
                }
                catch
                {
                    _currPlatform = WebConfig_GetAppSetting(_appSKey);
                }
                return (HostingPlatforms)Enum.Parse(typeof(HostingPlatforms), _currPlatform);
            }
        }
        public static void WriteLine(string message, bool consoleOutPut = false)
        {
            if (consoleOutPut)
            {
                Console.WriteLine(DateTime.Now.ToString() + ": " + message);
            }
            WriteErrorLog(message);
        }
        public static void WriteErrorLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\MyLog.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                //do nothing
            }
        }
        public static void WriteErrorLog(Exception ex)
        {
            WriteErrorLog(ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
        }
        public static string Config_GetConnectionString(string key)
        {
            string result = "";
            result = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            return result;
        }
        public static string WebConfig_GetAppSetting(string key)
        {
            string result = "";

            result = ConfigurationManager.AppSettings[key];
            if (result == null)
            {
                throw new Exception(String.Format("Could not find setting '{0}',", key));
            }

            return result;
        }
        public static string Config_GetAppSetting(string key)
        {
            string result = "";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            result = config.AppSettings.Settings[key].Value;
            return result;

        }
        public static void Config_UpdateAppSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
        }
        public static DateTime? ConverStringToNullableDateTime(string inputStr)
        {
            DateTime? dt;
            DateTime tempResult;
            bool success = DateTime.TryParseExact(inputStr, "MMddyyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tempResult);
            if (!success)
            {
                dt = null;
            }
            else
            {
                if (tempResult < SqlDateTime.MinValue.Value)
                {
                    dt = null;
                }
                else if (tempResult > SqlDateTime.MaxValue.Value)
                {
                    dt = null;
                }
                else
                {
                    dt = tempResult;
                }
            }
            return dt;
        }
        public static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
        public static int? ConverStringToNullableInt(string inputStr)
        {
            int? result;
            int tempResult;
            bool success = int.TryParse(inputStr, out tempResult);
            if (!success)
            {
                result = null;
            }
            else
            {
                result = tempResult;

            }
            return result;
        }
        public static short? ConverStringToNullableShort(string inputStr)
        {
            short? result;
            short tempResult;
            bool success = short.TryParse(inputStr, out tempResult);
            if (!success)
            {
                result = null;
            }
            else
            {
                result = tempResult;

            }
            return result;
        }
        public static DateTime? TryAddHours(DateTime dt, double addedValue)
        {
            DateTime? result = null;
            DateTime? finalResult;
            try
            {
                result = dt.AddHours(addedValue);

            }
            catch (ArgumentOutOfRangeException ex)
            {
                result = null;
            }
            finally
            {
                finalResult = result;
            }

            return finalResult;

        }
        public static DateTime? ValidateDateTime(DateTime? tempResult)
        {
            DateTime? dt = null;
            if (tempResult.HasValue)
            {
                if (tempResult < SqlDateTime.MinValue.Value)
                {
                    dt = null;
                }
                else if (tempResult > SqlDateTime.MaxValue.Value)
                {
                    dt = null;
                }
                else
                {
                    dt = tempResult;
                }
            }

            return dt;
        }

        public static bool IsPassword(string strIn)
        {
            bool result = true;
            Regex passPat = new Regex(@"(?=^.{8,10}$)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*");
            result = passPat.IsMatch(strIn);
            return result;
        }

        public static bool IsValidEmail(string strIn)
        {
            bool result = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            Regex emailPat = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            result = emailPat.IsMatch(strIn);
            return result;
        }


    }
}
