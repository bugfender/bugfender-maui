using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using BugfenderSDK;

namespace BugfenderTest.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			Bugfender.EnableAllWithToken("szdGvhnVUsBYAAb8bdtnqOiWJxR4a23H");
			Bugfender.WriteLine("Bugfender ID: {0}", Bugfender.DeviceIdentifier());

			return base.FinishedLaunching(app, options);
		}
	}
}
