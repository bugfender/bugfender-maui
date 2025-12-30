namespace Bugfender.Sdk
{
    public partial class BugfenderBinding : IBugfenderBinding
    {
        private static readonly Lazy<BugfenderBinding> lazy = new Lazy<BugfenderBinding>(() => new BugfenderBinding());
        private static readonly object sdkTypeLock = new object();
        private static bool sdkTypeSet;

        public static BugfenderBinding Instance { get { return lazy.Value; } }

        private BugfenderBinding() { }

        public void Init(BugfenderOptions options)
        {
            SetSdkTypeOnce();
#if NET7_0
            // .NET 7 implementation
            if (options.apiUri != null)
            {
                BugfenderSDK.Bugfender.SetApiURL(NSUrl.FromString(options.apiUri.ToString()));
            }
#else
            // .NET 8/9 implementation
            if (options.apiUri != null)
            {
                BugfenderSDK.Bugfender.SetApiURL(NSUrl.FromString(options.apiUri.ToString()));
            }
#endif
            if (options.baseUri != null)
            {
                BugfenderSDK.Bugfender.SetBaseURL(NSUrl.FromString(options.baseUri.ToString()));
            }
            BugfenderSDK.Bugfender.ActivateLogger(options.appKey);
            if (options.maximumLocalStorageSize != null)
            {
                BugfenderSDK.Bugfender.MaximumLocalStorageSize = options.maximumLocalStorageSize.Value;
            }
            BugfenderSDK.Bugfender.SetPrintToConsole(options.printToConsole);
            if (options.logUIEvents)
            {
                BugfenderSDK.Bugfender.EnableUIEventLogging();
            }
            if (options.nativeCrashReporting)
            {
                BugfenderSDK.Bugfender.EnableCrashReporting();
            }
            if (options.mauiCrashReporting)
            {
                AppDomain.CurrentDomain.UnhandledException += AppDomainExceptionHandler;
                TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionHandler;
            }
        }

        public Uri DeviceUri
        {
            get
            {
                NSUrl url = BugfenderSDK.Bugfender.DeviceIdentifierUrl();
                if (url == null)
                    return null;
                return new Uri(url.ToString());
            }
        }

        public Uri SessionUri
        {
            get
            {
                NSUrl url = BugfenderSDK.Bugfender.SessionIdentifierUrl();
                if (url == null)
                    return null;
                return new Uri(url.ToString());
            }
        }

        public bool ForceEnabled
        {
            set
            {
                BugfenderSDK.Bugfender.SetForceEnabled(value);
            }
        }

        public void SetDeviceString(string key, string value)
        {
            BugfenderSDK.Bugfender.SetDeviceString(s: value, key: key);
        }

        public void SetDeviceInteger(string key, int value)
        {
            BugfenderSDK.Bugfender.SetDeviceInteger(i: (ulong)value, key: key);
        }

        public void SetDeviceFloat(string key, float value)
        {
            BugfenderSDK.Bugfender.SetDeviceDouble(d: value, key: key);
        }

        public void RemoveDeviceKey(string key)
        {
            BugfenderSDK.Bugfender.RemoveDeviceKey(key);
        }

        public void Log(int lineNumber, String method, String file, LogLevel logLevel, String tag, String message)
        {
            // a negative lineNumber indicates we need to guess from the stack
            if (lineNumber < 0)
            {
                System.Diagnostics.StackFrame frame = new System.Diagnostics.StackTrace(true).GetFrame(2);
                if (frame.GetType().Namespace.StartsWith("Com.Bugfender"))
                {
                    frame = new System.Diagnostics.StackTrace(true).GetFrame(3);
                }

                lineNumber = frame.GetFileLineNumber();
                method = frame.GetMethod().Name;
                file = System.IO.Path.GetFileName(frame.GetFileName());
                if (method == null) method = "";
                if (file == null) file = "";
            }

            BugfenderSDK.Bugfender.LogWithLineNumber(lineNumber, method, file, MapLoglLevelToBFLogLevel(logLevel), tag, message);
        }

        private static BugfenderSDK.BFLogLevel MapLoglLevelToBFLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Fatal:
                    return BugfenderSDK.BFLogLevel.Fatal;
                case LogLevel.Error:
                    return BugfenderSDK.BFLogLevel.Error;
                case LogLevel.Warning:
                    return BugfenderSDK.BFLogLevel.Warning;
                case LogLevel.Info:
                    return BugfenderSDK.BFLogLevel.Info;
                case LogLevel.Debug:
                    return BugfenderSDK.BFLogLevel.Default;
                case LogLevel.Trace:
                    return BugfenderSDK.BFLogLevel.Trace;
                default:
                    return BugfenderSDK.BFLogLevel.Info;
            }
        }

        public void ForceSendOnce()
        {
            BugfenderSDK.Bugfender.ForceSendOnce();
        }

        public Uri SendIssue(string title, string markdown)
        {
            NSUrl url = BugfenderSDK.Bugfender.SendIssueReturningUrlWithTitle(title, markdown);
            if (url == null)
                return null;
            return new Uri(url.ToString());
        }

        public Uri SendCrash(string title, string text)
        {
            NSUrl url = BugfenderSDK.Bugfender.SendCrashWithTitle(title, text);
            if (url == null)
                return null;
            return new Uri(url.ToString());
        }

        /* TODO
        public static void ShowUserFeedbackViewController(string title, string hint, string subjectPlaceholder, string messagePlaceholder, string sendButtonTitle, string cancelButtonTitle, Action<Uri> completionAction)
        {
            var intent = Bugfender.GetUserFeedbackActivityIntent(this, title, hint, subjectPlaceholder, messagePlaceholder, sendButtonTitle);
            this.StartActivityForResult(intent, FeedbackActivityRequestCode);
        }*/

        public Uri SendUserFeedback(string subject, string message)
        {
            NSUrl url = BugfenderSDK.Bugfender.SendUserFeedbackReturningUrlWithSubject(subject, message);
            if (url == null)
                return null;
            return new Uri(url.ToString());
        }

        private static void AppDomainExceptionHandler(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var e = unhandledExceptionEventArgs.ExceptionObject as Exception;
            var title = e.Message;
            if (title == null)
            {
                title = e.ToString();
            }
            var detail = e.StackTrace;
            if (detail == null)
            {
                detail = "";
            }
            BugfenderSDK.Bugfender.SendCrashWithTitle(title, detail);
        }

        private static void UnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var title = unobservedTaskExceptionEventArgs.Exception.ToString();
            var detail = unobservedTaskExceptionEventArgs.Exception.StackTrace;
            if (detail == null)
            {
                detail = "";
            }
            BugfenderSDK.Bugfender.SendCrashWithTitle(title, detail);
        }

        private static void SetSdkTypeOnce()
        {
            if (sdkTypeSet)
            {
                return;
            }

            lock (sdkTypeLock)
            {
                if (sdkTypeSet)
                {
                    return;
                }

                // Tag requests as coming from the MAUI binding.
                BugfenderSDK.Bugfender.SetSDKType("netmaui", SdkVersion.Version);
                sdkTypeSet = true;
            }
        }
    }
}
