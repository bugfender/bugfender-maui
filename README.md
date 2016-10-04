# Bugfender bindings for Xamarin

This repository provides bindings for iOS and Android on Xamarin projects. Also provides a sample solution that shows the integration.

## Integrating bindings on an iOS project

* Right click on *Your Solution* > **Add Existing Project...** > Select the `BugfenderBinding.iOS` project you just downloaded.
* Right click on *Your Project* > Double click on **References** > Then add the `BugfenderBinding.iOS` project.
* Edit `AppDelegate.cs` and initialize Bugfender in the `FinishedLaunching` method, like this:

```
using BugfenderSDK;

// ...

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			Bugfender.EnableAllWithToken("YOUR APP KEY");
			Bugfender.WriteLine("Bugfender ID: {0}", Bugfender.DeviceIdentifier());
			
			return true;
		}

```
* Replace *YOUR APP KEY* with your Bugfender app key.
* For more information, have a look at the [iOS SDK reference](http://cocoadocs.org/docsets/BugfenderSDK/).


## Integrating bindings on an Android project

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