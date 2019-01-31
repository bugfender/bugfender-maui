using System;

using UIKit;
using BugfenderSDK;
using Foundation;

namespace XamarinSample.iOS
{
	public partial class ViewController : UIViewController
	{
        public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
        }

        partial void LogButton_TouchUpInside(UIButton sender)
        {
            Bugfender.WriteLine("Hello world");
        }

        partial void CrashButton_TouchUpInside(UIButton sender)
        {
            throw new Exception("test");
        }

        partial void UserFeedbackButton_TouchUpInside(UIButton sender)
        {
            UIViewController vc = Bugfender.UserFeedbackViewController("Send Feedback", "Hint", "This is the subject", "this is the message", "Send", "Cancel", HandleAction);
            this.ShowViewController(vc, sender);
        }

        void HandleAction(bool sent)
        {
            // do something with the sent parameter
        }

    }
}
