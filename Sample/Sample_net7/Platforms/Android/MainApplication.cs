using Android.App;
using Android.Runtime;
using Bugfender.Sdk;

namespace Sample
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            BugfenderBinding bugfender = BugfenderBinding.Instance;
            bugfender.Init(this, new BugfenderOptions
            {
                appKey = "VabTcPpzCgdXqXDFo8SmSOjx3PuJ9fB9",
                printToConsole = true,
                nativeCrashReporting = true,
                mauiCrashReporting = true,
                logUIEvents = true
            });
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}

