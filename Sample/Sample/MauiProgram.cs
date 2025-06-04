using Microsoft.Extensions.Logging;
using Bugfender.Sdk;

namespace Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Note: For Android, Bugfender initialization is done in MainApplication.cs
		// This is the cross-platform approach using the local SDK
		var bugfender = BugfenderBinding.Instance;
		bugfender.Info("App started from MauiProgram");

		return builder.Build();
	}
}

