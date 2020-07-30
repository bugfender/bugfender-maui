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

		// +(void)setBaseURL:(NSURL * _Nonnull)url;
		[Static]
		[Export("setBaseURL:")]
		void SetBaseURL(NSUrl url);

		// +(void)activateLogger:(NSString * _Nonnull)appKey;
		[Static]
		[Export("activateLogger:")]
		void ActivateLogger(string appKey);

		// +(NSString * _Nullable)appKey;
		[Static]
		[Export("appKey")]
		string AppKey { get; }

		// +(NSUInteger)maximumLocalStorageSize;
		// +(void)setMaximumLocalStorageSize:(NSUInteger)maximumLocalStorageSize;
		[Static]
		[Export("maximumLocalStorageSize")]
		nuint MaximumLocalStorageSize { get; set; }

		// +(NSString * _Nonnull)deviceIdentifier __attribute__((deprecated("Use deviceIdentifierUrl instead.")));
		[Static]
		[Export("deviceIdentifier")]
		[Obsolete("Use DeviceIdentifierUrl instead.")]
		string DeviceIdentifier();

		// +(NSURL * _Nullable)deviceIdentifierUrl;
		[Static]
		[Export("deviceIdentifierUrl")]
		[return: NullAllowed]
		NSUrl DeviceIdentifierUrl();

		// +(NSString * _Nullable)sessionIdentifier __attribute__((deprecated("Use sessionIdentifierUrl instead.")));
		[Static]
		[Export("sessionIdentifier")]
		[Obsolete("Use SessionIdentifierUrl instead.")]
		[return: NullAllowed]
		string SessionIdentifier();

		// +(NSURL * _Nullable)sessionIdentifierUrl;
		[Static]
		[Export("sessionIdentifierUrl")]
		[return: NullAllowed]
		NSUrl SessionIdentifierUrl();

		// +(void)setForceEnabled:(BOOL)enabled;
		[Static]
		[Export("setForceEnabled:")]
		void SetForceEnabled(bool enabled);

		// +(BOOL)forceEnabled;
		[Static]
		[Export("forceEnabled")]
		bool ForceEnabled();

		// +(void)setPrintToConsole:(BOOL)enabled;
		[Static]
		[Export("setPrintToConsole:")]
		void SetPrintToConsole(bool enabled);

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

		// +(void)overrideDeviceName:(NSString * _Nonnull)deviceName;
		[Static]
		[Export("overrideDeviceName:")]
		void OverrideDeviceName(string deviceName);

		// +(void)setDeviceBOOL:(BOOL)b forKey:(NSString * _Nonnull)key;
		[Static]
		[Export("setDeviceBOOL:forKey:")]
		void SetDeviceBOOL(bool value, string key);

		// +(void)setDeviceString:(NSString * _Nonnull)s forKey:(NSString * _Nonnull)key;
		[Static]
		[Export("setDeviceString:forKey:")]
		void SetDeviceString(string value, string key);

		// +(void)setDeviceInteger:(UInt64)i forKey:(NSString * _Nonnull)key;
		[Static]
		[Export("setDeviceInteger:forKey:")]
		void SetDeviceInteger(ulong value, string key);

		// +(void)setDeviceDouble:(double)d forKey:(NSString * _Nonnull)key;
		[Static]
		[Export("setDeviceDouble:forKey:")]
		void SetDeviceDouble(double value, string key);

		// +(void)removeDeviceKey:(NSString * _Nonnull)key;
		[Static]
		[Export("removeDeviceKey:")]
		void RemoveDeviceKey(string key);

		// +(void)logWithLineNumber:(NSInteger)lineNumber method:(NSString * _Nonnull)method file:(NSString * _Nonnull)file level:(BFLogLevel)level tag:(NSString * _Nullable)tag message:(NSString * _Nonnull)message __attribute__((swift_name("log(lineNumber:method:file:level:tag:message:)")));
		[Static]
		[Export("logWithLineNumber:method:file:level:tag:message:")]
		void Log(nint lineNumber, string method, string file, BFLogLevel level, [NullAllowed] string tag, string message);

		// +(void)forceSendOnce;
		[Static]
		[Export("forceSendOnce")]
		void ForceSendOnce();

		// +(NSString * _Nullable)sendIssueWithTitle:(NSString * _Nonnull)title text:(NSString * _Nonnull)text __attribute__((deprecated("Use sendIssueReturningUrlWithTitle:text: instead.")));
		[Static]
		[Export("sendIssueWithTitle:text:")]
		[Obsolete("Use SendIssueReturningUrl instead.")]
		[return: NullAllowed]
		string SendIssue(string title, string text);

		// +(NSURL * _Nullable)sendIssueReturningUrlWithTitle:(NSString * _Nonnull)title text:(NSString * _Nonnull)text;
		[Static]
		[Export("sendIssueReturningUrlWithTitle:text:")]
		[return: NullAllowed]
		NSUrl SendIssueReturningUrl(string title, string text);

		// +(NSURL * _Nullable)sendCrashWithTitle:(NSString * _Nonnull)title text:(NSString * _Nonnull)text;
		[Static]
		[Export("sendCrashWithTitle:text:")]
		[return: NullAllowed]
		NSUrl SendCrash(string title, string text);

		// +(BFUserFeedbackNavigationController * _Nonnull)userFeedbackViewControllerWithTitle:(NSString * _Nonnull)title hint:(NSString * _Nonnull)hint subjectPlaceholder:(NSString * _Nonnull)subjectPlaceholder messagePlaceholder:(NSString * _Nonnull)messagePlaceholder sendButtonTitle:(NSString * _Nonnull)sendButtonTitle cancelButtonTitle:(NSString * _Nonnull)cancelButtonTitle completion:(void (^ _Nullable)(BOOL, NSURL * _Nullable))completionBlock;
		[Static]
		[Export("userFeedbackViewControllerWithTitle:hint:subjectPlaceholder:messagePlaceholder:sendButtonTitle:cancelButtonTitle:completion:")]
		UIViewController UserFeedbackViewController(string title, string hint, string subjectPlaceholder, string messagePlaceholder, string sendButtonTitle, string cancelButtonTitle, [NullAllowed] Action<bool, NSUrl> completionBlock);

		// +(void)sendUserFeedbackWithSubject:(NSString * _Nonnull)subject message:(NSString * _Nonnull)message __attribute__((deprecated("Use sendUserFeedbackReturningUrlWithSubject:message: instead.")));
		[Static]
		[Export("sendUserFeedbackWithSubject:message:")]
		[Obsolete("Use SendUserFeedbackReturningUrl instead.")]
		void SendUserFeedback(string subject, string message);

		// +(NSURL * _Nullable)sendUserFeedbackReturningUrlWithSubject:(NSString * _Nonnull)subject message:(NSString * _Nonnull)message;
		[Static]
		[Export("sendUserFeedbackReturningUrlWithSubject:message:")]
		[return: NullAllowed]
		NSUrl SendUserFeedbackReturningUrl(string subject, string message);
	}
}
