using System;
using Android.Runtime;

namespace Com.Bugfender.Sdk
{
    // These partial classes provide the missing `A(java.io.File)` implementation
    // required by the obfuscated `O` (exposed as `IO`) interface for classes M
    // and R0. The real behaviour lives inside the native library, therefore we
    // can simply forward to the already-present `B(java.io.File)` method which
    // returns a Java object.

    public partial class M
    {
        public virtual Java.Lang.Object? A(Java.IO.File? p0)
        {
            return B(p0);
        }
    }

    public partial class R0
    {
        public virtual Java.Lang.Object? A(Java.IO.File? p0)
        {
            return B(p0);
        }
    }
} 
