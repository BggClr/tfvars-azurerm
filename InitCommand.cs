using System.Text.Json;
using ConsoleAppFramework;

namespace Tfvars.Azurerm;

[RegisterCommands]
public class InitCommand
{
	/// <summary>
	///     Initializes local registry with data from blob
	///     correct storage/blob name should be specified
	/// </summary>
	/// <param name="accountName">-a, Storage account name.</param>
	/// <param name="container">-c, Storage account container =.</param>
	[Command("init")]
	public async Task InitAsync(string accountName, string container)
	{
		var settings = new Settings(accountName, container);
		await File.WriteAllTextAsync(GlobalOptions.SettingsPath,
			JsonSerializer.Serialize(settings, GlobalOptions.JsonSerializerOptions));
	}
}
