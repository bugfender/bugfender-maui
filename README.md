# Bugfender SDK for Xamarin

Bugfender is a tool to aggregate all your mobile application logs in on a place, so you can access them remotely. This repository provides bindings for iOS and Android on Xamarin projects. Also provides a sample solution that shows the integration.

In order to use Bugfender, you will need an account which you can [create here](https://bugfender.com).

## Usage

### Add the NuGet to your project

Add the NuGet **Bugfender.Sdk** to both your iOS and Android projects.

### Adding Bugfender to your iOS project

* Edit `AppDelegate.cs` and initialize Bugfender in the `FinishedLaunching` method, like this:

```
using Bugfender.Sdk;

// ...

public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
{
	BugfenderBinding bugfender = BugfenderBinding.Instance;
	bugfender.ActivateLogger("YOUR APP KEY", true); // true == enable logging to console
	bugfender.EnableUIEventLogging();
	bugfender.EnableXamarinCrashReporting();
	bugfender.WriteLine("Logs for this device are here: {0}", bugfender.DeviceUri.ToString());
	bugfender.Warning("TAG", "This is a warning");
	bugfender.Error("TAG", "This is an error!");
	return true;
}
```

* Replace *YOUR APP KEY* with your Bugfender app key.
* Edit your project > Options > iOS Bundle Signing > Custom Entitlements and ensure it is set to `Entitlements.plist` for all configurations and platforms.

### Adding Bugfender to your Android project

* If you don't have one, create a new application class by right-clicking on *Your Project* > **New File...** > **General** > **Empty Class**. Then add the following code (in this example the class is called `SampleApplication`, you can name it whatever you want):

```
using System;
using Android.App;
using Android.Runtime;

using Com.Bugfender.Sdk;

[Application]
public class SampleApplication : Application
{
    public SampleApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
    {
    }

	public override void OnCreate()
	{
		base.OnCreate();
		BugfenderBinding bugfender = BugfenderBinding.Instance;
		bugfender.ActivateLogger("YOUR APP KEY", true); // true == enable logging to console
		bugfender.EnableUIEventLogging(this);
        bugfender.EnableXamarinCrashReporting();
		bugfender.WriteLine("Logs for this device are here: {0}", bugfender.DeviceUri.ToString());
		bugfender.Warning("TAG", "This is a warning");
		bugfender.Error("TAG", "This is an error!");
	}
}
```
* Replace *YOUR APP KEY* with your Bugfender app key.

## Quick reference

Setup:

 * `void ActivateLogger(string appToken, bool printToConsole)`: enables Bugfender. Call this as soon as the application is launched.
 * `void EnableXamarinCrashReporting()`: enables crash reports.
 * `UInt32 MaximumLocalStorageSize { set; }`: change the amount of storage the log cache can take on disk. The size is in bytes and can be up to 50 megabytes.
 * `Uri DeviceUri { get; }`: the device URI can be used to see the logs of this device in the Bugfender Dashboard. Useful if you want to integrate Bugfender deeper in your workflow, such as showing the Bugfender URL of a device in your help desk software.
 * `Uri SessionUri { get; }`: similar to `DeviceUri`, points to the logs of the current execution of hte app.
 * `void ForceSendOnce()`: forces the SDK to send logs for the current session. Can be useful if you want to inspect the logs only for some sessions. Take a look at `SendIssue()`.
 * `bool ForceEnabled { set; }`: forces the SDK to send logs, even if the device is disabled from the dashboard. Use with caution because there is no way to disable logging from the dashboard if you change your mind, prefer to enable devices from the dashboard instead.

Sending logs:

 * Use `WriteLine("hello world")` as you would use the system-provided `Console`, `Trace` or `Debug` objects.
 * Use `Fatal()`/`Error()`/`Warning()`/`Info()`/`Debug()`/`Trace()` to log events with a certain level. For example `Error("this shouldn't happen")`
 * You can also specify a tag to easily group logs that belong to the same category, for example `Error("Networking", "Server responded with status 500")`
 * Use `void Log(int lineNumber, String method, String file, LogLevel logLevel, String tag, String message)` if you wan to provide all details for each log.

**Tip:** If your project is currently `Console`/`Trace`/`Debug` to write logs, you can search & replace them to `Bugfender` to get started quickly.

 Custom device associated data:
 * `void SetDeviceString(string key, string value)`: associates data to this device so you can search for this device. You can set details like the user ID, so later on you can find the device corresponding to a user easily.
 * `void SetDeviceInteger(string key, int value)`: same for integers.
 * `void SetDeviceFloat(string key, float value)`: same for floats.
 * `void RemoveDeviceKey(string key)`: removes data associated from a device.

 Send rich data:

 * `Uri SendIssue(string title, string markdown)`: send information about an exceptional situation you need to look into. This will force all logs in the current session to be sent, even if log sending is disabled.
 * `Uri SendUserFeedback(string subject, string message)`: send user feedback that will appear in the dashboard. This will force all logs in the current session to be sent, even if log sending is disabled.
 * `Uri SendCrash(string title, string text)`: manually send crash information. Usually you do not want to use this,enable automated crash reporting instead. This will force all logs in the current session to be sent, even if log sending is disabled.

## Full reference

This project is wrapping the native iOS and Android SDKs. You have the full reference here:

* For more information, have a look at the [iOS SDK reference](https://bugfender.github.io/BugfenderSDK-iOS/).
* For more information, have a look at the [Android SDK reference](http://www.javadoc.io/doc/com.bugfender.sdk/android).

## Contributing

This project is open source, please feel free to contribute by filing new issues or submitting push requests.

The SDK is in the `SDK` folder. You can compile your own version of the NuGet by right-clicking on the Bugfender.Sdk.NuGet project and selecting Create NuGet Package.

This repository contains the Bugfender iOS and Android SDKs, which can be updated anytime and maybe are not updated here. At the moment of writing this, the SDKs used are:

* Android 3.0.8
* iOS 1.10.1

### Updating iOS

Follow these steps for updating:

* Download the latest version of the [iOS SDK from GitHub](https://github.com/bugfender/BugfenderSDK-iOS) and add it to the `Binding.iOS` project.
* Edit the `Properties/AssemblyInfo.cs` file to reflect the version.
* Update `ApiDefinition.cs` by using [Objective Sharpie](https://developer.xamarin.com/guides/cross-platform/macios/binding/objective-sharpie/). Manually check which are the methods updated and merge them. For reference, the invocation looks something like this: `sharpie bind BugfenderSDK.framework/Headers/BugfenderSDK.h -scope BugfenderSDK.framework/Headers/ -sdk iphoneos12.1`
* Update the `BugfenderBinding.cs` file if the API changed.

### Updating Android

Follow these steps:

* Download the latest version of the [Android SDK from Maven Central](http://search.maven.org/#search%7Cga%7C1%7Cbugfender). You need the `aar` file, add it to the `Binding.Android` project.
* Edit the `Properties/AssemblyInfo.cs` file to reflect the version.
* Update the `BugfenderBinding.cs` file if the API changed.

### Testing with local sample

Once a nuget has been created, you can add it to a local repo and import it from the sample application for testing:

    nuget add SDK/Bugfender.Sdk.NuGet/bin/Release/Bugfender.Sdk.x.x.x.nupkg -source ~/nugetrepo
