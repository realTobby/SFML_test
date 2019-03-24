using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_Test
{
    public enum LogLevel
    {
        CRITICAL,
        INFORMATION,
        WARNING,
        ERROR
    }

    public class Logger
    {
        public void Log(LogLevel level, string eventName)
        {
            Console.WriteLine("[" + level.ToString() + "] ==> " + eventName);
        }
    }
}
