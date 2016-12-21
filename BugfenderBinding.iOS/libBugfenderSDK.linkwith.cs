using System;
using ObjCRuntime;

[assembly: LinkWith ("libBugfenderSDK.a", LinkTarget.ArmV7 | LinkTarget.Simulator, ForceLoad = true)]
