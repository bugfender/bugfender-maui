using System;
using BugfenderSDK;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace BugfenderSDK
{
	// @interface BFUserFeedbackViewController : UITableViewController
	[BaseType (typeof(UITableViewController))]
	interface BFUserFeedbackViewController
	{
		// @property (nonatomic, strong) UIColor * _Nonnull mainBackgroundColor;
		[Export ("mainBackgroundColor", ArgumentSemantic.Strong)]
		UIColor MainBackgroundColor { get; set; }

		// @property (nonatomic, strong) UIColor * _Nonnull secondaryBackgroundColor;
		[Export ("secondaryBackgroundColor", ArgumentSemantic.Strong)]
		UIColor SecondaryBackgroundColor { get; set; }

		// @property (nonatomic, strong) NSString * _Nonnull hint;
		[Export ("hint", ArgumentSemantic.Strong)]
		string Hint { get; set; }

		// @property (nonatomic, strong) UIFont * _Nonnull hintFont;
		[Export ("hintFont", ArgumentSemantic.Strong)]
		UIFont HintFont { get; set; }

		// @property (nonatomic, strong) UIColor * _Nonnull hintFontColor;
		[Export ("hintFontColor", ArgumentSemantic.Strong)]
		UIColor HintFontColor { get; set; }

		// @property (nonatomic, strong) UIFont * _Nonnull subjectFont;
		[Export ("subjectFont", ArgumentSemantic.Strong)]
		UIFont SubjectFont { get; set; }

		// @property (nonatomic, strong) UIColor * _Nonnull subjectFontColor;
		[Export ("subjectFontColor", ArgumentSemantic.Strong)]
		UIColor SubjectFontColor { get; set; }

		// @property (nonatomic, strong) UIColor * _Nonnull subjectPlaceholderFontColor;
		[Export ("subjectPlaceholderFontColor", ArgumentSemantic.Strong)]
		UIColor SubjectPlaceholderFontColor { get; set; }

		// @property (nonatomic, strong) NSString * _Nonnull subjectPlaceholder;
		[Export ("subjectPlaceholder", ArgumentSemantic.Strong)]
		string SubjectPlaceholder { get; set; }

		// @property (nonatomic, strong) UIFont * _Nonnull messageFont;
		[Export ("messageFont", ArgumentSemantic.Strong)]
		UIFont MessageFont { get; set; }

		// @property (nonatomic, strong) UIColor * _Nonnull messageFontColor;
		[Export ("messageFontColor", ArgumentSemantic.Strong)]
		UIColor MessageFontColor { get; set; }

		// @property (nonatomic, strong) UIColor * _Nonnull messagePlaceholderFontColor;
		[Export ("messagePlaceholderFontColor", ArgumentSemantic.Strong)]
		UIColor MessagePlaceholderFontColor { get; set; }

		// @property (nonatomic, strong) NSString * _Nonnull messagePlaceholder;
		[Export ("messagePlaceholder", ArgumentSemantic.Strong)]
		string MessagePlaceholder { get; set; }

		// -(void)dismiss;
		[Export ("dismiss")]
		void Dismiss ();

		// -(void)sendFeedback;
		[Export ("sendFeedback")]
		void SendFeedback ();

		// @property (copy, nonatomic) void (^ _Nonnull)(BOOL, NSURL * _Nullable) completionBlock;
		[Export ("completionBlock", ArgumentSemantic.Copy)]
		Action<bool, NSUrl> CompletionBlock { get; set; }
	}

	// @interface BFUserFeedbackNavigationController : UINavigationController
	[BaseType (typeof(UINavigationController))]
	interface BFUserFeedbackNavigationController
	{
		// @property (nonatomic, strong) BFUserFeedbackViewController * _Nonnull feedbackViewController;
		[Export ("feedbackViewController", ArgumentSemantic.Strong)]
		BFUserFeedbackViewController FeedbackViewController { get; set; }

		// +(BFUserFeedbackNavigationController * _Nonnull)userFeedbackViewControllerWithTitle:(NSString * _Nonnull)title hint:(NSString * _Nonnull)hint subjectPlaceholder:(NSString * _Nonnull)subjectPlaceholder messagePlaceholder:(NSString * _Nonnull)messagePlaceholder sendButtonTitle:(NSString * _Nonnull)sendButtonTitle cancelButtonTitle:(NSString * _Nonnull)cancelButtonTitle completion:(void (^ _Nonnull)(BOOL, NSURL * _Nullable))completionBlock;
		[Static]
		[Export ("userFeedbackViewControllerWithTitle:hint:subjectPlaceholder:messagePlaceholder:sendButtonTitle:cancelButtonTitle:completion:")]
		BFUserFeedbackNavigationController UserFeedbackViewControllerWithTitle (string title, string hint, string subjectPlaceholder, string messagePlaceholder, string sendButtonTitle, string cancelButtonTitle, Action<bool, NSUrl> completionBlock);
	}

	[Static]
	//[Verify (ConstantsInterfaceAssociation)]
	partial interface Constants
	{
		// extern const double BFLibraryVersionNumber;
		[Field ("BFLibraryVersionNumber", "__Internal")]
		double BFLibraryVersionNumber { get; }
	}

	// @interface Bugfender : NSObject
	[BaseType (typeof(NSObject))]
	interface Bugfender
	{
		// +(void)setApiURL:(NSURL * _Nonnull)url;
		[Static]
		[Export ("setApiURL:")]
		void SetApiURL (NSUrl url);

		// +(void)setBaseURL:(NSURL * _Nonnull)url;
		[Static]
		[Export ("setBaseURL:")]
		void SetBaseURL (NSUrl url);

		// +(void)setSDKType:(NSString * _Nonnull)sdkType;
		[Static]
		[Export ("setSDKType:")]
		void SetSDKType (string sdkType);

		// +(void)activateLogger:(NSString * _Nonnull)appKey;
		[Static]
		[Export ("activateLogger:")]
		void ActivateLogger (string appKey);

		// +(NSString * _Nullable)appKey;
		[Static]
		[NullAllowed, Export ("appKey")]
		//[Verify (MethodToProperty)]
		string AppKey { get; }

		// +(NSUInteger)maximumLocalStorageSize;
		// +(void)setMaximumLocalStorageSize:(NSUInteger)maximumLocalStorageSize;
		[Static]
		[Export ("maximumLocalStorageSize")]
		//[Verify (MethodToProperty)]
		nuint MaximumLocalStorageSize { get; set; }

		// +(NSString * _Nonnull)deviceIdentifier __attribute__((deprecated("Use deviceIdentifierUrl instead.")));
		[Static]
		[Export ("deviceIdentifier")]
		string DeviceIdentifier ();

		// +(NSURL * _Nullable)deviceIdentifierUrl;
		[Static]
		[Export ("deviceIdentifierUrl")]
		[return: NullAllowed]
		NSUrl DeviceIdentifierUrl ();

		// +(NSString * _Nullable)sessionIdentifier __attribute__((deprecated("Use sessionIdentifierUrl instead.")));
		[Static]
		[Export ("sessionIdentifier")]
		[return: NullAllowed]
		string SessionIdentifier ();

		// +(NSURL * _Nullable)sessionIdentifierUrl;
		[Static]
		[Export ("sessionIdentifierUrl")]
		[return: NullAllowed]
		NSUrl SessionIdentifierUrl ();

		// +(void)setForceEnabled:(BOOL)enabled;
		[Static]
		[Export ("setForceEnabled:")]
		void SetForceEnabled (bool enabled);

		// +(BOOL)forceEnabled;
		[Static]
		[Export ("forceEnabled")]
		bool ForceEnabled ();

		// +(void)setPrintToConsole:(BOOL)enabled;
		[Static]
		[Export ("setPrintToConsole:")]
		void SetPrintToConsole (bool enabled);

		// +(BOOL)printToConsole;
		[Static]
		[Export ("printToConsole")]
		bool PrintToConsole ();

		// +(void)enableUIEventLogging;
		[Static]
		[Export ("enableUIEventLogging")]
		void EnableUIEventLogging ();

		// +(void)enableUIEventLoggingWithIgnoredViewsTags:(NSArray<NSNumber *> * _Nonnull)ignoredViewsTags;
		[Static]
		[Export ("enableUIEventLoggingWithIgnoredViewsTags:")]
		void EnableUIEventLoggingWithIgnoredViewsTags (NSNumber[] ignoredViewsTags);

		// +(void)enableCrashReporting;
		[Static]
		[Export ("enableCrashReporting")]
		void EnableCrashReporting ();

		// +(void)overrideDeviceName:(NSString * _Nonnull)deviceName;
		[Static]
		[Export ("overrideDeviceName:")]
		void OverrideDeviceName (string deviceName);

		// +(void)setDeviceBOOL:(BOOL)b forKey:(NSString * _Nonnull)key;
		[Static]
		[Export ("setDeviceBOOL:forKey:")]
		void SetDeviceBOOL (bool b, string key);

		// +(void)setDeviceString:(NSString * _Nonnull)s forKey:(NSString * _Nonnull)key;
		[Static]
		[Export ("setDeviceString:forKey:")]
		void SetDeviceString (string s, string key);

		// +(void)setDeviceInteger:(UInt64)i forKey:(NSString * _Nonnull)key;
		[Static]
		[Export ("setDeviceInteger:forKey:")]
		void SetDeviceInteger (ulong i, string key);

		// +(void)setDeviceDouble:(double)d forKey:(NSString * _Nonnull)key;
		[Static]
		[Export ("setDeviceDouble:forKey:")]
		void SetDeviceDouble (double d, string key);

		// +(void)removeDeviceKey:(NSString * _Nonnull)key;
		[Static]
		[Export ("removeDeviceKey:")]
		void RemoveDeviceKey (string key);

		// +(void)logWithLineNumber:(NSInteger)lineNumber method:(NSString * _Nonnull)method file:(NSString * _Nonnull)file level:(BFLogLevel)level tag:(NSString * _Nullable)tag message:(NSString * _Nonnull)message __attribute__((swift_name("log(lineNumber:method:file:level:tag:message:)")));
		[Static]
		[Export ("logWithLineNumber:method:file:level:tag:message:")]
		void LogWithLineNumber (nint lineNumber, string method, string file, BFLogLevel level, [NullAllowed] string tag, string message);

		// +(void)forceSendOnce;
		[Static]
		[Export ("forceSendOnce")]
		void ForceSendOnce ();

		// +(NSString * _Nullable)sendIssueWithTitle:(NSString * _Nonnull)title text:(NSString * _Nonnull)text __attribute__((deprecated("Use sendIssueReturningUrlWithTitle:text: instead.")));
		[Static]
		[Export ("sendIssueWithTitle:text:")]
		[return: NullAllowed]
		string SendIssueWithTitle (string title, string text);

		// +(NSURL * _Nullable)sendIssueReturningUrlWithTitle:(NSString * _Nonnull)title text:(NSString * _Nonnull)text;
		[Static]
		[Export ("sendIssueReturningUrlWithTitle:text:")]
		[return: NullAllowed]
		NSUrl SendIssueReturningUrlWithTitle (string title, string text);

		// +(NSURL * _Nullable)sendCrashWithTitle:(NSString * _Nonnull)title text:(NSString * _Nonnull)text;
		[Static]
		[Export ("sendCrashWithTitle:text:")]
		[return: NullAllowed]
		NSUrl SendCrashWithTitle (string title, string text);

		// +(BFUserFeedbackNavigationController * _Nonnull)userFeedbackViewControllerWithTitle:(NSString * _Nonnull)title hint:(NSString * _Nonnull)hint subjectPlaceholder:(NSString * _Nonnull)subjectPlaceholder messagePlaceholder:(NSString * _Nonnull)messagePlaceholder sendButtonTitle:(NSString * _Nonnull)sendButtonTitle cancelButtonTitle:(NSString * _Nonnull)cancelButtonTitle completion:(void (^ _Nullable)(BOOL, NSURL * _Nullable))completionBlock;
		[Static]
		[Export ("userFeedbackViewControllerWithTitle:hint:subjectPlaceholder:messagePlaceholder:sendButtonTitle:cancelButtonTitle:completion:")]
		BFUserFeedbackNavigationController UserFeedbackViewControllerWithTitle (string title, string hint, string subjectPlaceholder, string messagePlaceholder, string sendButtonTitle, string cancelButtonTitle, [NullAllowed] Action<bool, NSUrl> completionBlock);

		// +(void)sendUserFeedbackWithSubject:(NSString * _Nonnull)subject message:(NSString * _Nonnull)message __attribute__((deprecated("Use sendUserFeedbackReturningUrlWithSubject:message: instead.")));
		[Static]
		[Export ("sendUserFeedbackWithSubject:message:")]
		void SendUserFeedbackWithSubject (string subject, string message);

		// +(NSURL * _Nullable)sendUserFeedbackReturningUrlWithSubject:(NSString * _Nonnull)subject message:(NSString * _Nonnull)message;
		[Static]
		[Export ("sendUserFeedbackReturningUrlWithSubject:message:")]
		[return: NullAllowed]
		NSUrl SendUserFeedbackReturningUrlWithSubject (string subject, string message);
	}
}
