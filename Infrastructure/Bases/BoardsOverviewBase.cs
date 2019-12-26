using System;
using System.Collections.Generic;
using System.Text;
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
        

        
    }
}