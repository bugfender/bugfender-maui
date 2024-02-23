using Android.App;
using Android.Runtime;
using Bugfender.Sdk;

namespace Sample;

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
            appKey = "YOUR APP KEY",
            // apiUri = new Uri("https://api.bugfender.com"),
            // baseUri = new Uri("https://dashboard.bugfender.com"),
            printToConsole = true,
            nativeCrashReporting = true,
            mauiCrashReporting = true,
            logUIEvents = true,
        });
        bugfender.WriteLine("Logs for this device are here: {0}", bugfender.DeviceUri.ToString());
        bugfender.Warning("TAG", "This is a warning");
        bugfender.Error("TAG", "This is an error!");
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

