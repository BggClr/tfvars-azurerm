using ConsoleAppFramework;

namespace Tfvars.Azurerm;

internal class InitFilter(ConsoleAppFilter next) : ConsoleAppFilter(next)
{
	public override async Task InvokeAsync(ConsoleAppContext context, CancellationToken cancellationToken)
	{
		if (!context.Arguments.Contains("init", StringComparer.InvariantCultureIgnoreCase)
			&& !File.Exists(GlobalOptions.SettingsPath))
		{
			ConsoleApp.LogError("use init command before do any actions");
		}

		await Next.InvokeAsync(context, cancellationToken);
	}
}
