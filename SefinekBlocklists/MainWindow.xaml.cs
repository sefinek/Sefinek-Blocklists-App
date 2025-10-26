using System.IO;
using Microsoft.Web.WebView2.Core;
using SefinekBlocklists.Scripts;

namespace SefinekBlocklists;

public sealed partial class MainWindow
{
	private const string DefaultUrl = "https://sefinek.net/blocklist-generator";
	private static readonly string AppDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sefinek Blocklists");
	private static readonly string SettingsPath = Path.Combine(AppDataDir, "settings.ini");

	public MainWindow()
	{
		InitializeComponent();
		Loaded += async (_, _) => await InitializeWebView();
	}

	private async Task InitializeWebView()
	{
		Title += $" v{Utils.AppFileVersion}";
		Directory.CreateDirectory(AppDataDir);

		webView.CoreWebView2InitializationCompleted += OnWebViewInitialized;
		webView.NavigationCompleted += OnNavigationCompleted;

		CoreWebView2Environment? environment = await CoreWebView2Environment.CreateAsync(null, AppDataDir).ConfigureAwait(true);
		await webView.EnsureCoreWebView2Async(environment).ConfigureAwait(true);

		CoreWebView2Settings? settings = webView.CoreWebView2.Settings;
		settings.UserAgent += $" SefinekBlocklists/{Utils.AppFileVersion}";
		settings.AreDevToolsEnabled = false;
		settings.IsStatusBarEnabled = false;
		settings.AreHostObjectsAllowed = false;
		settings.IsPasswordAutosaveEnabled = false;
	}

	private void OnWebViewInitialized(object? sender, CoreWebView2InitializationCompletedEventArgs e)
	{
		if (!e.IsSuccess)
		{
			Utils.ShowErrorMessage("Failed to initialize WebView2.");
			return;
		}

		var url = LoadUrlFromSettings() ?? DefaultUrl;
		SaveUrlToSettings(url);
		webView.Source = new Uri(url);
	}

	private void OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		if (e.IsSuccess)
			SaveUrlToSettings(webView.Source.ToString());
		else
			Utils.ShowErrorMessage("Failed to load the webpage.");
	}

	private static string? LoadUrlFromSettings()
	{
		return IniFile.Read(SettingsPath, "Settings", "CurrentUrl");
	}

	private static void SaveUrlToSettings(string url)
	{
		IniFile.Write(SettingsPath, "Settings", "CurrentUrl", url);
	}
}
