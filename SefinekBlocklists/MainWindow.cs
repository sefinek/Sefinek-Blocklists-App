using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using SefinekBlocklists.Models;
using SefinekBlocklists.Scripts;

namespace SefinekBlocklists;

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
		Text += $@" v{Program.AppFileVersion}";

		webView21.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
		await webView21.EnsureCoreWebView2Async(await CoreWebView2Environment.CreateAsync(null, AppData));
		webView21.CoreWebView2.Settings.UserAgent += $" SefinekBlocklists/{Program.AppFileVersion}";
		webView21.NavigationCompleted += WebView_NavigationCompleted;
	}

	private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
	{
		if (!e.IsSuccess)
		{
			Utils.ShowErrorMessage("Failed to initialize WebView2.");
			return;
		}

		try
		{
			string url = File.Exists(JsonFilePath)
				? JsonConvert.DeserializeObject<Settings>(File.ReadAllText(JsonFilePath))?.CurrentUrl ?? DefaultUrl
				: DefaultUrl;

			File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(new Settings { CurrentUrl = url }, Formatting.Indented));
			webView21.Source = new Uri(url);
		}
		catch (Exception ex)
		{
			Utils.ShowErrorMessage($"An error occurred: {ex.Message}");
		}
	}

	private void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		if (e.IsSuccess)
			SaveCurrentUrlToJson(webView21.Source.ToString());
		else
			Utils.ShowErrorMessage("Failed to load the webpage.");
	}

	private static void SaveCurrentUrlToJson(string url)
	{
		try
		{
			File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(new { CurrentUrl = url }));
		}
		catch (Exception ex)
		{
			Utils.ShowErrorMessage($"Error while saving the current URL to JSON: {ex.Message}");
		}
	}
}
