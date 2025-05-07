using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WGL.Utility.BlobFiles.FileInterface;

namespace WGL.Utility.BlobFiles.FileRepository
{
    public class BlobFiles
    {
        public string blobContainerName { get; set; } = "wglcontainer";
        public string connectionString { get; set; } = "DefaultEndpointsProtocol=https;AccountName=wgltest;AccountKey=EUERJKaFUDxhdRMyUoPhs7la2Hz6M+BraSLhmwyc7DEzhP5sXwOzKaCcOcwrAPd8itdReWq2sewq+AStDTviHQ==;EndpointSuffix=core.windows.net";
    }
    public class BlobStorageRepository : IBlobStorageRepository
    {
      
        private readonly string _blobContainerName;
        private readonly string _connectionString;
        BlobFiles files = new();
        public BlobStorageRepository()
        {
            _blobContainerName = files.blobContainerName;
            _connectionString = files.connectionString;
        }
        public async Task<string> UploadFileAsync(int UserID, string fileName, byte[] fileData)
        {
            var fileExtension= Path.GetExtension(fileName);
            var fileNameWithoutExtention = Path.GetFileNameWithoutExtension(fileName);

            var UserFileName = fileNameWithoutExtention + "_" + UserID + fileExtension;
            var blobConnectionString = new BlobServiceClient(_connectionString);
            var containerClient = blobConnectionString.GetBlobContainerClient(_blobContainerName);            
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);           
            var blobClient = containerClient.GetBlobClient(UserFileName);
            
            using (var stream = new MemoryStream(fileData))
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }
            return UserFileName;
        }
        
        public async Task<byte[]> DownloadFileAsync(string fileName)
        {
            var _blobServiceClient =new BlobServiceClient(_connectionString);
            var containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            if (await blobClient.ExistsAsync())
            {
                using (var stream = new MemoryStream())
                {
                    await blobClient.DownloadToAsync(stream);
                    return stream.ToArray();
                }
            }
            throw new FileNotFoundException(fileName + "File not found in blob storage");
        }
        public async Task DeleteFileAsync(string fileName)
        {
            var _blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = _blobServiceClient?.GetBlobContainerClient(_blobContainerName);
            var blobClient = containerClient?.GetBlobClient(fileName);
            await blobClient!.DeleteIfExistsAsync();
        }
    }
}
