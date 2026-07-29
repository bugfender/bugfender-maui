using Bugfender.Sdk;
using System.Reflection;

namespace Sample;

public partial class MainPage : ContentPage
{
	int count = 0;
	private string _dotNetVersion;

	public MainPage()
	{
		InitializeComponent();
		
		// Get the actual .NET version once during initialization
		_dotNetVersion = GetDotNetMajorVersion();
		
		var bugfender = BugfenderBinding.Instance;
		bugfender.Info("MainPage initialized");
		
		// Initialize app info displays
		UpdateAppInfo();
		UpdateDotNetVersion();
		UpdateDeviceInfo();
	}

	private string GetDotNetMajorVersion()
	{
		var version = Environment.Version;
		return $".NET {version.Major}";
	}

	private void UpdateAppInfo()
	{
		AppInfoLabel.Text = $"🚀 Bugfender SDK {_dotNetVersion} Migration";
		AppInfoLabel.TextColor = Colors.White;
	}

	private void UpdateDotNetVersion()
	{
		var version = Environment.Version;
		var frameworkName = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
		DotNetVersionLabel.Text = $"📱 {frameworkName} v{version}";
	}

	private void UpdateDeviceInfo()
	{
		var deviceInfo = $"🔧 Platform: {DeviceInfo.Platform} | Device: {DeviceInfo.Model}";
		DeviceInfoLabel.Text = deviceInfo;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		CounterBtn.Text = $"Clicked {count} time";
		
		if (count == 1)
			CounterBtn.Text += "";
		else
			CounterBtn.Text += "s";

		var bugfender = BugfenderBinding.Instance;
		bugfender.Info($"Button clicked {count} times");

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private void OnTestLoggingClicked(object sender, EventArgs e)
	{
		var bugfender = BugfenderBinding.Instance;
		
		// Test different log levels
		bugfender.Trace("🔍 This is a TRACE level log");
		bugfender.Debug("🐛 This is a DEBUG level log");
		bugfender.Info("ℹ️ This is an INFO level log");
		bugfender.Warning("⚠️ This is a WARNING level log");
		bugfender.Error("❌ This is an ERROR level log");
		bugfender.Fatal("💀 This is a FATAL level log");

		DisplayAlert("Bugfender", "✅ Sent 6 different log levels to Bugfender!", "OK");
	}

	private void OnTestDeviceInfoClicked(object sender, EventArgs e)
	{
		var bugfender = BugfenderBinding.Instance;
		
		// Set device information
		bugfender.SetDeviceString("user_id", "test_user_12345");
		bugfender.SetDeviceString("app_version", AppInfo.VersionString);
		bugfender.SetDeviceString("build_number", AppInfo.BuildString);
		bugfender.SetDeviceString("platform", DeviceInfo.Platform.ToString());
		bugfender.SetDeviceString("device_model", DeviceInfo.Model);
		bugfender.SetDeviceString("os_version", DeviceInfo.VersionString);
		bugfender.SetDeviceInteger("click_count", count);
		bugfender.SetDeviceFloat("screen_density", (float)DeviceDisplay.Current.MainDisplayInfo.Density);

		bugfender.Info("📋 Device information has been set in Bugfender");
		DisplayAlert("Bugfender", "✅ Device information sent to Bugfender!", "OK");
	}

	private void OnTestIssueClicked(object sender, EventArgs e)
	{
		var bugfender = BugfenderBinding.Instance;
		
		string issueTitle = $"🔧 Test Issue from {_dotNetVersion} MAUI App";
		string issueMarkdown = $@"# Test Issue Report

## Details
- **App Version**: {AppInfo.VersionString}
- **Platform**: {DeviceInfo.Platform}
- **Device**: {DeviceInfo.Model}
- **OS Version**: {DeviceInfo.VersionString}
- **Button Clicks**: {count}
- **Timestamp**: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
- **Runtime**: {_dotNetVersion}

## Description
This is a test issue sent from the Bugfender {_dotNetVersion} MAUI sample app to verify issue reporting functionality.

## Steps to Reproduce
1. Open the sample app
2. Click 'Send Test Issue' button
3. Issue should appear in Bugfender dashboard";

		var issueUrl = bugfender.SendIssue(issueTitle, issueMarkdown);
		bugfender.Info($"📝 Test issue sent. URL: {issueUrl}");
		
		DisplayAlert("Bugfender", $"✅ Test issue sent!\nIssue URL: {issueUrl}", "OK");
	}

	private async void OnTestCrashClicked(object sender, EventArgs e)
	{
		var result = await DisplayAlert("⚠️ Warning", 
			"This will test crash reporting by throwing an exception. Continue?", 
			"Yes, Test Crash", "Cancel");

		if (result)
		{
			var bugfender = BugfenderBinding.Instance;
			bugfender.Warning("🧪 About to test crash reporting...");
			
			// Give a moment for the warning to be sent
			await Task.Delay(1000);
			
			// Throw a test exception
			throw new InvalidOperationException($"🧪 Test crash from Bugfender {_dotNetVersion} MAUI sample app - this is intentional for testing crash reporting!");
		}
	}

	private async void OnTestNetworkClicked(object sender, EventArgs e)
	{
		var bugfender = BugfenderBinding.Instance;
		bugfender.Info("Starting network logging test request");

		try
		{
			// Android: .NET HttpClient is NOT OkHttp — use the instrumented OkHttp helper
			var result = await Task.Run(() => bugfender.SendInstrumentedNetworkRequest(
				"https://httpbin.org/post",
				method: "POST",
				body: "{\"username\":\"demo\",\"password\":\"super-secret\"}",
				headers: new Dictionary<string, string>
				{
					["Authorization"] = "Bearer secret-token",
					["X-Demo"] = "maui-network-logging",
					["Content-Type"] = "application/json; charset=utf-8",
				}));

			bugfender.Info(
				$"Network test completed: {result.Status}, shouldCapture={result.ShouldCapture}, reqId={result.RequestId}");
			bugfender.ForceSendOnce();

			await DisplayAlert(
				"Bugfender",
				$"✅ Instrumented network request finished ({result.Status}).\n\n" +
				$"shouldCapture={result.ShouldCapture}\nreqId={result.RequestId}\n\n" +
				"Check bf_network / Network tab in the dashboard.\n" +
				"Authorization/password should be redacted by obfuscation handlers.",
				"OK");
		}
		catch (Exception ex)
		{
			bugfender.Error($"Test network request failed: {ex.Message}");
			await DisplayAlert("Bugfender", $"❌ Network request failed:\n{ex.Message}", "OK");
		}
	}
}


