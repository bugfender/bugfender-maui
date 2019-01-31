using System;
using Foundation;

namespace BugfenderSDK
{
	// @interface Bugfender : NSObject
	[BaseType(typeof(NSObject))]
	interface Bugfender
	{
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

		// +(void)enableUIEventLogging;
		[Static]
		[Export("enableUIEventLogging")]
		void EnableUIEventLogging();

        // +(void)enableCrashReporting;
        [Static]
        [Export("enableCrashReporting")]
        void EnableCrashReporting();

		// +(void)sendIssueWithTitle:(NSString *)title text:(NSString *)text;
		[Static]
		[Export("sendIssueWithTitle:text:")]
		void SendIssueWithTitle(string title, string text);

		// +(void)setDeviceBOOL:(BOOL)b forKey:(NSString *)key;
		[Static]
		[Export("setDeviceBOOL:forKey:")]
		void SetDeviceBOOL(bool b, string key);

		// +(void)setDeviceString:(NSString *)s forKey:(NSString *)key;
		[Static]
		[Export("setDeviceString:forKey:")]
		void SetDeviceString(string s, string key);

		// +(void)setDeviceInteger:(UInt64)i forKey:(NSString *)key;
		[Static]
		[Export("setDeviceInteger:forKey:")]
		void SetDeviceInteger(ulong i, string key);

		// +(void)setDeviceDouble:(double)d forKey:(NSString *)key;
		[Static]
		[Export("setDeviceDouble:forKey:")]
		void SetDeviceDouble(double d, string key);

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
	}
}