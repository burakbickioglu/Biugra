
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Interfaces;

public interface IBlobService
{
    Task<CommandResult<string>> UploadAsync(string containerName, string blobName, IFormFile file);
    Task<Stream> DownloadAsync(string containerName, string fileName);
    Task<CommandResult<string>> RemoveAsync(string containerName, string fileName);
    Task<CommandResult<string>> TransferAsync(string containerName, string fileName, string destinationContainer);
}
