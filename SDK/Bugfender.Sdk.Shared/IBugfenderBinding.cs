using System;

namespace Bugfender.Sdk
{
    public interface IBugfenderBinding
    {
        void ActivateLogger(string appToken, bool printToConsole);
        void EnableXamarinCrashReporting();
        void SetApiUri(Uri uri);
        void SetBaseUri(Uri uri);
        UInt32 MaximumLocalStorageSize { set; }
        Uri DeviceUri { get; }
        Uri SessionUri { get; }
        bool ForceEnabled { set; }
        void SetDeviceString(string key, string value);
        void SetDeviceInteger(string key, int value);
        void SetDeviceFloat(string key, float value);
        void RemoveDeviceKey(string key);
        void Log(int lineNumber, String method, String file, LogLevel logLevel, String tag, String message);
        void ForceSendOnce();
        Uri SendIssue(string title, string markdown);
        Uri SendCrash(string title, string text);
        Uri SendUserFeedback(string subject, string message);
    }
}
