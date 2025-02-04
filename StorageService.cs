using Azure.Identity;
using Azure.Storage.Blobs;

public class StorageService : ServiceBase
{
	public async Task<BlobContainerClient> GetContainerClientAsync()
	{
		var settings = await GetSettings();
		var accountUri = new Uri($"https://{settings.AccountName}.blob.core.windows.net/");
		var client = new BlobServiceClient(accountUri, new AzureCliCredential());

		var containerClient = client.GetBlobContainerClient(settings.Container);
		await containerClient.CreateIfNotExistsAsync();

		return containerClient;
	}

	public async Task<BlobClient> GetBlobClientAsync(string blob)
	{
		var containerClient = await GetContainerClientAsync();
		return containerClient.GetBlobClient(blob);
	}
}
