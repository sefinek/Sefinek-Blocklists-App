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
			var inSection = false;

			foreach (var line in File.ReadLines(filePath))
			{
				var trimmed = line.Trim();

				if (trimmed.StartsWith('[') && trimmed.EndsWith(']'))
				{
					inSection = trimmed[1..^1].Equals(section, StringComparison.OrdinalIgnoreCase);
					continue;
				}

				if (!inSection || !trimmed.Contains('=')) continue;

				var separatorIndex = trimmed.IndexOf('=');
				var currentKey = trimmed[..separatorIndex].Trim();

				if (currentKey.Equals(key, StringComparison.OrdinalIgnoreCase))
					return trimmed[(separatorIndex + 1)..].Trim();
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
			List<string> lines = File.Exists(filePath) ? [..File.ReadAllLines(filePath)] : [];
			var sectionFound = false;
			var keyUpdated = false;
			var sectionIndex = -1;

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

					if (currentSection.Equals(section, StringComparison.OrdinalIgnoreCase))
					{
						sectionFound = true;
						sectionIndex = i;
					}

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
				var insertIndex = sectionIndex + 1;
				while (insertIndex < lines.Count && !lines[insertIndex].Trim().StartsWith('['))
					insertIndex++;

				lines.Insert(insertIndex, $"{key}={value}");
			}

			File.WriteAllLines(filePath, lines, Encoding.UTF8);
		}
		catch (Exception ex)
		{
			Utils.ShowErrorMessage($"Error writing to INI file:\n{ex.Message}");
		}
	}
}
