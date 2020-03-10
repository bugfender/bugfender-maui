using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

namespace BugfenderSDK
{
	[Native]
	public enum BFLogLevel : ulong
	{
		Default = 0,
		Warning = 1,
		Error = 2,
        Trace = 3,
        Info = 4,
        Fatal = 5
	}
}