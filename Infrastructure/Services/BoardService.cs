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
            //CATCH RESPONSE
            return await JsonSerializer.DeserializeAsync<AllBoards>
                (await _httpClient.GetStreamAsync($"boards"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Catalogue>> GetBoardCatalog(string board)
        {
            //CATCH RESPONSE
            var catalog = await JsonSerializer.DeserializeAsync<IEnumerable<Catalogue>>(await _httpClient.GetStreamAsync($"catalog/{board}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);
            return catalog;
        }


        public async Task<ThreadPosts> GetThreadPosts(string board, long threadNumber)
        {
            //CATCH RESPONSE
            return await JsonSerializer
                .DeserializeAsync<ThreadPosts>(
                    await _httpClient.GetStreamAsync($"posts?board={board}&threadnumber={threadNumber}"),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);

        }

    }
}
