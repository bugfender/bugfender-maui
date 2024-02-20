namespace Bugfender.Sdk
{
    public struct BugfenderOptions
    {
        public string appKey;
        public Uri? apiUri;
        public Uri? baseUri;
        public uint? maximumLocalStorageSize;
        public bool printToConsole;
        public bool nativeCrashReporting;
        public bool mauiCrashReporting;
        public bool logUIEvents;
    }

    public interface IBugfenderBinding
    {
        Uri DeviceUri { get; }
        Uri SessionUri { get; }
        bool ForceEnabled { set; }
        void SetDeviceString(string key, string value);
        void SetDeviceInteger(string key, int value);
        void SetDeviceFloat(string key, float value);
        void RemoveDeviceKey(string key);
        void Log(int lineNumber, string method, string file, LogLevel logLevel, string tag, string message);
        void ForceSendOnce();
        Uri SendIssue(string title, string markdown);
        Uri SendCrash(string title, string text);
        Uri SendUserFeedback(string subject, string message);
    }
}
