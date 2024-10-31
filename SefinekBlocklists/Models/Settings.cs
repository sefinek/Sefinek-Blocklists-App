using Newtonsoft.Json;

namespace SefinekBlocklists.Models;

public class Settings
{
	[JsonProperty(nameof(CurrentUrl))] public required string CurrentUrl { get; set; }
}
