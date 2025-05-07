namespace WGL.Utility.BlobFiles.FileInterface
{
    public interface IBlobStorageRepository
    {
        Task<string> UploadFileAsync(int UserID,string fileName, byte[] fileData);                
        Task<byte[]> DownloadFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
    }
}
