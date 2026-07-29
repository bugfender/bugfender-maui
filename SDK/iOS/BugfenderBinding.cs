using System.Collections.Generic;
using Foundation;

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

            ApplyNetworkLoggingOptions(options);
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

        public void SetNetworkLoggingEnabled(bool enabled)
        {
            BugfenderSDK.Bugfender.SetNetworkLoggingEnabled(enabled);
        }

        public void SetNetworkLoggingCaptureBodies(bool capture)
        {
            BugfenderSDK.Bugfender.SetNetworkLoggingCaptureBodies(capture);
        }

        public void SetNetworkLoggingCaptureErrorResponseBodies(bool capture)
        {
            BugfenderSDK.Bugfender.SetNetworkLoggingCaptureErrorResponseBodies(capture);
        }

        public void SetNetworkLoggingURLFilter(IReadOnlyList<string>? allowlist, IReadOnlyList<string>? denylist)
        {
            BugfenderSDK.Bugfender.SetNetworkLoggingURLFilterWithAllowlist(
                allowlist == null ? null : allowlist as string[] ?? allowlist.ToArray(),
                denylist == null ? null : denylist as string[] ?? denylist.ToArray());
        }

        public void SetNetworkLoggingMaxRequestsPerMinute(int? count)
        {
            BugfenderSDK.Bugfender.SetNetworkLoggingMaxRequestsPerMinute(
                count.HasValue ? NSNumber.FromInt32(count.Value) : null);
        }

        public void SetNetworkLoggingRequestObfuscationHandler(NetworkLoggingRequestObfuscationHandler? handler)
        {
            if (handler == null)
            {
                BugfenderSDK.Bugfender.SetNetworkLoggingRequestObfuscationHandler(null);
                return;
            }

            BugfenderSDK.Bugfender.SetNetworkLoggingRequestObfuscationHandler((url, headers, body) =>
            {
                var managed = handler(url ?? "", ToManagedHeaders(headers), body);
                return new BugfenderSDK.BFNetworkRequestData(
                    managed.Url,
                    ToNativeHeaders(managed.Headers),
                    managed.Body);
            });
        }

        public void SetNetworkLoggingResponseObfuscationHandler(NetworkLoggingResponseObfuscationHandler? handler)
        {
            if (handler == null)
            {
                BugfenderSDK.Bugfender.SetNetworkLoggingResponseObfuscationHandler(null);
                return;
            }

            BugfenderSDK.Bugfender.SetNetworkLoggingResponseObfuscationHandler((headers, body) =>
            {
                var managed = handler(ToManagedHeaders(headers), body);
                return new BugfenderSDK.BFNetworkResponseData(
                    ToNativeHeaders(managed.Headers),
                    managed.Body);
            });
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
            using var request = new NSMutableUrlRequest(NSUrl.FromString(url)!);
            request.HttpMethod = httpMethod;

            var headerDict = new NSMutableDictionary();
            bool hasAuthorization = false;
            if (headers != null)
            {
                foreach (var pair in headers)
                {
                    if (string.IsNullOrEmpty(pair.Key) || pair.Value == null)
                    {
                        continue;
                    }
                    if (string.Equals(pair.Key, "Authorization", StringComparison.OrdinalIgnoreCase))
                    {
                        hasAuthorization = true;
                    }
                    headerDict[pair.Key] = new NSString(pair.Value);
                }
            }
            if (!hasAuthorization)
            {
                headerDict["Authorization"] = new NSString("secret-token");
            }
            request.Headers = headerDict;

            if (httpMethod is "POST" or "PUT" or "PATCH")
            {
                var payload = body ?? "{}";
                request.Body = NSData.FromString(payload, NSStringEncoding.UTF8);
                if (request.Headers == null || request.Headers["Content-Type"] == null)
                {
                    var withContentType = new NSMutableDictionary(headerDict);
                    withContentType["Content-Type"] = new NSString("application/json; charset=utf-8");
                    request.Headers = withContentType;
                }
            }

            var semaphore = new System.Threading.ManualResetEventSlim(false);
            NSUrlResponse? urlResponse = null;
            NSError? error = null;

            NSUrlSession.SharedSession.CreateDataTask(request, (data, response, err) =>
            {
                urlResponse = response;
                error = err;
                semaphore.Set();
            }).Resume();

            if (!semaphore.Wait(TimeSpan.FromSeconds(20)))
            {
                BugfenderSDK.Bugfender.ForceSendOnce();
                throw new TimeoutException("Instrumented network request timed out");
            }

            BugfenderSDK.Bugfender.ForceSendOnce();

            if (error != null)
            {
                throw new InvalidOperationException(error.LocalizedDescription ?? "network_error");
            }

            int status = 0;
            if (urlResponse is NSHttpUrlResponse httpResponse)
            {
                status = (int)httpResponse.StatusCode;
            }

            return new InstrumentedNetworkResult(status, shouldCapture: true, requestId: null);
        }

        private static void ApplyNetworkLoggingOptions(BugfenderOptions options)
        {
            if (options.networkLoggingEnabled)
            {
                BugfenderSDK.Bugfender.SetNetworkLoggingEnabled(true);
            }
            if (options.networkLoggingCaptureBodies)
            {
                BugfenderSDK.Bugfender.SetNetworkLoggingCaptureBodies(true);
            }
            if (options.networkLoggingCaptureErrorResponseBodies)
            {
                BugfenderSDK.Bugfender.SetNetworkLoggingCaptureErrorResponseBodies(true);
            }
        }

        private static IDictionary<string, string> ToManagedHeaders(NSDictionary? headers)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (headers == null)
            {
                return result;
            }

            foreach (var key in headers.Keys)
            {
                var keyString = key?.ToString();
                if (keyString == null)
                {
                    continue;
                }
                result[keyString] = headers.ObjectForKey(key)?.ToString() ?? "";
            }
            return result;
        }

        private static NSDictionary ToNativeHeaders(IDictionary<string, string> headers)
        {
            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var pair in headers)
            {
                keys.Add(new NSString(pair.Key));
                values.Add(new NSString(pair.Value ?? ""));
            }
            return NSDictionary.FromObjectsAndKeys(values.ToArray(), keys.ToArray());
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
