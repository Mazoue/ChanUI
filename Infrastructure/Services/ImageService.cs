using Framework.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ImageService> _logger;

        public ImageService(HttpClient httpClient, ILogger<ImageService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public string GenerateFilePath(string baseFolder, string boardName, string threadName, string fileName, string fileExtension)
        {
            var fullName = $"{fileName}.{fileExtension}";
            return Path.Combine(baseFolder, boardName, threadName, fullName);
        }

        public bool FolderExists(string path)
        {
            return Directory.Exists(path);
        }
        public bool FileNameInUseExists(string path)
        {
            return File.Exists(path);
        }

        public async Task DownloadFile(string fileUrl, string destination)
        {
            try
            {
                await _httpClient.GetAsync($"api/post/downloadfile?fileUrl={fileUrl}&destination={destination}")
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to DownloadFile.", ex);
                throw;
            }
        }

        public int CalculateFileSizeInKiloBytes(int bytes)
        {
            return bytes / 1024;
        }
    }
}