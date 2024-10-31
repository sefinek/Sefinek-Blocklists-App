using Newtonsoft.Json;

namespace SefinekBlocklists.Models;

public class Settings
{
	[JsonProperty("CurrentUrl")] public string? CurrentUrl { get; set; }
}
