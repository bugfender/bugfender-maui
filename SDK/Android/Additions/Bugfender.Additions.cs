using Android.Runtime;
using Java.Interop;

namespace Com.Bugfender.Sdk
{
    public partial class Bugfender
    {
        [Register("setSDKType", "(Ljava/lang/String;)V", "")]
        internal static unsafe void SetSdkType(string sdkType)
        {
            const string __id = "setSDKType.(Ljava/lang/String;)V";
            var nativeSdkType = JNIEnv.NewString(sdkType);
            try
            {
                JniArgumentValue* __args = stackalloc JniArgumentValue[1];
                __args[0] = new JniArgumentValue(nativeSdkType);
                _members.StaticMethods.InvokeVoidMethod(__id, __args);
            }
            finally
            {
                JNIEnv.DeleteLocalRef(nativeSdkType);
            }
        }
    }
}
