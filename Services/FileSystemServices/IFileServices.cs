using Microsoft.AspNetCore.Http;
using Services.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FileSystemServices
{
    public interface IFileServices
    {

        public Task<FileSystemResult> AddImageAsync(string path, IFormFile file);
        public Task<FileSystemResult> DeleteImageAsync(string path, string? Folder = null);
        public Task<FileSystemResult> AddRangeImageAsync(string path, List<IFormFile> files);
        public Task<FileSystemResult> UpdateImage(string OldImage,string path, IFormFile file);
        public Task<FileSystemResult> DeleteRangeImageAsync(List<string> path);


    }
}
