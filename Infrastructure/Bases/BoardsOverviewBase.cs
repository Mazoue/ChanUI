using Framework.Datamodels;
using Framework.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.IO;
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
            var posts = await BoardDataService.GetThreadPosts(board, threadNumber).ConfigureAwait(false);
            FullBoard.Posts = posts.Posts;
        }

        protected int ConvertBytesToKiloBytes(int bytes)
        {
            return ImageDataService.CalculateFileSizeInKiloBytes(bytes);
        }

        
        protected async Task DownloadPost(string baseFolder, string boardName, string threadName, Post post)
        {
            //Base Folder
            System.IO.Directory.CreateDirectory(baseFolder);

            //Base Folder + BoardName
            var baseFolderBoardName = Path.Combine(baseFolder, boardName);
            System.IO.Directory.CreateDirectory(baseFolderBoardName);

            //Base Folder + BoardName + ThreadName
            threadName = CleanInput(threadName);
            if (!string.IsNullOrEmpty(threadName))
            {
                var baseFolderBoardNameThreadName = Path.Combine(baseFolderBoardName, threadName);
                System.IO.Directory.CreateDirectory(baseFolderBoardNameThreadName);

                //PostName + Extension
                var postName = CleanInput(post.filename);
                if (!string.IsNullOrEmpty(postName))
                {
                    var postNameAndExtension = $"{post.filename}{post.ext}";
                    var filePath = Path.Combine(baseFolderBoardNameThreadName, postNameAndExtension);
                    var fileUrl = $"{boardName}/{post.tim}{post.ext}";

                    await ImageDataService.DownloadFile(fileUrl, filePath).ConfigureAwait(false);
                }
            }
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
                return string.Empty;
            }
        }

    }
}