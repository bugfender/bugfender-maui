using System.Collections.Generic;

namespace Bugfender.Sdk
{
    public struct BugfenderOptions
    {
        public string appKey;
        public Uri? apiUri;
        public Uri? baseUri;
        public uint? maximumLocalStorageSize;
        public bool printToConsole;
        public bool nativeCrashReporting;
        public bool mauiCrashReporting;
        public bool logUIEvents;
        /// <summary>Enable network request/response capture. Defaults to <c>false</c>.</summary>
        public bool networkLoggingEnabled;
        /// <summary>Capture request/response bodies when network logging is enabled. Defaults to <c>false</c>.</summary>
        public bool networkLoggingCaptureBodies;
        /// <summary>
        /// Capture response bodies only for HTTP status &gt;= 400 when full body capture is off.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool networkLoggingCaptureErrorResponseBodies;
    }

    public interface IBugfenderBinding
    {
        Uri? DeviceUri { get; }
        Uri? SessionUri { get; }
        bool ForceEnabled { set; }
        void SetDeviceString(string key, string value);
        void SetDeviceInteger(string key, int value);
        void SetDeviceFloat(string key, float value);
        void RemoveDeviceKey(string key);
        void Log(int lineNumber, string method, string file, LogLevel logLevel, string tag, string message);
        void ForceSendOnce();
        Uri? SendIssue(string title, string markdown);
        Uri? SendCrash(string title, string text);
        Uri? SendUserFeedback(string subject, string message);

        /// <summary>Enable or disable network request/response capture. Defaults to <c>false</c>.</summary>
        void SetNetworkLoggingEnabled(bool enabled);

        /// <summary>Enable or disable full request/response body capture. Defaults to <c>false</c>.</summary>
        void SetNetworkLoggingCaptureBodies(bool capture);

        /// <summary>
        /// Capture response bodies only for HTTP status &gt;= 400 when full body capture is off.
        /// </summary>
        void SetNetworkLoggingCaptureErrorResponseBodies(bool capture);

        /// <summary>
        /// URL filtering: include only URLs matching <paramref name="allowlist"/>;
        /// exclude URLs matching <paramref name="denylist"/>. Null = no filter.
        /// </summary>
        void SetNetworkLoggingURLFilter(IReadOnlyList<string>? allowlist, IReadOnlyList<string>? denylist);

        /// <summary>Limit how many network logs are captured per calendar minute. Null = no limit.</summary>
        void SetNetworkLoggingMaxRequestsPerMinute(int? count);

        /// <summary>Optional request obfuscation handler applied before a network log is sent.</summary>
        void SetNetworkLoggingRequestObfuscationHandler(NetworkLoggingRequestObfuscationHandler? handler);

        /// <summary>Optional response obfuscation handler applied before a network log is sent.</summary>
        void SetNetworkLoggingResponseObfuscationHandler(NetworkLoggingResponseObfuscationHandler? handler);

        /// <summary>
        /// Sample / verification helper: send an HTTP request through the platform client that
        /// Bugfender instruments (OkHttp on Android, URLSession on iOS) so it appears as
        /// <c>bf_network</c>. Default .NET <c>HttpClient</c> is not captured on Android.
        /// </summary>
        InstrumentedNetworkResult SendInstrumentedNetworkRequest(
            string url,
            string method = "GET",
            string? body = null,
            IReadOnlyDictionary<string, string>? headers = null);
    }
}
