using Bugfender.Sdk;

namespace Sample;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		var bugfender = BugfenderBinding.Instance;
		bugfender.Info("MainPage initialized");
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
}


