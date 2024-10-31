using Newtonsoft.Json;

namespace SefinekBlocklists.Models;

public class Settings
{
	[JsonProperty("currenturl")] public string? CurrentUrl { get; set; }
}
