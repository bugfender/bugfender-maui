using Android.App;
using Android.Content.PM;
using Android.OS;
using Bugfender.Sdk;

namespace Sample;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
       
        BugfenderBinding bugfender = BugfenderBinding.Instance;
        bugfender.ActivateLogger("YOUR APP KEY", true); // true == enable logging to console
        bugfender.EnableUIEventLogging(Application);
        bugfender.EnableMauiCrashReporting();
        bugfender.WriteLine("Logs for this device are here: {0}", bugfender.DeviceUri.ToString());
        bugfender.Warning("TAG", "This is a warning");
        bugfender.Error("TAG", "This is an error!");
    }
}
