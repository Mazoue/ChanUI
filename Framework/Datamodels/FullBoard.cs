using System.Collections.Generic;
using Framework.Enums;

namespace Framework.Datamodels
{
    public class FullBoard
    {
        public LoadingStage CurrentStage { get; set; } = LoadingStage.Initializing;
       
        public string CurrentBoard { get; set; }
        public long CurrentThreadId { get; set;}
        public string CurrentThreadName { get; set; }
        public AllBoards Boards { get; set; }
        public IEnumerable<Catalogue> Threads { get; set; }
        public IEnumerable<Post> Posts { get; set; }

    }
}

