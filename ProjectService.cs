using System.Text.Json;

public class ProjectService(StorageService storageService) : ServiceBase
{
	public async Task<ProjectRegistry> GetProjectRegistryAsync()
	{
		var containerClient = await storageService.GetContainerClientAsync();
		var blobClient = containerClient.GetBlobClient(GlobalOptions.ProjectRegistryFileName);

		if (await blobClient.ExistsAsync())
		{
			var download = await blobClient.DownloadContentAsync();

			return download.Value.Content.ToObjectFromJson<ProjectRegistry>(new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
		}

		return new ProjectRegistry(new List<ProjectItem>());
	}

	public async Task UpdateProjectRegistryAsync(ProjectRegistry projectRegistry)
	{
		var containerClient = await storageService.GetContainerClientAsync();
		var blobClient = containerClient.GetBlobClient(GlobalOptions.ProjectRegistryFileName);

		var updated = JsonSerializer.Serialize(projectRegistry, GlobalOptions.JsonSerializerOptions);
		await blobClient.UploadAsync(BinaryData.FromString(updated), true);
	}

	public async Task<ProjectItem> GetLocalProjectItemAsync()
	{
		if (File.Exists(GlobalOptions.LocalProjectFileName))
		{
			var project = await File.ReadAllTextAsync(GlobalOptions.LocalProjectFileName);
			return JsonSerializer.Deserialize<ProjectItem>(project, GlobalOptions.JsonSerializerOptions);
		}

		return null;
	}

	public async Task SetLocalProjectItemAsync(ProjectItem project)
	{
		await File.WriteAllTextAsync(GlobalOptions.LocalProjectFileName,
			JsonSerializer.Serialize(project, GlobalOptions.JsonSerializerOptions));
	}
}
