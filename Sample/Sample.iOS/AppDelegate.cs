using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Bugfender.Sdk;

namespace Sample.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            BugfenderBinding bugfender = BugfenderBinding.Instance;
            bugfender.ActivateLogger("YOUR APP KEY", true); // true == enable logging to console
            bugfender.EnableUIEventLogging();
            bugfender.EnableXamarinCrashReporting();
            bugfender.WriteLine("Logs for this device are here: {0}", bugfender.DeviceUri.ToString());
            bugfender.Warning("TAG", "This is a warning");
            bugfender.Error("TAG", "This is an error!");

            return base.FinishedLaunching(app, options);
        }
    }
}
