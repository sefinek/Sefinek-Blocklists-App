using SefinekBlocklistsApp.Properties;
using SefinekBlocklistsApp.Scripts;

namespace SefinekBlocklistsApp;

internal static class Program
{
	/// <summary>
	///    The main entry point for the application.
	/// </summary>
	[STAThread]
	private static void Main()
	{
		InitializeApplication();

		try
		{
			Application.Run(new MainWindow { Icon = Resources.icon });
		}
		catch (Exception ex)
		{
			Utils.ShowErrorMessage($"Sorry, but something went wrong.\n\n{ex.Message}");
		}
	}

	private static void InitializeApplication()
	{
		ApplicationConfiguration.Initialize();
		Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
	}
}
