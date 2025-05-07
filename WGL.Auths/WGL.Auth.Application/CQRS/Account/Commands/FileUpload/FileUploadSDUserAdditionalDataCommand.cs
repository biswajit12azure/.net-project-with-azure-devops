using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Entities;
using WGL.Utility.BlobFiles.FileInterface;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Commands.FileUpload
{
    public class FileUploadSDUserAdditionalDataCommand : IRequest<Response<bool>>
    {
        public AdditionalSD Data { get; set; }
    }
    public class UploadSDUserAdditionalDataCommandHandler : IRequestHandler<FileUploadSDUserAdditionalDataCommand, Response<bool>>
    {
        private readonly IBlobStorageRepository _blobStorageRepository;
        private readonly IAccountRepositoryAsync _accountRepository;

        public UploadSDUserAdditionalDataCommandHandler(IBlobStorageRepository blobStorageRepository, IAccountRepositoryAsync accountRepository)
        {
            _blobStorageRepository = blobStorageRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Response<bool>> Handle(FileUploadSDUserAdditionalDataCommand request, CancellationToken cancellationToken)
        {
            var validFileData = new List<Document>();
            // Upload files to Blob Storage and get URLs
            foreach (var file in request.Data.FileData)
            {
                // byte[] fileBytes = await File.ReadAllBytesAsync(file.fileUrl);
                var blobUrl = await _blobStorageRepository.UploadFileAsync(request.Data.UserID,file.FileName!, Convert.FromBase64String(file.File!));
                //var blobUrl = await _blobStorageRepository.UploadFileAsync(file.File);
                file.Url = blobUrl; // Assign Blob URL to the file
                
                validFileData.Add(file);
            }
            if (validFileData.Count > 0)
            {
                // Insert into AdditionalMC table
                int addtional=await _accountRepository.InsertAdditionalSDUserDataAsync(request.Data);

                // Insert into Document table
                await _accountRepository.UploadUserDocumentsAsync(request.Data.FileData, addtional);

                return new Response<bool>(true, message: "insert succussfully");
            }
            return new Response<bool>(false, message: "No valid files to process.");

        }
    }
}
