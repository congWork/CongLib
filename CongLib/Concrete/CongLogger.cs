using CongLib.Abstract;
using CongLib.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongLib.Concrete
{
    
    public class CongLogger : ICongLogger
    {
        private string _path;

        public CongLogger()
        {
            _path = AppDomain.CurrentDomain.BaseDirectory + "\\MyLog.txt";
        }
        public CongLogger(string path)
        {
            _path = path;
        }
        public void WriteLog(LogType logType, string message, bool consoleOutPut = false)
        {

            if (consoleOutPut)
            {
                Console.WriteLine(string.Format("{0}==>{1}==>{2}", DateTime.Now.ToString(), logType, message));
            }
            StreamWriter sw = null;
            try
            {
                if (File.Exists(_path))
                {
                    //check file size
                    FileInfo f = new FileInfo(_path);
                    double fileSizeInMb = CommonHelper.ConvertBytesToMegabytes(f.Length);
                    if (fileSizeInMb > 200)
                    {
                        //create a new file for logging
                        string origPath = this._path;
                        string basePath = origPath.Substring(0, origPath.LastIndexOf(".txt"));
                        DateTime currDateTime = DateTime.Now;
                        this._path = string.Format("{0}-{1}-{2}.txt", basePath, currDateTime.Millisecond, currDateTime.ToString("MMddyyyy"));
                    }
                }
                object logModel = new
                {
                    CreatedTime = DateTime.Now.ToString(),
                    LogType = logType.ToString(),
                    Message = message
                };

                string jsonLog = JsonConvert.SerializeObject(logModel);

                sw = new StreamWriter(_path, true);
                sw.WriteLine(jsonLog);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                //do nothing
            }
        }
    }
}
