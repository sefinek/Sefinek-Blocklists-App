using System.IO;
using System.Text;

namespace SefinekBlocklists.Scripts;

internal static class IniFile
{
	public static string? Read(string filePath, string section, string key)
	{
		if (!File.Exists(filePath))
			return null;

		try
		{
			var lines = File.ReadAllLines(filePath);
			var inSection = false;

			foreach (var line in lines)
			{
				var trimmed = line.Trim();

				if (trimmed.StartsWith('[') && trimmed.EndsWith(']'))
				{
					var currentSection = trimmed[1..^1];
					inSection = currentSection.Equals(section, StringComparison.OrdinalIgnoreCase);
					continue;
				}

				if (!inSection || !trimmed.Contains('=')) continue;

				var separatorIndex = trimmed.IndexOf('=');
				var currentKey = trimmed[..separatorIndex].Trim();
				if (!currentKey.Equals(key, StringComparison.OrdinalIgnoreCase)) continue;

				var value = trimmed[(separatorIndex + 1)..].Trim();
				return value;
			}
		}
		catch
		{
			return null;
		}

		return null;
	}

	public static void Write(string filePath, string section, string key, string value)
	{
		try
		{
			List<string> lines = [];
			var sectionFound = false;
			var keyUpdated = false;

			if (File.Exists(filePath))
				lines.AddRange(File.ReadAllLines(filePath));

			for (var i = 0; i < lines.Count; i++)
			{
				var trimmed = lines[i].Trim();

				if (trimmed.StartsWith('[') && trimmed.EndsWith(']'))
				{
					var currentSection = trimmed[1..^1];

					if (sectionFound && !keyUpdated)
					{
						lines.Insert(i, $"{key}={value}");
						keyUpdated = true;
						break;
					}

					sectionFound = currentSection.Equals(section, StringComparison.OrdinalIgnoreCase);
					continue;
				}

				if (sectionFound && trimmed.Contains('='))
				{
					var separatorIndex = trimmed.IndexOf('=');
					var currentKey = trimmed[..separatorIndex].Trim();

					if (currentKey.Equals(key, StringComparison.OrdinalIgnoreCase))
					{
						lines[i] = $"{key}={value}";
						keyUpdated = true;
						break;
					}
				}
			}

			if (!sectionFound)
			{
				if (lines.Count > 0 && !string.IsNullOrWhiteSpace(lines[^1]))
					lines.Add(string.Empty);

				lines.Add($"[{section}]");
				lines.Add($"{key}={value}");
			}
			else if (!keyUpdated)
			{
				for (var i = 0; i < lines.Count; i++)
				{
					var trimmed = lines[i].Trim();
					if (trimmed.StartsWith('[') && trimmed.EndsWith(']'))
					{
						var currentSection = trimmed[1..^1];
						if (currentSection.Equals(section, StringComparison.OrdinalIgnoreCase))
						{
							var insertIndex = i + 1;
							while (insertIndex < lines.Count && !lines[insertIndex].Trim().StartsWith('['))
								insertIndex++;

							lines.Insert(insertIndex, $"{key}={value}");
							break;
						}
					}
				}
			}

			File.WriteAllLines(filePath, lines, Encoding.UTF8);
		}
		catch (Exception ex)
		{
			Utils.ShowErrorMessage($"Error writing to INI file:\n{ex.Message}");
		}
	}
}
