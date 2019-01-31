// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace XamarinSample.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UIButton Button { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CrashButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LogButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UserFeedbackButton { get; set; }

        [Action ("CrashButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CrashButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("LogButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LogButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("UserFeedbackButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UserFeedbackButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CrashButton != null) {
                CrashButton.Dispose ();
                CrashButton = null;
            }

            if (LogButton != null) {
                LogButton.Dispose ();
                LogButton = null;
            }

            if (UserFeedbackButton != null) {
                UserFeedbackButton.Dispose ();
                UserFeedbackButton = null;
            }
        }
    }
}