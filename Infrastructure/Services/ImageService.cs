using Framework.Interfaces.Services;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;

        public ImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> DownloadFile(string fileUrl, string destination)
        {
            using (var result = await _httpClient.GetAsync(fileUrl).ConfigureAwait(false))
            {
                if (result.IsSuccessStatusCode)
                {
                    await File.WriteAllBytesAsync(destination, await result.Content.ReadAsByteArrayAsync().ConfigureAwait(false))
                        .ConfigureAwait(false);
                }
            }
            return "done";
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}