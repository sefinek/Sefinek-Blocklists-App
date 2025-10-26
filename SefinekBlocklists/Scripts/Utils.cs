using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace SefinekBlocklists.Scripts;

internal static class Utils
{
	public static readonly string? AppFileVersion = GetAppVersion();

	private static string? GetAppVersion()
	{
		try
		{
			return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
		}
		catch
		{
			return null;
		}
	}

	public static void ShowErrorMessage(string message) =>
		MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
}
