using Android.Runtime;
using Java.Interop;

namespace Com.Bugfender.Sdk
{
    public partial class Bugfender
    {
        [Register("setSDKType", "(Ljava/lang/String; I)V", "")]
        internal static unsafe void SetSdkType(string sdkType, int version)
        {
            const string __id = "setSDKType.(Ljava/lang/String; I)V";
            var nativeSdkType = JNIEnv.NewString(sdkType);
            try
            {
                JniArgumentValue* __args = stackalloc JniArgumentValue[2];
                __args[0] = new JniArgumentValue(nativeSdkType);
                __args[1] = new JniArgumentValue(version);
                _members.StaticMethods.InvokeVoidMethod(__id, __args);
            }
            finally
            {
                JNIEnv.DeleteLocalRef(nativeSdkType);
            }
        }
    }
}
