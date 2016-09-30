using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Com.Bugfender.Sdk;

namespace BugfenderTest.Droid
{
	[Activity(Label = "BugfenderTest.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			Bugfender.Init(this.ApplicationContext, "szdGvhnVUsBYAAb8bdtnqOiWJxR4a23H", true);
			Bugfender.D("TAG", "Hello, testing!");
			Bugfender.W("TAG", "Hello, warning!");
			Bugfender.E("TAG", Bugfender.DeviceIdentifier);

			LoadApplication(new App());
		}
	}
}
