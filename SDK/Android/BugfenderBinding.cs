using System.Collections.Generic;
using Android.Runtime;
using Com.Bugfender.Sdk;
using NativeNetworkRequestData = Com.Bugfender.Sdk.NetworkRequestData;
using NativeNetworkResponseData = Com.Bugfender.Sdk.NetworkResponseData;

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

            ApplyNetworkLoggingOptions(options);
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

        public void SetNetworkLoggingEnabled(bool enabled)
        {
            Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingEnabled(enabled);
        }

        public void SetNetworkLoggingCaptureBodies(bool capture)
        {
            Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingCaptureBodies(capture);
        }

        public void SetNetworkLoggingCaptureErrorResponseBodies(bool capture)
        {
            Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingCaptureErrorResponseBodies(capture);
        }

        public void SetNetworkLoggingURLFilter(IReadOnlyList<string>? allowlist, IReadOnlyList<string>? denylist)
        {
            Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingURLFilter(
                ToJavaStringList(allowlist),
                ToJavaStringList(denylist));
        }

        public void SetNetworkLoggingMaxRequestsPerMinute(int? count)
        {
            Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingMaxRequestsPerMinute(
                count.HasValue ? Java.Lang.Integer.ValueOf(count.Value) : null);
        }

        public void SetNetworkLoggingRequestObfuscationHandler(NetworkLoggingRequestObfuscationHandler? handler)
        {
            if (handler == null)
            {
                Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingRequestObfuscationHandler(null);
                return;
            }

            Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingRequestObfuscationHandler(
                new RequestObfuscationHandlerBridge(handler));
        }

        public void SetNetworkLoggingResponseObfuscationHandler(NetworkLoggingResponseObfuscationHandler? handler)
        {
            if (handler == null)
            {
                Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingResponseObfuscationHandler(null);
                return;
            }

            Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingResponseObfuscationHandler(
                new ResponseObfuscationHandlerBridge(handler));
        }

        /// <summary>
        /// Instruments an OkHttp <c>OkHttpClient.Builder</c> (passed as a Java object) so
        /// Bugfender can capture its traffic. Requires the bundled <c>android-okhttp</c> AAR
        /// and OkHttp on the classpath.
        /// </summary>
        public void InstrumentOkHttpBuilder(Java.Lang.Object? okHttpBuilder)
        {
            if (okHttpBuilder == null)
            {
                return;
            }

            Com.Bugfender.Sdk.BugfenderOkHttpHooks.InstrumentOkHttpBuilder(okHttpBuilder);
        }

        /// <inheritdoc />
        public InstrumentedNetworkResult SendInstrumentedNetworkRequest(
            string url,
            string method = "GET",
            string? body = null,
            IReadOnlyDictionary<string, string>? headers = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                url = "https://example.com/";
            }

            string httpMethod = string.IsNullOrWhiteSpace(method) ? "GET" : method.ToUpperInvariant();

            Dictionary<string, string>? headerDict = null;
            if (headers != null)
            {
                headerDict = new Dictionary<string, string>();
                foreach (var pair in headers)
                {
                    if (!string.IsNullOrEmpty(pair.Key) && pair.Value != null)
                    {
                        headerDict[pair.Key] = pair.Value;
                    }
                }
            }

            try
            {
                var payload = Com.Bugfender.Sdk.Maui.InstrumentedNetworkHelper.Send(
                    url,
                    httpMethod,
                    body,
                    headerDict);
                if (payload == null)
                {
                    throw new InvalidOperationException("Instrumented network helper returned null");
                }

                int status = 0;
                bool shouldCapture = false;
                string? requestId = null;

                if (payload.TryGetValue("status", out var statusObj) && statusObj is Java.Lang.Number statusNum)
                {
                    status = statusNum.IntValue();
                }
                if (payload.TryGetValue("shouldCapture", out var captureObj) && captureObj is Java.Lang.Boolean captureBool)
                {
                    shouldCapture = captureBool.BooleanValue();
                }
                if (payload.TryGetValue("requestId", out var requestIdObj) && requestIdObj != null)
                {
                    requestId = requestIdObj.ToString();
                }

                return new InstrumentedNetworkResult(status, shouldCapture, requestId);
            }
            catch (Java.Lang.Throwable ex)
            {
                Android.Util.Log.Error("BF/MauiNet", "instrumented request failed", ex);
                try
                {
                    Com.Bugfender.Sdk.Bugfender.ForceSendOnce();
                }
                catch
                {
                    // ignore
                }
                throw new InvalidOperationException(ex.Message ?? "instrumented network request failed", ex);
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("BF/MauiNet", Java.Lang.Throwable.FromException(ex), "instrumented request failed");
                try
                {
                    Com.Bugfender.Sdk.Bugfender.ForceSendOnce();
                }
                catch
                {
                    // ignore
                }
                throw;
            }
        }

        public static void HandleUnhandledException(object? sender, RaiseThrowableEventArgs args)
        {
            Exception e = (Exception)args.Exception;
            var id = Com.Bugfender.Sdk.Bugfender.SendCrash(e.Message + " (managed code exception)", e.ToString());
            Console.WriteLine("Sending managed code exception: {0} {1}", id, e);
        }

        private static void ApplyNetworkLoggingOptions(BugfenderOptions options)
        {
            if (options.networkLoggingEnabled)
            {
                Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingEnabled(true);
            }
            if (options.networkLoggingCaptureBodies)
            {
                Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingCaptureBodies(true);
            }
            if (options.networkLoggingCaptureErrorResponseBodies)
            {
                Com.Bugfender.Sdk.Bugfender.SetNetworkLoggingCaptureErrorResponseBodies(true);
            }
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

        private static IList<string>? ToJavaStringList(IReadOnlyList<string>? values)
        {
            if (values == null)
            {
                return null;
            }

            var list = new List<string>(values.Count);
            for (int i = 0; i < values.Count; i++)
            {
                list.Add(values[i]);
            }
            return list;
        }

        private static IDictionary<string, string> ToManagedHeaders(IDictionary<string, string>? headers)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (headers == null)
            {
                return result;
            }

            foreach (var pair in headers)
            {
                if (pair.Key == null)
                {
                    continue;
                }
                result[pair.Key] = pair.Value ?? "";
            }
            return result;
        }

        private sealed class RequestObfuscationHandlerBridge : Java.Lang.Object, INetworkLoggingRequestObfuscationHandler
        {
            private readonly NetworkLoggingRequestObfuscationHandler _handler;

            public RequestObfuscationHandlerBridge(NetworkLoggingRequestObfuscationHandler handler)
            {
                _handler = handler;
            }

            public NativeNetworkRequestData? Obfuscate(string? url, IDictionary<string, string>? headers, string? body)
            {
                var managed = _handler(url ?? "", ToManagedHeaders(headers), body);
                return new NativeNetworkRequestData(
                    managed.Url,
                    new Dictionary<string, string>(managed.Headers),
                    managed.Body);
            }
        }

        private sealed class ResponseObfuscationHandlerBridge : Java.Lang.Object, INetworkLoggingResponseObfuscationHandler
        {
            private readonly NetworkLoggingResponseObfuscationHandler _handler;

            public ResponseObfuscationHandlerBridge(NetworkLoggingResponseObfuscationHandler handler)
            {
                _handler = handler;
            }

            public NativeNetworkResponseData? Obfuscate(IDictionary<string, string>? headers, string? body)
            {
                var managed = _handler(ToManagedHeaders(headers), body);
                return new NativeNetworkResponseData(
                    new Dictionary<string, string>(managed.Headers),
                    managed.Body);
            }
        }
    }
}
