using Android.App;
using Android.Runtime;
using Bugfender.Sdk;

namespace Sample
{
    [Application]
    public class MainApplication : MauiApplication
    {
        /// <summary>
        /// Set to <c>false</c> for production so apiUri/baseUri are omitted and the SDK
        /// uses its default (production) hosts.
        /// </summary>
        private const bool UseLocalBugfender = true;

        /// <summary>
        /// Local/dev only: host for a Bugfender stack running on your machine.
        /// - Android emulator: use <c>10.0.2.2</c> (loopback to the host).
        /// - Physical device: replace with your machine's LAN IP (e.g. <c>192.168.70.107</c>).
        /// Unused when <see cref="UseLocalBugfender"/> is <c>false</c>.
        /// </summary>
        private const string BugfenderHost = "10.0.2.2";

        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            BugfenderBinding bugfender = BugfenderBinding.Instance;
            // Local: pass apiUri/baseUri pointing at your machine.
            // Production: set UseLocalBugfender to false (omit overrides → SDK defaults).
            bugfender.Init(this, new BugfenderOptions
            {
                appKey = "K44Gu6T0ZwtlLPEhuFPFfcY3YDNmyyoH",
                apiUri = UseLocalBugfender ? new Uri($"http://{BugfenderHost}:3100/") : null,
                baseUri = UseLocalBugfender ? new Uri($"https://{BugfenderHost}:3000/") : null,
                printToConsole = true,
                nativeCrashReporting = true,
                mauiCrashReporting = true,
                logUIEvents = true,
                networkLoggingEnabled = true,
                networkLoggingCaptureBodies = true,
            });
            ConfigureNetworkLogging(bugfender);
        }

        private static void ConfigureNetworkLogging(BugfenderBinding bugfender)
        {
            // Redact auth header and password values
            bugfender.SetNetworkLoggingRequestObfuscationHandler((url, headers, body) =>
            {
                var safeHeaders = new Dictionary<string, string>(headers);
                if (safeHeaders.ContainsKey("Authorization"))
                {
                    safeHeaders["Authorization"] = "***REDACTED***";
                }
                var safeBody = body == null
                    ? null
                    : System.Text.RegularExpressions.Regex.Replace(
                        body,
                        "\"password\"\\s*:\\s*\"[^\"]*\"",
                        "\"password\":\"***REDACTED***\"",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Android.Util.Log.Info(
                    "BF/Sample",
                    $"obfuscate-request called url={url} auth={safeHeaders.GetValueOrDefault("Authorization")} body={safeBody}");
                return new NetworkRequestData(url, safeHeaders, safeBody);
            });
            bugfender.SetNetworkLoggingResponseObfuscationHandler((headers, body) =>
            {
                var safeHeaders = new Dictionary<string, string>(headers);
                if (safeHeaders.ContainsKey("Set-Cookie"))
                {
                    safeHeaders["Set-Cookie"] = "***REDACTED***";
                }
                return new NetworkResponseData(safeHeaders, body);
            });
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}

