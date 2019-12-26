using System.Collections.Generic;

namespace Framework.Datamodels
{
    public class FullBoard
    {
        public AllBoards Boards { get; set; }
        public IEnumerable<Catalogue> Threads { get; set; }
        public IEnumerable<Post> Posts { get; set; }

    }
}

