using System;
using Android.App;
using Android.Runtime;

namespace Bugfender.Sdk
{

    public partial class BugfenderBinding: IBugfenderBinding
    {
        private static readonly Lazy<BugfenderBinding> lazy = new Lazy<BugfenderBinding>(() => new BugfenderBinding());
        
        public static BugfenderBinding Instance { get { return lazy.Value; } }
        
        private BugfenderBinding() { }

        public void ActivateLogger(string appToken, bool printToConsole)
        {
            Com.Bugfender.Sdk.Bugfender.Init(Application.Context, appToken, printToConsole);
        }

        public void SetApiUri(Uri uri)
        {
            Com.Bugfender.Sdk.Bugfender.SetApiUrl(uri.ToString());
		}

		public void SetBaseUri(Uri uri)
        {
            Com.Bugfender.Sdk.Bugfender.SetBaseUrl(uri.ToString());
		}

		public UInt32 MaximumLocalStorageSize
        {
            set
            {
                Com.Bugfender.Sdk.Bugfender.SetMaximumLocalStorageSize(value);
            }
        }

		public Uri DeviceUri
        { 
            get
            {
                Java.Net.URL url = Com.Bugfender.Sdk.Bugfender.DeviceUrl;
                if (url == null)
                    return null;
                return new Uri(url.ToString());
            }
        }

		public Uri SessionUri
        {
            get
            {
                Java.Net.URL url = Com.Bugfender.Sdk.Bugfender.SessionUrl;
                if (url == null)
                    return null;
                return new Uri(url.ToString());
            }
        }

        public bool ForceEnabled
        {
            set {
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
            Com.Bugfender.Sdk.Bugfender.SetDeviceInteger(key, new Java.Lang.Integer(value));
        }

		public void SetDeviceFloat(string key, float value)
        {
            Com.Bugfender.Sdk.Bugfender.SetDeviceFloat(key, new Java.Lang.Float(value));
        }

		public void RemoveDeviceKey(string key)
        {
            Com.Bugfender.Sdk.Bugfender.RemoveDeviceKey(key);
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

            Com.Bugfender.Sdk.Bugfender.Log(lineNumber, method, file, MapLoglLevelToSdkLogLevel(logLevel), tag, message);
        }

        private static Com.Bugfender.Sdk.LogLevel MapLoglLevelToSdkLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Fatal:
                    return Com.Bugfender.Sdk.LogLevel.Fatal;
                case LogLevel.Error:
                    return Com.Bugfender.Sdk.LogLevel.Error;
                case LogLevel.Warning:
                    return Com.Bugfender.Sdk.LogLevel.Warning;
                case LogLevel.Info:
                    return Com.Bugfender.Sdk.LogLevel.Info;
                case LogLevel.Debug:
                    return Com.Bugfender.Sdk.LogLevel.Debug;
                case LogLevel.Trace:
                    return Com.Bugfender.Sdk.LogLevel.Trace;
                default:
                    return Com.Bugfender.Sdk.LogLevel.Info;
            }
        }

        public void ForceSendOnce()
        {
            Com.Bugfender.Sdk.Bugfender.ForceSendOnce();
        }

		public Uri SendIssue(string title, string markdown)
        {
            Java.Net.URL url = Com.Bugfender.Sdk.Bugfender.SendIssue(title, markdown);
            if (url == null)
                return null;
            return new Uri(url.ToString());
        }

        public Uri SendCrash(string title, string text)
        {
            Java.Net.URL url = Com.Bugfender.Sdk.Bugfender.SendCrash(title, text);
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
            Java.Net.URL url = Com.Bugfender.Sdk.Bugfender.SendUserFeedback(subject, message);
            if (url == null)
                return null;
            return new Uri(url.ToString());
        }

        public void EnableXamarinCrashReporting()
        {
            AndroidEnvironment.UnhandledExceptionRaiser += HandleUnhandledException;
        }

        public static void HandleUnhandledException(object sender, RaiseThrowableEventArgs args)
        {
            Exception e = (Exception)args.Exception;
            var id = Com.Bugfender.Sdk.Bugfender.SendCrash(e.Message + " (managed code exception)", e.ToString());
            Console.WriteLine("Sending managed code exception: {0} {1}", id, e);
        }
    }
}