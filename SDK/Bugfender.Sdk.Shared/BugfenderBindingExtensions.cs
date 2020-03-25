using System;

namespace Bugfender.Sdk
{
    public static class BugfenderBindingExtensions
    {
        public static void Fatal(this IBugfenderBinding bf, string message)
        {
            bf.Log(-1, "", "", LogLevel.Fatal, "", message);
        }

        public static void Error(this IBugfenderBinding bf, string message)
        {
            bf.Log(-1, "", "", LogLevel.Error, "", message);
        }

        public static void Warning(this IBugfenderBinding bf, string message)
        {
            bf.Log(-1, "", "", LogLevel.Warning, "", message);
        }

        public static void Info(this IBugfenderBinding bf, string message)
        {
            bf.Log(-1, "", "", LogLevel.Info, "", message);
        }

        public static void Debug(this IBugfenderBinding bf, string message)
        {
            bf.Log(-1, "", "", LogLevel.Debug, "", message);
        }

        public static void Trace(this IBugfenderBinding bf, string message)
        {
            bf.Log(-1, "", "", LogLevel.Trace, "", message);
        }

        public static void Fatal(this IBugfenderBinding bf, string tag, string message)
        {
            bf.Log(-1, "", "", LogLevel.Fatal, tag, message);
        }

        public static void Error(this IBugfenderBinding bf, string tag, string message)
        {
            bf.Log(-1, "", "", LogLevel.Error, tag, message);
        }

        public static void Warning(this IBugfenderBinding bf, string tag, string message)
        {
            bf.Log(-1, "", "", LogLevel.Warning, tag, message);
        }

        public static void Info(this IBugfenderBinding bf, string tag, string message)
        {
            bf.Log(-1, "", "", LogLevel.Info, tag, message);
        }

        public static void Debug(this IBugfenderBinding bf, string tag, string message)
        {
            bf.Log(-1, "", "", LogLevel.Debug, tag, message);
        }

        public static void Trace(this IBugfenderBinding bf, string tag, string message)
        {
            bf.Log(-1, "", "", LogLevel.Trace, tag, message);
        }

        public static void WriteLine(this IBugfenderBinding bf, ValueType value)
        {
            bf.Log(-1, "", "", LogLevel.Info, "", value.ToString());
        }

        public static void WriteLine(this IBugfenderBinding bf)
        {
            bf.Log(-1, "", "", LogLevel.Info, "", "");
        }

        public static void WriteLine(this IBugfenderBinding bf, char[] buffer, int index, int count)
        {
            bf.Log(-1, "", "", LogLevel.Info, "", new string(buffer, index, count));
        }

        public static void WriteLine(this IBugfenderBinding bf, string format, params object[] arg)
        {
            string message = String.Format(format, arg);
            bf.Log(-1, "", "", LogLevel.Info, "", message);
        }

        public static void WriteLine(this IBugfenderBinding bf, string value)
        {
            bf.Log(-1, "", "", LogLevel.Info, "", value);
        }

        public static void WriteLine(this IBugfenderBinding bf, object value)
        {
            bf.Log(-1, "", "", LogLevel.Info, "", value.ToString());
        }
    }
}