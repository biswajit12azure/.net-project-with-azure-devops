using MediatR;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Entities;
using WGL.Utility.BlobFiles.FileInterface;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Commands.FileUpload
{
    public class FileUploadMCUserAdditionalDataCommand : IRequest<Response<bool>>
    {
        public required AdditionalMC Data { get; set; }
    }
    public class UploadMCUserAdditionalDataCommandHandler : IRequestHandler<FileUploadMCUserAdditionalDataCommand, Response<bool>>
    {
        private readonly IBlobStorageRepository _blobStorageRepository;
        private readonly IAccountRepositoryAsync _accountRepository;

        public UploadMCUserAdditionalDataCommandHandler(IBlobStorageRepository blobStorageRepository, IAccountRepositoryAsync accountRepository)
        {
            _blobStorageRepository = blobStorageRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Response<bool>> Handle(FileUploadMCUserAdditionalDataCommand request, CancellationToken cancellationToken)
        {
            var validFileData = new List<Document>();
            // Upload files to Blob Storage and get URLs
            foreach (var file in request.Data.FileData)
            {
                var blobUrl = await _blobStorageRepository.UploadFileAsync(request.Data.UserID, file.FileName!, Convert.FromBase64String(file.File!));
                file.Url = blobUrl; // Assign Blob URL to the file                
                validFileData.Add(file);
                
            }
            if (validFileData.Count > 0)
            {
                // Insert into AdditionalMC table
                int additionalID=await _accountRepository.InsertAdditionalMCUserDataAsync(request.Data);

                // Insert into Document table
                await _accountRepository.UploadUserDocumentsAsync(request.Data.FileData, additionalID);

                return new Response<bool>(true, message: "insert succussfully");
            }
            return new Response<bool>(false, message: "No valid files to process.");


        }
    }

}
