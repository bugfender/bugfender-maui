using Android.App;
using Android.Widget;
using Android.OS;
using Com.Bugfender.Sdk;
using System;
using Android.Content;
using Android.Runtime;

namespace XamarinSample.Droid
{
	[Activity(Label = "XamarinSample", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;
        int FeedbackActivityRequestCode = 1337;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);
			button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

            Button helloBugfenderButton = FindViewById<Button>(Resource.Id.helloBugfenderButton);
            helloBugfenderButton.Click += delegate { Bugfender.D("hello_tag", "Hello world!"); };

            Button sendFeedbackButton = FindViewById<Button>(Resource.Id.sendFeedbackButton);
            sendFeedbackButton.Click += delegate
            {
                var intent = Bugfender.GetUserFeedbackActivityIntent(this, "Feedback Title", "Instructions", "Subject", "Message", "Send");
                this.StartActivityForResult(intent, FeedbackActivityRequestCode);
            };

            Button crashButton = FindViewById<Button>(Resource.Id.crashButton);
            crashButton.Click += delegate { throw new Exception("test"); };
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if(requestCode == FeedbackActivityRequestCode)
            {
                bool feedbackSent = resultCode == Result.Ok;
                //TODO: do something with feedbackSent
                Bugfender.T("TAG", "Feedback sent: " + feedbackSent);
            }
            else
            {
                base.OnActivityResult(requestCode, resultCode, data);
            }
        }
    }
}
