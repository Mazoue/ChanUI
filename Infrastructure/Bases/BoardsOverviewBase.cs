using Framework.Datamodels;
using Framework.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Bases
{
    public class BoardsOverviewBase : ComponentBase
    {
        [Inject]
        public IBoardService BoardDataService { get; set; }
        [Inject]
        public IImageService ImageDataService { get; set; }

        public FullBoard FullBoard { get; set; }


        protected override async Task OnInitializedAsync()
        {
            FullBoard = new FullBoard { Boards = await BoardDataService.GetAllBoards().ConfigureAwait(false) };

        }

        protected async Task GetThreads(string boardName)
        {
            FullBoard.Threads = await BoardDataService.GetBoardCatalog(boardName).ConfigureAwait(false);
        }

        protected async Task GetThreadPosts(string board, long threadNumber)
        {
            var posts = await BoardDataService.GetThreadPosts(board,threadNumber).ConfigureAwait(false);
            FullBoard.Posts = posts.Posts;
        }

        protected int ConvertBytesToKiloBytes(int bytes)
        {
            return ImageDataService.CalculateFileSizeInKiloBytes(bytes);
        }

        private string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^\w\.@-]", "",
                    RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

    }
}