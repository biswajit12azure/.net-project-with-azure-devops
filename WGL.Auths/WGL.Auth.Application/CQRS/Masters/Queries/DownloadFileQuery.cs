using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.DTOs.Master;
using WGL.Utility.BlobFiles.FileInterface;

namespace WGL.Auth.Application.CQRS.Masters.Queries
{
    public class DownloadFileQuery : IRequest<BlobDownloadFileResponse>
    {
        public string FileName { get; set; }

        public DownloadFileQuery(string fileName)
        {
            FileName = fileName;
        }
    }
    public class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, BlobDownloadFileResponse>
    {
        private readonly IBlobStorageRepository _blobStorageRepository;

        public DownloadFileQueryHandler(IBlobStorageRepository blobStorageRepository)
        {
            _blobStorageRepository = blobStorageRepository;
        }

        public async Task<BlobDownloadFileResponse> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            var fileName = await _blobStorageRepository.DownloadFileAsync(request.FileName);
            var base64File = Convert.ToBase64String(fileName);
            var fileFormat = Path.GetExtension(request.FileName).TrimStart('.');

            var response = new BlobDownloadFileResponse
            {
                FileName = request.FileName,
                Format = fileFormat,
                File = base64File
            };
            return response;
        }
    }

}
