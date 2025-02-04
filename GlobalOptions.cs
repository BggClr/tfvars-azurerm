using System.Text.Json;

public static class GlobalOptions
{
	public static JsonSerializerOptions JsonSerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true
	};

	public static string SettingsPath
	{
		get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			".tf-azurerm-vars.json");
	}

	public static string ProjectRegistryFileName
	{
		get => "project-registry.json";
	}

	public static string LocalProjectFileName
	{
		get => ".tf-azurerm-vars.json";
	}

	public static string ProjectId(string projectId)
	{
		return projectId.Replace(" ", "-").ToLowerInvariant();
	}
}
