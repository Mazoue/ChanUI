using Framework.Datamodels;
using Framework.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace Infrastructure.Services
{
    public class BoardService : IBoardService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BoardService> _logger;

        public BoardService(HttpClient httpClient, ILogger<BoardService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<AllBoards> GetAllBoards()
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<AllBoards>
                    (await _httpClient.GetStreamAsync($"boards"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to Get All Boards.", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Catalogue>> GetBoardCatalog(string board)
        {
            try
            {
                var catalog = await JsonSerializer.DeserializeAsync<IEnumerable<Catalogue>>(await _httpClient.GetStreamAsync($"catalog/{board}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);
                return catalog;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to Get Board Catalog.", ex);
                throw;
            }
        }


        public async Task<ThreadPosts> GetThreadPosts(string board, long threadNumber)
        {
            try
            {
                return await JsonSerializer
                    .DeserializeAsync<ThreadPosts>(
                        await _httpClient.GetStreamAsync($"posts?board={board}&threadnumber={threadNumber}"),
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to Get Thread Posts.", ex);
                throw;
            }
        }
    }
}
