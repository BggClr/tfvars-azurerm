using ConsoleAppFramework;

[RegisterCommands("secrets")]
public class SecretApp
{
	private readonly ProjectService _projectService = new(new StorageService());
	private readonly StorageService _storageService = new();

	[Command("get")]
	public async Task GetSecrets(string projectId = null, string filename = "terraform.tfvars", bool overwrite = false,
		bool init = false)
	{
		var projectRegistry = await _projectService.GetProjectRegistryAsync();
		var project = !string.IsNullOrEmpty(projectId)
			? projectRegistry.Projects.FirstOrDefault(x => x.ProjectId == GlobalOptions.ProjectId(projectId))
			: await _projectService.GetLocalProjectItemAsync();

		if (project == null)
		{
			ConsoleApp.LogError($"{GlobalOptions.ProjectId(projectId)} not found");
			return;
		}

		if (project.Version == 0 && init == false)
		{
			ConsoleApp.LogError("The secrets were not uploaded yet.");
			return;
		}

		if (File.Exists(filename) && overwrite == false)
		{
			ConsoleApp.LogError($"{filename} already exists. Please use --overwrite true to overwrite it.");
			return;
		}

		var blobClient = await _storageService.GetBlobClientAsync($"{project.ProjectId}.tfvars".ToLowerInvariant());

		if (await blobClient.ExistsAsync())
		{
			await blobClient.DownloadToAsync(filename);
			await _projectService.SetLocalProjectItemAsync(project);
		}
		else
		{
			if (init == false)
			{
				ConsoleApp.LogError($"{filename} already exists. Please use --overwrite true to overwrite it.");
				return;
			}

			await _projectService.SetLocalProjectItemAsync(project);
		}
	}

	/// <summary>
	///     Save secrets to remote storage
	/// </summary>
	/// <param name="projectId">-p ProjectId</param>
	/// <param name="fileName">Filename to upload</param>
	[Command("upload")]
	public async Task UploadSecrets(string projectId = null, string fileName = "terraform.tfvars")
	{
		var projectRegistry = await _projectService.GetProjectRegistryAsync();
		var project = !string.IsNullOrEmpty(projectId)
			? projectRegistry.Projects.FirstOrDefault(x => x.ProjectId == GlobalOptions.ProjectId(projectId))
			: await _projectService.GetLocalProjectItemAsync();

		if (project == null)
		{
			ConsoleApp.LogError($"{GlobalOptions.ProjectId(projectId)} not found");
			return;
		}

		var newProjectVersion = new ProjectItem(project.ProjectId, project.Version + 1);
		await _projectService.SetLocalProjectItemAsync(newProjectVersion);

		var projects = projectRegistry.Projects.ToList();
		var updatedProjects = new List<ProjectItem>();

		foreach (var item in projects)
		{
			if (item.ProjectId == project.ProjectId)
			{
				if (item.Version >= newProjectVersion.Version)
				{
					ConsoleApp.LogError(
						"Conflict. The registry has the newer version. Please download latest changes to avoid loosing data.");
					return;
				}

				updatedProjects.Add(newProjectVersion);
			}
			else
			{
				updatedProjects.Add(item);
			}
		}

		await _projectService.UpdateProjectRegistryAsync(new ProjectRegistry(updatedProjects));

		var blobClient = await _storageService.GetBlobClientAsync($"{project.ProjectId}.tfvars".ToLowerInvariant());
		await blobClient.UploadAsync(fileName, true);
	}
}
