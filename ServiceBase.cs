using System.Text.Json;

public abstract class ServiceBase
{
	protected async Task<Settings> GetSettings()
	{
		var settings = await File.ReadAllTextAsync(GlobalOptions.SettingsPath);
		return JsonSerializer.Deserialize<Settings>(settings, GlobalOptions.JsonSerializerOptions);
	}
}
