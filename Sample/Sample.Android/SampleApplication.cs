using System;
using Android.App;
using Android.Runtime;

using Bugfender.Sdk;

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
