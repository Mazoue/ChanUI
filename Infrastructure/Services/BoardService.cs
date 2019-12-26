using Framework.Datamodels;
using Framework.Interfaces.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace Infrastructure.Services
{
    public class BoardService : IBoardService
    {
        private readonly HttpClient _httpClient;

        public BoardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<AllBoards> GetAllBoards()
        {
            return await JsonSerializer.DeserializeAsync<AllBoards>
                (await _httpClient.GetStreamAsync($"boards"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Catalogue>> GetBoardCatalog(string board)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Catalogue>>(await _httpClient.GetStreamAsync($"/{board}/catalog.json"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);
        }


        public async Task<ThreadPosts> GetThreadPosts(string board, string threadNumber)
        {
            return await JsonSerializer.DeserializeAsync<ThreadPosts>(await _httpClient.GetStreamAsync($"/{board}/thread/{threadNumber}.json"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
