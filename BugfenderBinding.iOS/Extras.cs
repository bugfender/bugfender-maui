using System;
using System.Diagnostics;
using System.IO;

namespace BugfenderSDK
{
	public partial class Bugfender
	{
		private static void LogMessage(string message)
		{
			StackFrame frame = new StackTrace(true).GetFrame(2);

			nint lineNumber = frame.GetFileLineNumber();
			string method = frame.GetMethod().Name;
			string file = Path.GetFileName(frame.GetFileName());
			BFLogLevel level = BFLogLevel.Default;
			string tag = "";

			if (method == null) method = "";
			if (file == null) file = "";

			Bugfender.Log(lineNumber, method, file, level, tag, message);
			Console.WriteLine("{0} ({1}:{2}) {3}", method, file, lineNumber, message);
		}

		public static void WriteLine(ValueType value)
		{
			Bugfender.LogMessage(value.ToString());
		}

		public static void WriteLine()
		{
			Bugfender.LogMessage("");
		}

		public static void WriteLine(char[] buffer, int index, int count)
		{
			Bugfender.LogMessage(new string(buffer, index, count));
		}

		public static void WriteLine(string format, params object[] arg)
		{
			string message = String.Format(format, arg);
			Bugfender.LogMessage(message);
		}

		public static void WriteLine(string value)
		{
			Bugfender.LogMessage(value);
		}

		public static void WriteLine(object value)
		{
			Bugfender.LogMessage(value.ToString());
		}

        public static void EnableXamarinCrashReporting()
        {
            Mono.Runtime.RemoveSignalHandlers();
            try {
                Bugfender.EnableCrashReporting();
            }
            finally
            {
                Mono.Runtime.InstallSignalHandlers();
            }
        }
	}
}