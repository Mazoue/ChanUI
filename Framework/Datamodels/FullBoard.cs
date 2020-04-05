using System.Collections.Generic;

namespace Framework.Datamodels
{
    public class FullBoard
    {
        public string CurrentBoard { get; set; }
        public long CurrentThreadId { get; set;}
        public string CurrentThreadName { get; set; }
        public AllBoards Boards { get; set; }
        public IEnumerable<Catalogue> Threads { get; set; }
        public IEnumerable<Post> Posts { get; set; }

    }
}

