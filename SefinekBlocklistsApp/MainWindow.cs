using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using SefinekBlocklistsApp.Models;
using SefinekBlocklistsApp.Scripts;

namespace SefinekBlocklistsApp;

public sealed partial class MainWindow : Form
{
	private static readonly string AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sefinek Blocklists");
	private static readonly string JsonFilePath = Path.Combine(AppData, "settings.json");
	private static readonly string DefaultUrl = "https://sefinek.net/blocklist-generator";

	public MainWindow()
	{
		InitializeComponent();
	}

	private async void MainWindow_Load(object sender, EventArgs e)
	{
		webView21.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;

		CoreWebView2Environment webView2Environment = await CoreWebView2Environment.CreateAsync(null, AppData);
		await webView21.EnsureCoreWebView2Async(webView2Environment);

		webView21.CoreWebView2.Settings.UserAgent += $" SefinekBlocklists/{Program.AppFileVersion}";
		webView21.NavigationCompleted += WebView_NavigationCompleted;
	}

	private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
	{
		if (e.IsSuccess)
			try
			{
				if (File.Exists(JsonFilePath))
				{
					string file = File.ReadAllText(JsonFilePath);
					Settings? settings = JsonConvert.DeserializeObject<Settings>(file);

					if (settings != null && !string.IsNullOrEmpty(settings.CurrentUrl))
						webView21.Source = new Uri(settings.CurrentUrl);
					else
						webView21.Source = new Uri(DefaultUrl);
				}
				else
				{
					Settings defaultSettings = new()
					{
						CurrentUrl = DefaultUrl
					};
					string defaultJson = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented);
					File.WriteAllText(JsonFilePath, defaultJson);

					webView21.Source = new Uri(DefaultUrl);
				}
			}
			catch (Exception ex)
			{
				Utils.ShowErrorMessage($@"Error: {ex.Message}");
			}
		else
			Utils.ShowErrorMessage(@"Failed to load WebView2.");
	}

	private void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		if (e.IsSuccess)
			SaveCurrentUrlToJson(webView21.Source.ToString());
		else
			Utils.ShowErrorMessage(@"Failed to load the web page.");
	}

	private static void SaveCurrentUrlToJson(string url)
	{
		try
		{
			var urlObject = new { CurrentUrl = url };
			string json = JsonConvert.SerializeObject(urlObject);
			File.WriteAllText(JsonFilePath, json);
		}
		catch (Exception ex)
		{
			Utils.ShowErrorMessage(@$"Error while saving current URL to JSON: {ex.Message}");
		}
	}
}
