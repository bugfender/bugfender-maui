using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

[Native]
public enum BFLogLevel : ulong
{
	Default = 0,
	Warning = 1,
	Error = 2
}
