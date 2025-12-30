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
                NSUrl? url = NSUrl.FromString(options.apiUri.ToString());
                if (url != null)
                {
                    BugfenderSDK.Bugfender.SetApiURL(url);
                }
            }
#else
            // .NET 8/9/10 implementation
            if (options.apiUri != null)
            {
                NSUrl? url = NSUrl.FromString(options.apiUri.ToString());
                if (url != null)
                {
                    BugfenderSDK.Bugfender.SetApiURL(url);
                }
            }
#endif
            if (options.baseUri != null)
            {
                NSUrl? url = NSUrl.FromString(options.baseUri.ToString());
                if (url != null)
                {
                    BugfenderSDK.Bugfender.SetBaseURL(url);
                }
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

        public Uri? DeviceUri
        {
            get
            {
                NSUrl? url = BugfenderSDK.Bugfender.DeviceIdentifierUrl();
                if (url == null)
                    return null;
                string? urlString = url.ToString();
                if (urlString == null)
                    return null;
                return new Uri(urlString);
            }
        }

        public Uri? SessionUri
        {
            get
            {
                NSUrl? url = BugfenderSDK.Bugfender.SessionIdentifierUrl();
                if (url == null)
                    return null;
                string? urlString = url.ToString();
                if (urlString == null)
                    return null;
                return new Uri(urlString);
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
                System.Diagnostics.StackFrame? frame = new System.Diagnostics.StackTrace(true).GetFrame(2);
                if (frame != null && frame.GetType().Namespace?.StartsWith("Com.Bugfender") == true)
                {
                    frame = new System.Diagnostics.StackTrace(true).GetFrame(3);
                }

                if (frame != null)
                {
                    lineNumber = frame.GetFileLineNumber();
#pragma warning disable IL2026 // Suppress trimming warning - GetMethod() is needed for stack trace logging
                    method = frame.GetMethod()?.Name ?? "";
#pragma warning restore IL2026
                    file = System.IO.Path.GetFileName(frame.GetFileName() ?? "");
                }
                else
                {
                    method = "";
                    file = "";
                }
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

        public Uri? SendIssue(string title, string markdown)
        {
            NSUrl? url = BugfenderSDK.Bugfender.SendIssueReturningUrlWithTitle(title, markdown);
            if (url == null)
                return null;
            string? urlString = url.ToString();
            if (urlString == null)
                return null;
            return new Uri(urlString);
        }

        public Uri? SendCrash(string title, string text)
        {
            NSUrl? url = BugfenderSDK.Bugfender.SendCrashWithTitle(title, text);
            if (url == null)
                return null;
            string? urlString = url.ToString();
            if (urlString == null)
                return null;
            return new Uri(urlString);
        }

        /* TODO
        public static void ShowUserFeedbackViewController(string title, string hint, string subjectPlaceholder, string messagePlaceholder, string sendButtonTitle, string cancelButtonTitle, Action<Uri> completionAction)
        {
            var intent = Bugfender.GetUserFeedbackActivityIntent(this, title, hint, subjectPlaceholder, messagePlaceholder, sendButtonTitle);
            this.StartActivityForResult(intent, FeedbackActivityRequestCode);
        }*/

        public Uri? SendUserFeedback(string subject, string message)
        {
            NSUrl? url = BugfenderSDK.Bugfender.SendUserFeedbackReturningUrlWithSubject(subject, message);
            if (url == null)
                return null;
            string? urlString = url.ToString();
            if (urlString == null)
                return null;
            return new Uri(urlString);
        }

        private static void AppDomainExceptionHandler(object? sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var e = unhandledExceptionEventArgs.ExceptionObject as Exception;
            if (e == null)
                return;
            
            var title = e.Message;
            if (title == null)
            {
                title = e.ToString();
            }
            var detail = e.StackTrace ?? "";
            BugfenderSDK.Bugfender.SendCrashWithTitle(title, detail);
        }

        private static void UnobservedTaskExceptionHandler(object? sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
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
