using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Application.DTOs.Master
{
    public class BlobDownloadFileResponse
    {
        public string FileName { get; set; }
        public string Format { get; set; }
        public string File { get; set; }
    }
}
