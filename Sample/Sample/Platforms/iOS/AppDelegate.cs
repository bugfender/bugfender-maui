using Foundation;
using UIKit;
using Bugfender.Sdk;

namespace Sample;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        BugfenderBinding bugfender = BugfenderBinding.Instance;
        bugfender.Init(new BugfenderOptions
        {
            appKey = "VabTcPpzCgdXqXDFo8SmSOjx3PuJ9fB9",
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
        bugfender.SetDeviceString("user", "test@example.com");
        return base.FinishedLaunching(application, launchOptions);
    }

}

