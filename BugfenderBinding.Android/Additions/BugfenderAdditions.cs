using System;
using Android.Runtime;

namespace Com.Bugfender.Sdk
{
    public partial class Bugfender
    {
        public static void EnableXamarinCrashReporting()
        {
            AndroidEnvironment.UnhandledExceptionRaiser += HandleUnhandledException;
        }

        protected static void HandleUnhandledException(object sender, RaiseThrowableEventArgs args)
        {
            Exception e = (Exception)args.Exception;
            var id = Bugfender.SendIssue(e.Message + " (managed code exception)", "```\n" + e + "\n```");
            Console.WriteLine("Sending managed code exception: {0} {1}", id, e);
        }
    }
}
