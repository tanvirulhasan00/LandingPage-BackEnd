using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LandingPage.Repositories.IRepositories
{
    public interface IFileRepository
    {
        Task<string> FileUpload(IFormFile file, string folderName);
        void DeleteFile(string fileUrl);

    }
}