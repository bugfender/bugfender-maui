# Bugfender bindings for Xamarin

This repository provides bindings for iOS and Android on Xamarin projects. Also provides a sample solution that shows the integration.

In order to use Bugfender, you will need an account which you can [create here](https://bugfender.com).

## Usage

### Downloading this project
You will need the files in this project, as first step you should [download it](https://github.com/bugfender/bugfender-xamarin/archive/master.zip).

### Integrating bindings on an iOS project

* Right click on *Your Solution* > **Add Existing Project...** > Select the `BugfenderBinding.iOS` project you just downloaded.
* Right click on *Your Project* > Double click on **References** > Then add the `BugfenderBinding.iOS` project.
* Edit `AppDelegate.cs` and initialize Bugfender in the `FinishedLaunching` method, like this:

```
using BugfenderSDK;

// ...

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			Bugfender.ActivateLogger("YOUR APP KEY");
			Bugfender.EnableUIEventLogging();
			Bugfender.WriteLine("Bugfender ID: {0}", Bugfender.DeviceIdentifier());
			
			return true;
		}

```
* Replace *YOUR APP KEY* with your Bugfender app key.
* For more information, have a look at the [iOS SDK reference](http://cocoadocs.org/docsets/BugfenderSDK/).

**Note:** Xamarin Studio might highlight your code saying "The name Bugfender does not exist in the current context". This is a bug in the IDE, your code will compile fine.

### Integrating bindings on an Android project

* Right click on *Your Solution* > **Add Existing Project...** > Select the `BugfenderBinding.Droid` project you just downloaded.
* Right click on *Your Project* > Double click on **References** > Then add the `BugfenderBinding.Droid` project.
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
		Bugfender.Init(this.ApplicationContext, "YOUR APP KEY", true);
		Bugfender.EnableUIEventLogging(this);
		Bugfender.EnableLogcatLogging();

		Bugfender.D("TAG", "Hello, testing!");
		Bugfender.W("TAG", "Hello, warning!");
		Bugfender.E("TAG", Bugfender.DeviceIdentifier);
	}
}
```
* Replace *YOUR APP KEY* with your Bugfender app key.
* For more information, have a look at the [Android SDK reference](http://www.javadoc.io/doc/com.bugfender.sdk/android).

## Updating

This repository contains the Bugfender iOS and Android SDKs, which can be updated anytime and maybe are not updated here. At the moment of writing this, the SDKs used are:

* Android 0.6.2
* iOS 0.3.26


### Updating iOS

Follow these steps for updating:

* Download the latest version of the [iOS SDK from GitHub](https://github.com/bugfender/BugfenderSDK-iOS).
* Rename `BugfenderSDK.framework/BugfenderSDK` to `libBugfenderSDK.a` and copy it to `BugfenderBinding.iOS`
* Update `BugfenderBinding.iOS/ApiDefinition.cs` by using [Objective Sharpie](https://developer.xamarin.com/guides/cross-platform/macios/binding/objective-sharpie/). Manually check which are the methods updated and merge them.

### Updating Android

Follow these steps:

* Download the latest version of the [Android SDK from Maven Central](http://search.maven.org/#search%7Cga%7C1%7Cbugfender). You need the `aar` file.
* Rename the file to `sdk-release.aar`.
* Replace the exising `aar` file in `BugfenderBinding.Droid/Jars` with the recently downloaded file.


## Known limitations / To Do

Xamarin integration could be better, here are a few things that can be done:

* If using `Console` to write logs in your project, you will not get those logs in Bugfender. This is because logs are not being sent to Logcat/NSLog and thus Bugfender can not intercept them. The only solution to that is to call `Bugfender` class directly for logging.
* If using `Trace`/`Debug` to write logs in your project, you will not get those logs in Bugfender. This is because logs are supressed from the application output when compiled for Release.  The only solution to that is to call `Bugfender` class directly for logging or maybe writing a `TraceListener` class.
* Provide a uniform interface between iOS and Android so that Bugfender can be used the same regardless of the platform. Ideally, provide a way that (platform-independent) shared projects can use it.
* Publish as a NuGet to make integration easier.
* Publish to Xamarin Components.

If you feel like contributing in any of these areas please [contact us](https://bugfender.com/contact) or submit pull-requests and we will love you for that!
