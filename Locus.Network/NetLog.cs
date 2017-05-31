using System;
using System.Collections.Generic;
using System.Text;

namespace Locus.Network
{
    public static class NetLog
    {
        public static void Debug(string val)
        {
            Console.WriteLine(val);
        }

        public static void Log(string val)
        {
            Console.WriteLine(val);
        }

        public static void Warn(string val)
        {
            Console.WriteLine(val);
        }

        public static void Error(string val)
        {
            Console.WriteLine(val);
        }
    }
}
