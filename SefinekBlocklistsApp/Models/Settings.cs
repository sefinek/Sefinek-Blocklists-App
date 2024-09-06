using Newtonsoft.Json;

namespace SefinekBlocklistsApp.Models;

public class Settings
{
	[JsonProperty("currenturl")] public string? CurrentUrl { get; set; }
}
