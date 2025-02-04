using ConsoleAppFramework;

[RegisterCommands("project")]
public class ProjectApp
{
	private readonly ProjectService _projectService = new(new StorageService());

	/// <summary>
	///     Lists projects in the global registry
	/// </summary>
	[Command("list")]
	public async Task ProjectList()
	{
		var projectRegistry = await _projectService.GetProjectRegistryAsync();
		foreach (var item in projectRegistry.Projects)
		{
			Console.WriteLine($"{item.ProjectId} {item.Version}");
		}
	}

	/// <summary>
	///     Adds new tfvars set to local registry for further upload
	/// </summary>
	/// <param name="projectId">Current project id</param>
	[Command("add")]
	public async Task ProjectAdd(string projectId)
	{
		var projectRegistry = await _projectService.GetProjectRegistryAsync();
		var projects = projectRegistry.Projects.ToList();
		projects.Add(new ProjectItem(GlobalOptions.ProjectId(projectId)));
		await _projectService.UpdateProjectRegistryAsync(new ProjectRegistry(projects));
	}
}
