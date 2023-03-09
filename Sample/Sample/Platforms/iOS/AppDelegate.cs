using Bugfender.Sdk;
using Foundation;
using UIKit;

namespace Sample;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        BugfenderBinding bugfender = BugfenderBinding.Instance;
        bugfender.ActivateLogger("YOUR APP KEY", true); // true == enable logging to console
        bugfender.EnableUIEventLogging();
        bugfender.EnableMauiCrashReporting();
        bugfender.WriteLine("Logs for this device are here: {0}", bugfender.DeviceUri.ToString());
        bugfender.Warning("TAG", "This is a warning");
        bugfender.Error("TAG", "This is an error!");

        return base.FinishedLaunching(app, options);
    }
}
