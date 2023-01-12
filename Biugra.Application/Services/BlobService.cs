using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Services;
public class BlobService : IBlobService
{

    private readonly BlobServiceClient blobServiceClient;
    private readonly ILogger<BlobService> _logger;
    public BlobService(IConfiguration configuration, ILogger<BlobService> logger)
    {
        blobServiceClient = new BlobServiceClient(configuration["ConnectionStrings:BlobConnection"]);
        _logger = logger;
    }
    public async Task<CommandResult<string>> UploadAsync(string containerName, string blobName, IFormFile file)
    {
        try
        {
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync();

            BlobClient blob = container.GetBlobClient(blobName);
            await using var data = file.OpenReadStream();
            await blob.DeleteIfExistsAsync();
            var result = await blob.UploadAsync(data);

            return result.GetRawResponse().IsError
                ? CommandResult<string>.GetFailed(result.GetRawResponse().ReasonPhrase)
                : CommandResult<string>.GetSucceed(blob.Uri.AbsoluteUri);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BlobService ERROR --- RequestType : {RequestType} -- Message : {message} ---  Container : {Container}",
                nameof(UploadAsync), ex.Message, containerName);
            return CommandResult<string>.GetFailed(ex);
        }

    }
    public async Task<Stream> DownloadAsync(string containerName, string fileName)
    {
        try
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            BlobDownloadInfo download = await blobClient.DownloadAsync();
            return download.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BlobService ERROR --- RequestType : {RequestType} -- Message : {message} ---  Container : {Container}",
                nameof(DownloadAsync), ex.Message, containerName);
            return null;
        }
    }
    public async Task<CommandResult<string>> RemoveAsync(string containerName, string fileName)
    {
        try
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            var result = await blobClient.DeleteIfExistsAsync();

            return result.Value
                ? CommandResult<string>.GetSucceed(string.Empty)
                : CommandResult<string>.GetFailed(result.GetRawResponse()?.ReasonPhrase);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BlobService ERROR --- RequestType : {RequestType} -- Message : {message} ---  Container : {Container}",
                nameof(RemoveAsync), ex.Message, containerName);
            return CommandResult<string>.GetFailed(ex);
        }


    }

    public async Task<CommandResult<string>> TransferAsync(string containerName, string fileName, string destinationContainer)
    {
        try
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobContainerClient destinationClient = blobServiceClient.GetBlobContainerClient(destinationContainer);
            await destinationClient.CreateIfNotExistsAsync();

            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            BlobClient destinationBlobClient = destinationClient.GetBlobClient(fileName);

            await destinationBlobClient.DeleteIfExistsAsync();
            var result = await destinationBlobClient.SyncCopyFromUriAsync(blobClient.Uri);

            if (result.GetRawResponse().IsError)
                return CommandResult<string>.GetFailed(result.GetRawResponse().ReasonPhrase);

            await blobClient.DeleteIfExistsAsync();

            return CommandResult<string>.GetSucceed(destinationBlobClient.Uri.AbsoluteUri);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BlobService ERROR --- RequestType : {RequestType} -- Message : {message} ---  Container : {Container}",
                nameof(TransferAsync), ex.Message, containerName);
            return CommandResult<string>.GetFailed(ex);
        }
    }
}
