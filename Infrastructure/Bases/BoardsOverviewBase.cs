using Framework.Datamodels;
using Framework.Enums;
using Framework.Interfaces.Services;
using Framework.Interfaces.Settings;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Framework.Datamodels.Downloads;

namespace Infrastructure.Bases
{
    public class BoardsOverviewBase : ComponentBase
    {
        [Inject] private IBoardService BoardDataService { get; set; }
        [Inject] private IImageService ImageDataService { get; set; }
        [Inject] private IDataAccessSettings DataAccessSettings { get; set; }

        protected FullBoard FullBoard { get; set; }
        protected IEnumerable<Download> CurrentDownloads { get; set; }

        protected override async Task OnInitializedAsync()
        {
            FullBoard = new FullBoard
            {
                CurrentStage = LoadingStage.Boards,
                Boards = await BoardDataService.GetAllBoards().ConfigureAwait(false)
            };
            CurrentDownloads = new List<Download>();
        }

        private async Task GetThreads(string boardName)
        {
            FullBoard.Threads = await BoardDataService.GetBoardCatalog(boardName).ConfigureAwait(false);
        }

        private async Task GetThreadPosts(string board, long threadNumber)
        {
            var posts = await BoardDataService.GetThreadPosts(board, threadNumber).ConfigureAwait(false);
            FullBoard.Posts = posts.Posts;
        }

        protected int ConvertBytesToKiloBytes(int bytes)
        {
            return ImageDataService.CalculateFileSizeInKiloBytes(bytes);
        }

        #region Button Methods
        #region Boards

        protected async Task DownloadBoardPosts(string currentBoard)
        {
            FullBoard.CurrentBoard = currentBoard;
            await GetThreads(currentBoard).ConfigureAwait(false);
            foreach (var page in FullBoard.Threads)
            {
                foreach (var thread in page.threads)
                {
                    FullBoard.CurrentThreadId = thread.no;
                    FullBoard.CurrentThreadName = !string.IsNullOrEmpty(thread.sub) ? thread.sub : "Misc";
                    await GetThreadPosts(FullBoard.CurrentBoard, thread.no).ConfigureAwait(false);
                    await ParseBoardPosts(FullBoard.Posts, FullBoard.CurrentBoard, FullBoard.CurrentThreadName)
                        .ConfigureAwait(false);

                }
            }
        }

        protected async Task ExpandBoardThreads(string currentBoard)
        {
            FullBoard.CurrentBoard = currentBoard;
            await GetThreads(currentBoard).ConfigureAwait(false);
            FullBoard.CurrentStage = LoadingStage.Threads;
        }

        #endregion

        #region Threads
        protected async Task DownloadThreadPosts(Thread currentThread)
        {
            FullBoard.CurrentThreadId = currentThread.no;
            FullBoard.CurrentThreadName = !string.IsNullOrEmpty(currentThread.sub) ? currentThread.sub : "Misc";
            await GetThreadPosts(FullBoard.CurrentBoard, currentThread.no).ConfigureAwait(false);
            await ParseBoardPosts(FullBoard.Posts, FullBoard.CurrentBoard, FullBoard.CurrentThreadName)
                .ConfigureAwait(false);

        }

        protected async Task ExpandThreadPosts(Thread currentThread)
        {
            FullBoard.CurrentThreadId = currentThread.no;
            FullBoard.CurrentThreadName = !string.IsNullOrEmpty(currentThread.sub) ? currentThread.sub : "Misc";
            await GetThreadPosts(FullBoard.CurrentBoard, currentThread.no).ConfigureAwait(false);
            FullBoard.CurrentStage = LoadingStage.Posts;
        }

        #endregion

        #endregion

        private async Task ParseBoardPosts(IEnumerable<Post> boardPosts, string currentBoard, string currentThreadName)
        {
            foreach (var post in boardPosts)
            {
                if (post.fsize > 1)
                {
                    await DownloadPost(currentBoard, currentThreadName, post).ConfigureAwait(false);
                }
            }
        }

        protected async Task DownloadPost(string boardName, string threadName, Post post)
        {
            //Base Folder
            System.IO.Directory.CreateDirectory(DataAccessSettings.IoSettings.BaseFolder);

            //Base Folder + BoardName
            var baseFolderBoardName = Path.Combine(DataAccessSettings.IoSettings.BaseFolder, boardName);
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