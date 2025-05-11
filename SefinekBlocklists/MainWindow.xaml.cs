using System.IO;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using SefinekBlocklists.Models;
using SefinekBlocklists.Scripts;

namespace SefinekBlocklists;

public sealed partial class MainWindow
{
	private const string DefaultUrl = "https://sefinek.net/blocklist-generator";
	private static readonly string AppDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sefinek Blocklists");
	private static readonly string SettingsPath = Path.Combine(AppDataDir, "settings.json");

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

		CoreWebView2Environment? environment = await CoreWebView2Environment.CreateAsync(null, AppDataDir);
		await webView.EnsureCoreWebView2Async(environment);

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

		string url = LoadUrlFromSettings() ?? DefaultUrl;
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
		if (!File.Exists(SettingsPath))
			return null;

		try
		{
			Settings? settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath));
			return settings?.CurrentUrl;
		}
		catch
		{
			return null;
		}
	}

	private static void SaveUrlToSettings(string url)
	{
		try
		{
			string json = JsonConvert.SerializeObject(new Settings { CurrentUrl = url }, Formatting.Indented);
			File.WriteAllText(SettingsPath, json);
		}
		catch (Exception ex)
		{
			Utils.ShowErrorMessage($"Error saving URL to settings:\n{ex.Message}");
		}
	}
}
