using System.Diagnostics;
using System.Reflection;
using SefinekBlocklists.Properties;
using SefinekBlocklists.Scripts;

namespace SefinekBlocklists;

internal static class Program
{
	public static readonly string? AppFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

	/// <summary>
	///     The main entry point for the application.
	/// </summary>
	[STAThread]
	private static void Main()
	{
		Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
		ApplicationConfiguration.Initialize();

		try
		{
			Application.Run(new MainWindow { Icon = Resources.icon });
		}
		catch (Exception ex)
		{
			Utils.ShowErrorMessage($"Sorry, but something went wrong.\n\n{ex.Message}");
		}
	}
}
