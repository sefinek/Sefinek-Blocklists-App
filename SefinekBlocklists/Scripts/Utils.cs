namespace SefinekBlocklists.Scripts;

internal static class Utils
{
	public static void ShowErrorMessage(string message)
	{
		MessageBox.Show(message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
	}
}
