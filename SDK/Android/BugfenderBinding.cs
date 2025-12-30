using Android.Runtime;

namespace Bugfender.Sdk
{
    public partial class BugfenderBinding : IBugfenderBinding
    {
        private static readonly Lazy<BugfenderBinding> lazy = new Lazy<BugfenderBinding>(() => new BugfenderBinding());
        private static bool sdkTypeSet;

        public static BugfenderBinding Instance { get { return lazy.Value; } }

        private BugfenderBinding() { }

        public void Init(Application app, BugfenderOptions options)
        {
            EnsureSdkType();
#if NET7_0
            // .NET 7 implementation
            if (options.apiUri != null)
            {
                Com.Bugfender.Sdk.Bugfender.SetApiUrl(options.apiUri.ToString());
            }
#else
            // .NET 8/9 implementation
            if (options.apiUri != null)
            {
                Com.Bugfender.Sdk.Bugfender.SetApiUrl(options.apiUri.ToString());
            }
#endif
            if (options.baseUri != null)
            {
                Com.Bugfender.Sdk.Bugfender.SetBaseUrl(options.baseUri.ToString());
            }
            Com.Bugfender.Sdk.Bugfender.Init(Application.Context, options.appKey, options.printToConsole, false);
            if (options.maximumLocalStorageSize != null)
            {
                Com.Bugfender.Sdk.Bugfender.SetMaximumLocalStorageSize(options.maximumLocalStorageSize.Value);
            }
            if (options.logUIEvents)
            {
                Com.Bugfender.Sdk.Bugfender.EnableUIEventLogging(app);
            }
            if (options.nativeCrashReporting)
            {
                Com.Bugfender.Sdk.Bugfender.EnableCrashReporting();
            }
            if (options.mauiCrashReporting)
            {
                AndroidEnvironment.UnhandledExceptionRaiser += HandleUnhandledException;
            }
        }

        public Uri? DeviceUri
        {
            get
            {
                Java.Net.URL? url = Com.Bugfender.Sdk.Bugfender.DeviceUrl;
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
                Java.Net.URL? url = Com.Bugfender.Sdk.Bugfender.SessionUrl;
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
                Com.Bugfender.Sdk.Bugfender.SetForceEnabled(value);
            }
        }

        public void EnableUIEventLogging(Application application)
        {
            Com.Bugfender.Sdk.Bugfender.EnableUIEventLogging(application);
        }

        public void SetDeviceString(string key, string value)
        {
            Com.Bugfender.Sdk.Bugfender.SetDeviceString(key, value);
        }

        public void SetDeviceInteger(string key, int value)
        {
            Com.Bugfender.Sdk.Bugfender.SetDeviceInteger(key, Java.Lang.Integer.ValueOf(value));
        }

        public void SetDeviceFloat(string key, float value)
        {
            Com.Bugfender.Sdk.Bugfender.SetDeviceFloat(key, Java.Lang.Float.ValueOf(value));
        }

        public void RemoveDeviceKey(string key)
        {
            Com.Bugfender.Sdk.Bugfender.RemoveDeviceKey(key);
        }

        public void Log(int lineNumber, string method, string file, LogLevel logLevel, string tag, string message)
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
                    method = frame.GetMethod()?.Name ?? "";
                    file = System.IO.Path.GetFileName(frame.GetFileName() ?? "");
                }
                else
                {
                    method = "";
                    file = "";
                }
            }

            Com.Bugfender.Sdk.Bugfender.Log(lineNumber, method, file, MapLoglLevelToSdkLogLevel(logLevel), tag, message);
        }

        private static Com.Bugfender.Sdk.LogLevel MapLoglLevelToSdkLogLevel(LogLevel logLevel)
        {
#pragma warning disable CS8603 // Enum values are never null, but compiler doesn't recognize this
            return logLevel switch
            {
                LogLevel.Fatal => Com.Bugfender.Sdk.LogLevel.Fatal,
                LogLevel.Error => Com.Bugfender.Sdk.LogLevel.Error,
                LogLevel.Warning => Com.Bugfender.Sdk.LogLevel.Warning,
                LogLevel.Info => Com.Bugfender.Sdk.LogLevel.Info,
                LogLevel.Debug => Com.Bugfender.Sdk.LogLevel.Debug,
                LogLevel.Trace => Com.Bugfender.Sdk.LogLevel.Trace,
                _ => Com.Bugfender.Sdk.LogLevel.Info
            };
#pragma warning restore CS8603
        }

        public void ForceSendOnce()
        {
            Com.Bugfender.Sdk.Bugfender.ForceSendOnce();
        }

        public Uri? SendIssue(string title, string markdown)
        {
            Java.Net.URL? url = Com.Bugfender.Sdk.Bugfender.SendIssue(title, markdown);
            if (url == null)
                return null;
            string? urlString = url.ToString();
            if (urlString == null)
                return null;
            return new Uri(urlString);
        }

        public Uri? SendCrash(string title, string text)
        {
            Java.Net.URL? url = Com.Bugfender.Sdk.Bugfender.SendCrash(title, text);
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
            Java.Net.URL? url = Com.Bugfender.Sdk.Bugfender.SendUserFeedback(subject, message);
            if (url == null)
                return null;
            string? urlString = url.ToString();
            if (urlString == null)
                return null;
            return new Uri(urlString);
        }

        public static void HandleUnhandledException(object? sender, RaiseThrowableEventArgs args)
        {
            Exception e = (Exception)args.Exception;
            var id = Com.Bugfender.Sdk.Bugfender.SendCrash(e.Message + " (managed code exception)", e.ToString());
            Console.WriteLine("Sending managed code exception: {0} {1}", id, e);
        }

        private static void EnsureSdkType()
        {
            if (sdkTypeSet)
            {
                return;
            }

            // Tag requests as coming from the MAUI binding.
            Com.Bugfender.Sdk.Bugfender.SetSdkType("netmaui", SdkVersion.Version);
            sdkTypeSet = true;
        }
    }
}
