namespace aqay_apis.Services
{
    public interface IAzureBlobService
    {

        Task<string> UploadAsync(IFormFile file);
        Task DeleteAsync(string blobName);
        Task<Stream> DownloadAsync(string blobName);
    }
}
