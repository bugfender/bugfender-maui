using System;
using Foundation;
using UIKit;

namespace BugfenderSDK
{
	// @interface Bugfender : NSObject
	[BaseType(typeof(NSObject))]
	interface Bugfender
	{
        // +(void)setApiURL:(NSURL * _Nonnull)url;
        [Static]
        [Export("setApiURL:")]
        void SetApiURL(NSUrl url);

        // +(void)activateLogger:(NSString *)appToken;
        [Static]
		[Export("activateLogger:")]
		void ActivateLogger(string appToken);

		// +(NSString *)token;
		[Static]
		[Export("appKey")]
		string AppKey { get; }

		// +(NSUInteger)maximumLocalStorageSize;
		// +(void)setMaximumLocalStorageSize:(NSUInteger)maximumLocalStorageSize;
		[Static]
		[Export("maximumLocalStorageSize")]
		nuint MaximumLocalStorageSize { get; set; }

		// +(NSString *)deviceIdentifier;
		[Static]
		[Export("deviceIdentifier")]
		string DeviceIdentifier();

        // +(NSString *)deviceIdentifier;
        [Static]
        [Export("sessionIdentifier")]
        string SessionIdentifier();

		// +(void)setForceEnabled:(BOOL)enabled;
		[Static]
		[Export("setForceEnabled:")]
		void SetForceEnabled(bool enabled);

		// +(BOOL)forceEnabled;
		[Static]
		[Export("forceEnabled")]
		bool ForceEnabled();

        // +(BOOL)printToConsole;
        [Static]
        [Export("printToConsole")]
        bool PrintToConsole();

        // +(void)enableUIEventLogging;
        [Static]
		[Export("enableUIEventLogging")]
		void EnableUIEventLogging();

        // +(void)enableCrashReporting;
        [Static]
        [Export("enableCrashReporting")]
        void EnableCrashReporting();

		// +(void)setDeviceBOOL:(BOOL)b forKey:(NSString *)key;
		[Static]
		[Export("setDeviceBOOL:forKey:")]
		void SetDeviceBOOL(bool value, string key);

		// +(void)setDeviceString:(NSString *)s forKey:(NSString *)key;
		[Static]
		[Export("setDeviceString:forKey:")]
		void SetDeviceString(string value, string key);

		// +(void)setDeviceInteger:(UInt64)i forKey:(NSString *)key;
		[Static]
		[Export("setDeviceInteger:forKey:")]
		void SetDeviceInteger(ulong value, string key);

		// +(void)setDeviceDouble:(double)d forKey:(NSString *)key;
		[Static]
		[Export("setDeviceDouble:forKey:")]
		void SetDeviceDouble(double value, string key);

		// +(void)removeDeviceKey:(NSString *)key;
		[Static]
		[Export("removeDeviceKey:")]
		void RemoveDeviceKey(string key);

		// +(void)logWithLineNumber:(NSInteger)lineNumber method:(NSString *)method file:(NSString *)file level:(BFLogLevel)level tag:(NSString *)tag message:(NSString *)message;
		[Static]
		[Export("logWithLineNumber:method:file:level:tag:message:")]
		void Log(nint lineNumber, string method, string file, BFLogLevel level, string tag, string message);

		// +(void)forceSendOnce;
		[Static]
		[Export("forceSendOnce")]
		void ForceSendOnce();

        // +(void)sendIssueWithTitle:(NSString *)title text:(NSString *)text;
        [Static]
        [Export("sendIssueWithTitle:text:")]
        void SendIssueWithTitle(string title, string text);

        // +(id)userFeedbackViewControllerWithTitle:(NSString * _Nonnull)title hint:(NSString * _Nonnull)hint subjectPlaceholder:(NSString * _Nonnull)subjectPlaceholder messagePlaceholder:(NSString * _Nonnull)messagePlaceholder sendButtonTitle:(NSString * _Nonnull)sendButtonTitle cancelButtonTitle:(NSString * _Nonnull)cancelButtonTitle completion:(void (^ _Nullable)(BOOL))completionBlock;
        [Static]
        [Export("userFeedbackViewControllerWithTitle:hint:subjectPlaceholder:messagePlaceholder:sendButtonTitle:cancelButtonTitle:completion:")]
        UIViewController UserFeedbackViewController(string title, string hint, string subjectPlaceholder, string messagePlaceholder, string sendButtonTitle, string cancelButtonTitle, [NullAllowed] Action<bool> completionBlock);

        // +(void)sendUserFeedbackWithSubject:(NSString * _Nonnull)subject message:(NSString * _Nonnull)message;
        [Static]
        [Export("sendUserFeedbackWithSubject:message:")]
        void SendUserFeedbackWithSubject(string subject, string message);
    }
}