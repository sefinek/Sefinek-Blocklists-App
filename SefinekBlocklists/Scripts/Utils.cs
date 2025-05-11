using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace SefinekBlocklists.Scripts;

internal static class Utils
{
	public static readonly string? AppFileVersion = FileVersionInfo
		.GetVersionInfo(Assembly.GetExecutingAssembly().Location)
		.FileVersion;

	public static void ShowErrorMessage(string message)
	{
		MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
	}
}
