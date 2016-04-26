using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongLib.Abstract
{
    public interface ICongLogger
    {
        void WriteLog(LogType logType, string message, bool consoleOutPut);
    }
}
