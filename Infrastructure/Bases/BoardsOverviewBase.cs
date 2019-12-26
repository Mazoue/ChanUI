using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Framework.Datamodels;
using Framework.Interfaces.Services;
using Microsoft.AspNetCore.Components;

namespace Infrastructure.Bases
{
    public class BoardsOverviewBase : ComponentBase
    {
        [Inject]
        public IBoardService BoardDataService { get; set; }
        public IImageService ImageDataService { get; set; }

        public FullBoard FullBoard { get; set; }
        

        protected override async Task OnInitializedAsync()
        {
            FullBoard = new FullBoard {Boards = await BoardDataService.GetAllBoards().ConfigureAwait(false)};

        }

        protected async Task GetThreads(string boardName)
        {
            FullBoard.Threads = await BoardDataService.GetBoardCatalog(boardName).ConfigureAwait(false);
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