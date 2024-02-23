# Bugfender bindings for .NET MAUI

Bugfender is a tool to aggregate all your mobile application logs in a single place, so you can access them remotely. This repository provides bindings for iOS and Android on MAUI projects. Also provides a sample solution that shows the integration.

In order to use Bugfender, you will need an account which you can [create here](https://bugfender.com).

## Usage

### Add the NuGet to your project

Add the [**Bugfender.Sdk**](https://www.nuget.org/packages/Bugfender.Sdk) NuGet to your .NET MAUI project.

### Adding Bugfender to your iOS project

* Edit your project > **Properties** > **Build** > **iOS** > **Build** and set the **Linker behavior** to **Link Frameworks SDKs Only**.
* Edit `AppDelegate.cs` and initialize Bugfender in the `FinishedLaunching` method, like this:

```
using Bugfender.Sdk;

// ...

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        BugfenderBinding bugfender = BugfenderBinding.Instance;
        bugfender.Init(new BugfenderOptions
        {
            appKey = "YOUR APP KEY",
            // apiUri = new Uri("https://api.bugfender.com"),
            // baseUri = new Uri("https://dashboard.bugfender.com"),
            printToConsole = true,
            nativeCrashReporting = true,
            mauiCrashReporting = true,
            logUIEvents = true,
            // maximumLocalStorageSize = 5*1024*1024,
        });
        // some examples on how to use Bugfender
        bugfender.WriteLine("Logs for this device are here: {0}", bugfender.DeviceUri.ToString());
        bugfender.Warning("TAG", "This is a warning");
        bugfender.Error("TAG", "This is an error!");
        bugfender.SetDeviceString("user", "test@example.com");
        return base.FinishedLaunching(application, launchOptions);
    }
```

* Replace *YOUR APP KEY* with your Bugfender app key.

### Adding Bugfender to your Android project

* Look for your application object (usually in `Platforms/Android/MainApplication.cs`). If you don't have it, create a new application class by right-clicking on *Your Project* > **New File...** > **General** > **Empty Class**. Then add the following code:

```
using Android.App;
using Android.Runtime;
using Bugfender.Sdk;

[Application]
public class MainApplication : Application
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
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
            // maximumLocalStorageSize = 5*1024*1024,
        });
        // some examples on how to use Bugfender
        bugfender.WriteLine("Logs for this device are here: {0}", bugfender.DeviceUri.ToString());
        bugfender.Warning("TAG", "This is a warning");
        bugfender.Error("TAG", "This is an error!");
    }
}
```
* Replace *YOUR APP KEY* with your Bugfender app key.

## Quick reference

Setup:

 * `apiUri` and `baseUri`: are used to point to a specific Bugfender instance, like a specific region or on-prem instance.
 * `nativeCrashReporting` and `mauiCrashReporting`: enable crash reports on the native (iOS/Android) and .NET MAUI stacks.
 * `maximumLocalStorageSize`: changes the amount of storage the log cache can take on disk. The size is in bytes and can be up to 50 megabytes.

Other `BugfenderBinding` methods that let you control behavior and enable integrations:

 * `Uri DeviceUri { get; }`: the device URI can be used to see the logs of this device in the Bugfender Dashboard. Useful if you want to integrate Bugfender deeper in your workflow, such as showing the Bugfender URL of a device in your help desk software.
 * `Uri SessionUri { get; }`: similar to `DeviceUri`, points to the logs of the current execution of the app.
 * `void ForceSendOnce()`: forces the SDK to send logs for the current session. Can be useful if you want to inspect the logs only for some sessions. Take a look at `SendIssue()`.
 * `bool ForceEnabled { set; }`: forces the SDK to send logs, even if the device is disabled from the dashboard. Use with caution because there is no way to disable logging from the dashboard if you change your mind, prefer to enable devices from the dashboard instead.

Sending logs:

 * Use `WriteLine("hello world")` as you would use the system-provided `Console`, `Trace` or `Debug` objects.
 * Use `Fatal()`/`Error()`/`Warning()`/`Info()`/`Debug()`/`Trace()` to log events with a certain level. For example `Error("this shouldn't happen")`
 * You can also specify a tag to easily group logs that belong to the same category, for example `Error("Networking", "Server responded with status 500")`
 * Use `void Log(int lineNumber, String method, String file, LogLevel logLevel, String tag, String message)` if you want to provide all details for each log.

**Tip:** If your project is currently using `Console`/`Trace`/`Debug` to write logs, you can search & replace them to `Bugfender` to get started quickly.

Custom device-associated data:
 * `void SetDeviceString(string key, string value)`: associates data to this device so you can search for this device. You can set details like the user ID, so later on you can find the device corresponding to a user easily.
 * `void SetDeviceInteger(string key, int value)`: same for integers.
 * `void SetDeviceFloat(string key, float value)`: same for floats.
 * `void RemoveDeviceKey(string key)`: removes data associated from a device.

 Send rich data:

 * `Uri SendIssue(string title, string markdown)`: send information about an exceptional situation you need to look into. This will force all logs in the current session to be sent, even if log sending is disabled.
 * `Uri SendUserFeedback(string subject, string message)`: send user feedback that will appear in the dashboard. This will force all logs in the current session to be sent, even if log sending is disabled.
 * `Uri SendCrash(string title, string text)`: manually send crash information. Usually you do not want to use this, enable automated crash reporting instead. This will force all logs in the current session to be sent, even if log sending is disabled.

## Full reference

This project is wrapping the native iOS and Android SDKs. You have the full reference here:

* For more information, have a look at the [iOS SDK reference](https://bugfender.github.io/BugfenderSDK-iOS/).
* For more information, have a look at the [Android SDK reference](http://www.javadoc.io/doc/com.bugfender.sdk/android).


# Troubleshooting

* **XAML Hot Reload is disabled because your iOS Linker Settings for <your app name> are unsupported**: this happens because .NET MAUI does not support linking native libraries and XAML Hot Reload at the same time. This is a limitation of .NET MAUI and there is no solution at the moment.
* **The iOS application crashes immediately at start with `System.ArgumentOutOfRangeException`**: you probably forgot to edit your project > **Properties** > **Build** > **iOS** > **Build** and set the **Linker behavior** to **Link Frameworks SDKs Only**.
* **Bugfender does not work on MacCatalyst**: this is a limitation of this bindings project. If you would like to contribute, you can submit a Pull-Request.

# Contributions

Thanks to [@AlonRom](http://github.com/AlonRom/) for their contributions to this project (conversion from Xamarin to .NET MAUI).